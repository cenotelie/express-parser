using Hime.Redist;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Express_Model;
using Express;

namespace Express_Parser
{
    class TextGenerator
    {
        private TextWriter writer;
        private ASTNode root;
        private bool debug;

        private Schema schema;

        public TextGenerator(ASTNode root, string baseName, string outputFile, bool debug)
        {
            this.root = root;
            this.debug = debug;
            if (debug) writer = Console.Out;
            else writer = new StreamWriter(outputFile);
            this.schema = new Schema(baseName);
        }

        public void GenerateSchema()
        {
            this.ProcessNode(this.root);
        }
        public void GenerateOWl()
        {
            this.schema.GenerateOWl(this.writer);
            this.writer.Close();
            if (debug) Console.ReadKey();
        }
        private void ProcessNode(ASTNode node)
        {
            switch (node.Symbol.ID)
            {
                case ExpressParser.ID.VariableSchemaDecl:
                    this.ProcessSchema(node);
                    break;
                case ExpressParser.ID.VariableUseDecl:
                    this.ProcessUse(node);
                    break;
                case ExpressParser.ID.VariableTypeDecl:
                    this.ProcessType(node);
                    break;
                case ExpressParser.ID.VariableEntityDecl:
                    this.ProcessEntity(node);
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
            this.schema.Name = node.Children[0].Value;
        }
        private void ProcessUse(ASTNode node)
        {
            string use = node.Children[0].Value;
            //We assume that IRIs have the same base
            this.schema.AddImport(use);
        }
        private void ProcessType(ASTNode node)
        {
            ASTNode nameNode = node.Children[0];
            string name = nameNode.Children[0].Value;
            for (int i = 1; i < node.Children.Count; i++)
            {
                switch (node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableSelectDecl:
                        this.ProcessSelectType(name, node.Children[1]);
                        break;
                    case ExpressParser.ID.VariableWhereDecl:
                        break;
                    default:
                        this.ProcessPropertyExp(name, node.Children[1]);
                        break;
                }
            }
        }
        private void ProcessSelectType(string name, ASTNode node)
        {
            SelectType type = new SelectType(name);
            ASTNode nameNode;
            for (int i = 1; i < node.Children.Count; i++)
            {
                nameNode = node.Children[i];
                type.AddType(nameNode.Children[0].Value);
            }
            this.schema.AddSelectType(type);
        }
        private void ProcessPropertyExp(string name, ASTNode node)
        {
            ASTNode type;
            if (node.Children.Count == 1)
            {
                type = node.Children[0];
                if (type.Symbol.ID == ExpressParser.ID.VariablePtKeyword)
                {
                    this.schema.AddDefType(name, node.Children[0].Children[0].Value);
                } 
                else
                {
                    this.schema.AddEquivalentClasses(name, node.Children[0].Children[0].Value);
                }
            }
            else
                return; //TODO: process collections;
        }
        private void ProcessEntity(ASTNode node)
        {
            ASTNode nameNode = node.Children[0];
            Entity entity = new Entity(nameNode.Children[0].Value);
            for (int i = 1; i < node.Children.Count; i++)
            {
                switch(node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableAbstractDecl:
                        entity.Abstract = true;
                        break;
                    case ExpressParser.ID.VariableSubtypeDecl:
                        this.ProcessSubTypes(entity, node.Children[i]);
                        break;
                    case ExpressParser.ID.VariableSupertypeDecl:
                        this.ProcessInheritance(entity, node.Children[i]);
                        break;
                    case ExpressParser.ID.VariableWhereDecl:
                        this.ProcessSubTypes(entity, node.Children[i]);
                        break;
                    case ExpressParser.ID.VariablePropDecl:
                        this.ProcessProperty(entity, node.Children[i]);
                        break;
                    default:
                        break;
                }
            }
            this.schema.AddEntity(entity);
        }
        private void ProcessInheritance(Entity owner, ASTNode node)
        {
            ASTNode nameNode;
            for (int i = 0; i < node.Children.Count; i++)
            {
                nameNode = node.Children[i];
                owner.AddSuperType(nameNode.Children[0].Value);
            }
        }
        private void ProcessSubTypes(Entity owner, ASTNode node)
        {
            ASTNode selector = node.Children[0];
            ASTNode nameNode;
            //Only the case "oneof" is considered here
            for (int i = 1; i < selector.Children.Count; i++)
            {
                nameNode = selector.Children[i];
                owner.AddDisjointUnion(nameNode.Children[0].Value);
            }
        }
        private void ProcessProperty(Entity owner, ASTNode node)
        {
            ASTNode propNode = node.Children[0];
            string name;
            List<string> propsChain = new List<string>();
            ASTNode child;
            for (int i = 0; i < propNode.Children.Count; i++)
            {
                child = propNode.Children[i];
                switch (child.Symbol.ID)
                {
                    case ExpressParser.ID.VariablePropId:
                        propsChain.Add(child.Children[0].Value);
                        break;
                    default:
                        break;
                }
            }
            if (propsChain.Count == 1) name = propsChain[0];
            else return; //FIXME: process chaining of props or SELF
            Property property = new Property(name, owner);
            ASTNode type;
            for (int i = 1; i < node.Children.Count; i++)
            {
                switch (node.Children[i].Symbol.ID)
                {
                    case ExpressParser.ID.VariableOptionalDecl:
                        property.Optional = true;
                        break;
                    case ExpressParser.ID.VariablePropExp:
                        type = node.Children[i];
                        if (type.Children.Count == 1)
                        {
                            property.PType = type.Children[0].Children[0].Value;
                        }
                        else
                            return; //TODO: process collections;
                        break;
                    default:
                        break;
                }
            }
            owner.AddProperty(property);
        }

    }
}
