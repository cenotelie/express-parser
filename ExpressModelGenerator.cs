using Hime.Redist;
using System;
using System.Collections.Generic;
using System.IO;
using Express_Model;
using Express;

namespace Express_Parser
{
    class ExpressModelGenerator
    {
        private readonly TextWriter writer;
        private readonly ASTNode root;
        private readonly bool debug;

        private readonly Schema schema;

        public ExpressModelGenerator(ASTNode root, string baseName, string inputFile, string outputFile, bool debug)
        {
            this.root = root;
            this.debug = debug;
            if (debug) writer = Console.Out;
            else writer = new StreamWriter(outputFile);
            schema = new Schema(baseName);
        }

        public void GenerateSchema()
        {
            ProcessNode(root);
        }
        public void GenerateOWl()
        {
            schema.GenerateOWL(writer);
            writer.Close();
            if (debug) Console.ReadKey();
        }
        private void ProcessNode(ASTNode node)
        {
            switch (node.Symbol.ID)
            {
                case ExpressParser.ID.VariableSchemaDecl:
                    ProcessSchema(node);
                    break;
                case ExpressParser.ID.VariableUseDecl:
                    ProcessUse(node);
                    break;
                case ExpressParser.ID.VariableTypeDecl:
                    ProcessType(node);
                    break;
                case ExpressParser.ID.VariableEntityDecl:
                    ProcessEntity(node);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < node.Children.Count; i++)
            {
                ProcessNode(node.Children[i]);
            }
        }
        private void ProcessSchema(ASTNode node)
        {
            schema.Name = node.Children[0].Value;
        }
        private void ProcessUse(ASTNode node)
        {
            string use = node.Children[0].Value;
            //We assume that IRIs have the same base
            schema.AddImport(use);
        }
        private void ProcessType(ASTNode node)
        {
            ASTNode nameNode = node.Children[0];
            string name = nameNode.Value;
            for (int i = 1; i < node.Children.Count; i++)
            {
                switch (node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableEnumDecl:
                        ProcessEnumeration(name, node.Children[1]);
                        break;
                    case ExpressParser.ID.VariableSelectDecl:
                        ProcessSelectType(name, node.Children[1]);
                        break;
                    case ExpressParser.ID.VariableWhereDecl:
                        break;
                    default:
                        ProcessPropertyExp(name, node.Children[1]);
                        break;
                }
            }
        }
        private void ProcessEnumeration(string name, ASTNode node)
        {
            Enumeration enumeration = new Enumeration(name);
            ASTNode nameNode;
            for (int i = 1; i < node.Children.Count; i++)
            {
                nameNode = node.Children[i];
                enumeration.AddLiteral(nameNode.Value);
            }
            schema.AddEnumeration(enumeration);
        }
        private void ProcessSelectType(string name, ASTNode node)
        {
            SelectType type = new SelectType(name);
            ASTNode nameNode;
            for (int i = 0; i < node.Children.Count; i++)
            {
                nameNode = node.Children[i];
                type.AddType(nameNode.Value);
            }
            schema.AddSelectType(type);
        }
        private void ProcessPropertyExp(string name, ASTNode node)
        {
            ASTNode type;
            if (node.Children.Count == 1)
            {
                type = node.Children[0];
                if (type.Symbol.ID == ExpressParser.ID.VariablePtKeyword)
                {
                    schema.AddDefType(name, node.Children[0].Children[0].Value);
                } 
                else
                {
                    schema.AddEquivalentClasses(name, node.Children[0].Value);
                }
            }
            else
                return;
        }
        private void ProcessEntity(ASTNode node)
        {
            ASTNode nameNode = node.Children[0];
            Entity entity = new Entity(nameNode.Value);
            for (int i = 1; i < node.Children.Count; i++)
            {
                switch(node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableSubtypeDecl:
                        if (node.Children[i].Children.Count > 0)
                        {
                            ProcessSubTypes(entity, node.Children[i].Children[0]);
                        }
                        break;
                    case ExpressParser.ID.VariableSupertypeDecl:
                        ProcessInheritance(entity, node.Children[i]);
                        break;
                    case ExpressParser.ID.VariableWhereDecl:
                        break;
                    case ExpressParser.ID.VariablePropDecl:
                        ProcessProperty(entity, node.Children[i]);
                        break;
                    default:
                        break;
                }
            }
            schema.AddEntity(entity);
        }
        private void ProcessInheritance(Entity owner, ASTNode node)
        {
            ASTNode nameNode;
            for (int i = 0; i < node.Children.Count; i++)
            {
                nameNode = node.Children[i];
                owner.AddSuperType(nameNode.Value);
            }
        }
        private void ProcessSubTypes(Entity owner, ASTNode node)
        {
            ASTNode nameNode;
            //OneOf Case
            if (node.Symbol.ID == ExpressParser.ID.VariableOneofExp)
            {
                for (int i = 1; i < node.Children.Count; i++)
                {
                    nameNode = node.Children[i];
                    owner.AddDisjointUnion(nameNode.Value);
                }
                return;
            }
            //AndOr Case
            if (node.Symbol.ID == ExpressParser.ID.VariableAndorExp)
            {
                for (int i = 1; i < node.Children.Count; i++)
                {
                    ProcessSubTypes(owner, node.Children[i]);
                }
            }
            //Single Inheritance
            if (node.Symbol.ID == ExpressLexer.ID.TerminalIdentifier)
            {
                //FIXME: what does this mean ?
            }
            
        }
        private void ProcessProperty(Entity owner, ASTNode node)
        {
            ASTNode propNode = node.Children[0]; //att_read_exp
            string name;
            List<string> propsChain = new List<string>();
            ASTNode child;
            for (int i = 0; i < propNode.Children.Count; i++)
            {
                child = propNode.Children[i];
                switch (child.Symbol.ID)
                {
                    case ExpressLexer.ID.TerminalIdentifier:
                        propsChain.Add(child.Value);
                        break;
                    default:
                        break;
                }
            }
            if (propsChain.Count == 1) name = propsChain[0];
            else return; //FIXME: other cases are related to restrictions of inherited properties (swrl ?)
            Property property = schema.GetProperty(name); //In case the property has already been defined
            ASTNode type;
            for (int i = 1; i < node.Children.Count; i++) //Optional and type_id nodes
            {
                switch (node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableOptionalDecl:
                        property.Optional = true;
                        break;
                    case ExpressParser.ID.VariableTypeId:
                        type = node.Children[i];
                        switch(type.Children[0].Symbol.ID)
                        {
                            case ExpressParser.ID.VariablePtKeyword:
                                //property = new DataProperty(name, owner, );
                                property.AddSubject(owner);
                                property.AddObject(type.Children[0].Children[0].Value);
                                property.IsFunctional = true;
                                break;
                            case ExpressLexer.ID.TerminalIdentifier:
                                //property = new ObjectProperty(name, owner, type.Children[0].Value);
                                property.AddSubject(owner);
                                property.AddObject(type.Children[0].Value);
                                property.IsFunctional = true;
                                break;
                            default:
                                //Collections: set / array / bag / list
                                property.AddSubject(owner);
                                ASTNode collectionTypeNode = type.Children[0];
                                //We do not consider collection of collections, to be improved if needed
                                if (collectionTypeNode.Children[1].Children.Count == 1)
                                {
                                    property.AddObject(collectionTypeNode.Children[1].Children[0].Value);
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
