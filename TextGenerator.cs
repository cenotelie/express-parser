using Hime.Redist;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Express_Model;

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
            switch (node.ToString())
            {
                case "schema_decl":
                    this.ProcessSchema(node);
                    break;
                case "use_decl":
                    this.ProcessUse(node);
                    break;
                case "type_decl":
                    this.ProcessType(node);
                    break;
                case "entity_decl":
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
            switch(node.Children[1].ToString())
            {
                case "select_decl":
                    this.ProcessSelectType(name, node.Children[1]);
                    break;
                default:
                    this.ProcessPrimitiveType(name, node.Children[1]);
                    break;
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
            //Attributes
            for (int i = 1; i < node.Children.Count; i++)
            {
                
                ProcessAttribute(entity, node.Children[i]);
            }
            this.schema.AddEntity(entity);
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
