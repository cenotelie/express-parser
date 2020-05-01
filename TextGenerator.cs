﻿using Hime.Redist;
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
            string name = node.Children[0].Value;
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
                        this.ProcessPrimitiveType(name, node.Children[1]);
                        break;
                }
            }
        }
        private void ProcessSelectType(string name, ASTNode node)
        {
            SelectType type = new SelectType(name);
            for (int i = 1; i < node.Children.Count; i++)
            {
                type.AddType(node.Children[i].Value);
            }
            this.schema.AddSelectType(type);
        }
        private void ProcessPrimitiveType(string name, ASTNode node)
        {
            this.schema.AddDefType(name, node.Children[0].Value);
        }
        private void ProcessEntity(ASTNode node)
        {
            Entity entity = new Entity(node.Children[0].Value);
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
                    default:
                        this.ProcessAttribute(entity, node.Children[i]);
                        break;
                }
            }
            this.schema.AddEntity(entity);
        }
        private void ProcessInheritance(Entity owner, ASTNode node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                owner.AddSuperType(node.Children[i].Value);
            }
        }
        private void ProcessSubTypes(Entity owner, ASTNode node)
        {
            ASTNode selector = node.Children[0];
            //Only the case "oneof" is considered here
            for (int i = 1; i < selector.Children.Count; i++)
            {
                owner.AddDisjointUnion(selector.Children[i].Value);
            }
        }
        private void ProcessAttribute(Entity owner, ASTNode node)
        {
            Express_Model.Attribute attribute = new Express_Model.Attribute(node.Children[0].Value, owner);
            ASTNode type;
            if (node.Children.Count == 3)
            {
                attribute.Optional = true;
                type = node.Children[2];
            }
            else
            {
                attribute.Optional = false;
                type = node.Children[1];
            }
            attribute.AType = (type.Children.Count == 0) ? type.Value : type.Children[0].Value;
            owner.AddAttribute(attribute);
        }
    }
}
