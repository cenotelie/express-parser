using Express;
using Hime.Redist;
using System;
using System.IO;
using System.Linq;

namespace Express_Parser
{
    class OwlGenerator: ExpressParser.Actions
    {
        private string baseName;
        private TextWriter writer;
        private bool debug = false;
        private string[] primitiveTypes = { "NUMBER", "STRING" };

        public OwlGenerator(string baseName, string fileName)
        {
            this.baseName = baseName;
            if (debug) writer = Console.Out;
            else writer = new StreamWriter(fileName);
            writer.WriteLine("Prefix(owl:=<http://www.w3.org/2002/07/owl#>)");
            writer.WriteLine("Prefix(rdf:=<http://www.w3.org/1999/02/22-rdf-syntax-ns#>)");
            writer.WriteLine("Prefix(rdfs:=<http://www.w3.org/2000/01/rdf-schema#>)");
            writer.WriteLine("Prefix(xml:=<http://www.w3.org/XML/1998/namespace>)");
            writer.WriteLine("Prefix(xsd:=<http://www.w3.org/2001/XMLSchema#>)");
        }

        public override void OnSchema(Symbol head, SemanticBody body)
        {
            string schema = body[0].Value;
            writer.WriteLine($"Prefix(:=<{this.baseName}#>)\n");
            writer.WriteLine($"Ontology(<{this.baseName}/{schema}>");
        }

        public override void OnUse(Symbol head, SemanticBody body)
        {
            //We assume that IRIs have the same base
            writer.WriteLine($"Import (<{this.baseName}/{body[0].Value}>)");
        }

        public override void OnSelectType(Symbol head, SemanticBody body)
        {
            writer.WriteLine($"EquivalentClasses(:{body[0].Value} ObjectUnionOf(");
            for (int i = 1; i < body.Length; i++)
            {
                writer.WriteLine($":{body[i].Value}");
            }
            writer.WriteLine("))");
        }

        public override void OnPrimitiveType(Symbol head, SemanticBody body)
        {
            writer.WriteLine($"DatatypeDefinition(:{body[0].Value} {this.GetPrimitiveType(body[1].Value)})");
        }

        public override void OnEntity(Symbol head, SemanticBody body)
        {
            writer.WriteLine($"Declaration(Class(:{body[0].Value}))");
        }

        public override void OnAttribute(Symbol head, SemanticBody body)
        {
            string name = body[0].Value;
            string type;
            bool optional = false;
            if ("OPTIONAL".Equals(body[1].Value))
            {
                optional = true;
                type = body[2].Value;
            }
            else
            {
                type = body[1].Value;
            }
            if (this.primitiveTypes.Contains(type))
            {
                this.ProcessPrimitiveAttribute(name, optional, type);
                return;
            }
            this.ProcessReferenceAttribute(name, optional, type);
        }

        private void ProcessPrimitiveAttribute(string name, bool optional, string type)
        {
            writer.WriteLine($"DataPropertyDomain(:{name} :{this.GetPrimitiveType(type)})");
            //FIXME: Retrieve parent node to get the range
        }

        private void ProcessReferenceAttribute(string name, bool optional, string type)
        {
            writer.WriteLine($"ObjectPropertyDomain(:{name} :{type})");
            //FIXME: Retrieve parent node to get the range
        }

        public void Close()
        {
            writer.WriteLine(")");
            writer.Close();
        }

        private string GetPrimitiveType(string key)
        {
            switch(key)
            {
                case "STRING":
                    return "xsd:string";
                case "NUMBER":
                    return "xsd:int";
                default:
                    return "rdfs:Literal";
            }
        }
    }
}
