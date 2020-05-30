using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Express_Model
{
    interface IOwlGeneration
    {
        void GenerateOWL(TextWriter writer);
    }
    public partial class Schema : IOwlGeneration
    {
        public static string GetOwlPrimitiveType(string key)
        {
            switch (key)
            {
                case "STRING":
                    return "xsd:string";
                case "NUMBER":
                    return "xsd:double";//TODO: integer or real
                case "BINARY":
                    return "xsd:base64Binary";//Base 32 in the last specification
                case "LOGICAL":
                    return "xsd:boolean";//TODO: Same as boolean with 'undefined' additional value
                case "BOOLEAN":
                    return "xsd:boolean";
                case "INTEGER":
                    return "xsd:int";
                case "REAL":
                    return "xsd:double";
                default:
                    return $":{key}";
            }
        }
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine("Prefix(owl:=<http://www.w3.org/2002/07/owl#>)");
            writer.WriteLine("Prefix(rdf:=<http://www.w3.org/1999/02/22-rdf-syntax-ns#>)");
            writer.WriteLine("Prefix(rdfs:=<http://www.w3.org/2000/01/rdf-schema#>)");
            writer.WriteLine("Prefix(xml:=<http://www.w3.org/XML/1998/namespace>)");
            writer.WriteLine("Prefix(xsd:=<http://www.w3.org/2001/XMLSchema#>)");
            writer.WriteLine($"Prefix(:=<{this.schemaBase}#>)\n");
            writer.WriteLine($"Ontology(<{this.schemaBase}/{this.Name}>");
            foreach (string import in this.imports)
            {
                writer.WriteLine($"Import (<{this.schemaBase}/{import}>)");
            }
            foreach (KeyValuePair<string, string> e in this.defTypes)
            {
                this.defTypes.TryGetValue(e.Key, out string type);
                writer.WriteLine($"DatatypeDefinition(:{e.Key} {Schema.GetOwlPrimitiveType(type)})");
            }
            foreach (KeyValuePair<string, string> e in this.equivalentClasses)
            {
                writer.WriteLine($"EquivalentClasses(:{e.Key} {e.Value})");
            }
            foreach (SelectType type in this.selectTypes)
            {
                type.GenerateOWL(writer);
            }
            List<string> defList = new List<string>(this.defTypes.Keys);
            foreach (Enumeration enumeration in this.enumerations)
            {
                enumeration.GenerateOWL(writer);
            }
            foreach (Entity entity in this.entities)
            {
                entity.TypeDef = defList;
                entity.GenerateOWL(writer);
            }
            foreach (Property prop in this.properties)
            {
                prop.GenerateOWL(writer);
            }
            writer.WriteLine(")");
        }
    }
    public partial class Enumeration : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine($"Declaration(Class(:{this.Name}))");
            StringBuilder builder = new StringBuilder($"EquivalentClasses(:{this.Name} ObjectOneOf(");
            foreach (string literal in this.literals)
            {
                builder.Append($":{literal} ");
                writer.WriteLine($"ClassAssertion(:{this.Name} :{literal})");
            }
            builder.Append("))");
            writer.WriteLine(builder.ToString());
        }
    }
    public partial class SelectType : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine($"EquivalentClasses(:{this.Name} ObjectUnionOf(");
            foreach (string type in this.types)
            {
                writer.WriteLine($":{type}");
            }
            writer.WriteLine("))");
        }
    }
    public partial class Entity : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine($"Declaration(Class(:{this.Name}))");
            //sub types
            if (this.disjointUnion.Count > 0)
            {
                var query = this.disjointUnion.Select(x => ":" + x);
                string union = string.Join(" ", query.ToList());
                writer.WriteLine($"DisjointUnion(:{this.Name} {union})");
            }
            //super types
            foreach (string super in this.superTypes)
            {
                writer.WriteLine($"SubClassOf(:{this.Name} :{super})");
            }
        }
    }
    public partial class Property : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            bool isPrimitiveObjects = false;
            foreach(string s in this.objects)
            {
                if (Schema.primitiveTypes.Contains(s))
                {
                    isPrimitiveObjects = true;
                    break;
                }
            }
            if (isPrimitiveObjects)
            {
                this.ProcessPrimitiveAttributes(writer);
            } else
            {
                this.ProcessReferenceAttributes(writer);
            }
        }
        private void ProcessPrimitiveAttributes(TextWriter writer)
        {
            if (this.subjects.Count == 0 || this.objects.Count == 0) return;
            if (this.IsFunctional)
            {
                writer.WriteLine($"FunctionalDataProperty(:{this.Name})");
            } else
            {
                writer.WriteLine($"DataProperty(:{this.Name})");
            }
            if (this.subjects.Count == 1)
            {
                writer.WriteLine($"DataPropertyDomain(:{this.Name} :{this.subjects.First().Name})");
            }
            else
            {
                var _s = from subject in this.subjects select $":{subject.Name}";
                var s = string.Join(" ", _s);
                writer.WriteLine($"DataPropertyDomain(:{this.Name} ObjectUnionOf({s}))");
            }
            if (this.objects.Count == 1)
            {
                writer.WriteLine($"DataPropertyRange(:{this.Name} {Schema.GetOwlPrimitiveType(this.objects.First())})");
            }
            else
            {
                var _o = from obj in this.objects select $"{Schema.GetOwlPrimitiveType(obj)}";
                var o = string.Join(" ", _o);
                writer.WriteLine($"DataPropertyRange(:{this.Name} DataUnionOf({o})");
            }
            //TODO: process the whole lists as Object / Data unions
            //writer.WriteLine($"DataPropertyDomain(:{this.Name} :{this.subjects.First().Name})");
            //writer.WriteLine($"DataPropertyRange(:{this.Name} {Schema.GetOwlPrimitiveType(this.objects.First())})");
            //FIXME: process optionality
        }
        private void ProcessReferenceAttributes(TextWriter writer)
        {
            //TODO: process the whole lists as Object / Data unions
            if (this.subjects.Count == 0 || this.objects.Count == 0) return;
            if (this.subjects.Count == 1)
            {
                writer.WriteLine($"ObjectPropertyDomain(:{this.Name} :{this.subjects.First().Name})");
            } else
            {
                var _s = from subject in this.subjects select $":{subject.Name}";
                var s = string.Join(" ", _s);
                writer.WriteLine($"ObjectPropertyDomain(:{this.Name} ObjectUnionOf({s}))");
            }
            if (this.objects.Count == 1)
            {
                writer.WriteLine($"ObjectPropertyRange(:{this.Name} :{this.objects.First()})");
            } else
            {
                var _o = from obj in this.objects select $":{obj}";
                var o = string.Join(" ", _o);
                writer.WriteLine($"ObjectPropertyRange(:{this.Name} :{this.objects.First()})");
            }
            
            //FIXME: process optionality
        }

    }
}
