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
            writer.WriteLine($"Prefix(:=<{schemaBase}#>)\n");
            writer.WriteLine($"Ontology(<{schemaBase}/{Name}>");
            foreach (string import in imports)
            {
                writer.WriteLine($"Import (<{schemaBase}/{import}>)");
            }
            foreach (KeyValuePair<string, string> e in defTypes)
            {
                defTypes.TryGetValue(e.Key, out string type);
                writer.WriteLine($"DatatypeDefinition(:{e.Key} {Schema.GetOwlPrimitiveType(type)})");
                writer.WriteLine($"AnnotationAssertion(rdfs:label :{e.Key} \"{e.Key}\"^^xsd:string)");
            }
            foreach (KeyValuePair<string, string> e in equivalentClasses)
            {
                writer.WriteLine($"EquivalentClasses(:{e.Key} {e.Value})");
                writer.WriteLine($"AnnotationAssertion(rdfs:label :{e.Key} \"{e.Key}\"^^xsd:string)");
            }
            foreach (SelectType type in selectTypes)
            {
                type.GenerateOWL(writer);
            }
            List<string> defList = new List<string>(defTypes.Keys);
            foreach (Enumeration enumeration in enumerations)
            {
                enumeration.GenerateOWL(writer);
            }
            foreach (Entity entity in entities)
            {
                entity.TypeDef = defList;
                entity.GenerateOWL(writer);
            }
            foreach (Property prop in properties)
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
            writer.WriteLine($"Declaration(Class(:{Name}))");
            writer.WriteLine($"AnnotationAssertion(rdfs:label :{Name} \"{Name}\"^^xsd:string)");
            StringBuilder builder = new StringBuilder($"EquivalentClasses(:{Name} ObjectOneOf(");
            foreach (string literal in literals)
            {
                builder.Append($":{literal} ");
                writer.WriteLine($"ClassAssertion(:{Name} :{literal})");
            }
            builder.Append("))");
            writer.WriteLine(builder.ToString());
        }
    }
    public partial class SelectType : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            if (types.Count() == 0) return;
            writer.WriteLine($"EquivalentClasses(:{Name} ObjectUnionOf(");
            foreach (string type in types)
            {
                writer.WriteLine($":{type}");
            }
            writer.WriteLine("))");
            writer.WriteLine($"AnnotationAssertion(rdfs:label :{Name} \"{Name}\"^^xsd:string)");
        }
    }
    public partial class Entity : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine($"Declaration(Class(:{Name}))");
            writer.WriteLine($"AnnotationAssertion(rdfs:label :{Name} \"{Name}\"^^xsd:string)");
            //sub types
            if (disjointUnion.Count > 0)
            {
                var query = disjointUnion.Select(x => ":" + x);
                string union = string.Join(" ", query.ToList());
                writer.WriteLine($"DisjointUnion(:{Name} {union})");
            }
            //super types
            foreach (string super in superTypes)
            {
                writer.WriteLine($"SubClassOf(:{Name} :{super})");
            }
        }
    }
    public partial class Property : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            bool isPrimitiveObjects = false;
            foreach(string s in objects)
            {
                if (Schema.primitiveTypes.Contains(s))
                {
                    isPrimitiveObjects = true;
                    break;
                }
            }
            if (isPrimitiveObjects)
            {
                ProcessPrimitiveAttributes(writer);
            } else
            {
                ProcessReferenceAttributes(writer);
            }
        }
        private void ProcessPrimitiveAttributes(TextWriter writer)
        {
            if (subjects.Count == 0 || objects.Count == 0) return;
            if (IsFunctional)
            {
                writer.WriteLine($"FunctionalDataProperty(:{Name})");
            } else
            {
                writer.WriteLine($"DataProperty(:{Name})");
            }
            writer.WriteLine($"AnnotationAssertion(rdfs:label :{Name} \"{Name}\"^^xsd:string)");
            if (subjects.Count == 1)
            {
                writer.WriteLine($"DataPropertyDomain(:{Name} :{subjects.First().Name})");
            }
            else
            {
                var _s = from subject in subjects select $":{subject.Name}";
                var s = string.Join(" ", _s);
                writer.WriteLine($"DataPropertyDomain(:{Name} ObjectUnionOf({s}))");
            }
            if (objects.Count == 1)
            {
                writer.WriteLine($"DataPropertyRange(:{Name} {Schema.GetOwlPrimitiveType(objects.First())})");
            }
            else
            {
                var _o = from obj in objects select $"{Schema.GetOwlPrimitiveType(obj)}";
                var o = string.Join(" ", _o);
                writer.WriteLine($"DataPropertyRange(:{Name} DataUnionOf({o})");
            }
            //TODO: process the whole lists as Object / Data unions
            //writer.WriteLine($"DataPropertyDomain(:{Name} :{subjects.First().Name})");
            //writer.WriteLine($"DataPropertyRange(:{Name} {Schema.GetOwlPrimitiveType(objects.First())})");
            //FIXME: process optionality
        }
        private void ProcessReferenceAttributes(TextWriter writer)
        {
            writer.WriteLine($"AnnotationAssertion(rdfs:label :{Name} \"{Name}\"^^xsd:string)");
            //TODO: process the whole lists as Object / Data unions
            if (subjects.Count == 0 || objects.Count == 0) return;
            if (subjects.Count == 1)
            {
                writer.WriteLine($"ObjectPropertyDomain(:{Name} :{subjects.First().Name})");
            } else
            {
                var _s = from subject in subjects select $":{subject.Name}";
                var s = string.Join(" ", _s);
                writer.WriteLine($"ObjectPropertyDomain(:{Name} ObjectUnionOf({s}))");
            }
            if (objects.Count == 1)
            {
                writer.WriteLine($"ObjectPropertyRange(:{Name} :{objects.First()})");
            } else
            {
                var _o = from obj in objects select $":{obj}";
                var o = string.Join(" ", _o);
                writer.WriteLine($"ObjectPropertyRange(:{Name} ObjectUnionOf({o}))");
            }
            
            //FIXME: process optionality
        }

    }
}
