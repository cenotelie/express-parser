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
                string type = "rdfs:Literal";
                this.defTypes.TryGetValue(e.Key, out type);
                writer.WriteLine($"DatatypeDefinition(:{e.Key} {Schema.GetPrimitiveType(type)})");
            }
            foreach (KeyValuePair<string, string> e in this.equivalentClasses)
            {
                writer.WriteLine($"EquivalentClasses(:{e.Key} :{e.Value})");
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
            //Attributes
            foreach (Property property in this.properties)
            {
                property.TypeDef = this.TypeDef;
                property.GenerateOWL(writer);
            }
        }
    }
    public partial class Property : IOwlGeneration
    {
        public void GenerateOWL(TextWriter writer)
        {
            if (Schema.primitiveTypes.Contains(this.PType) || this.TypeDef.Contains(this.PType))
            {
                this.ProcessPrimitiveAttribute(writer, Owner.Name, this.Name, this.Optional, this.PType);
                return;
            }
            this.ProcessReferenceAttribute(writer, Owner.Name, this.Name, this.Optional, this.PType);
        }
        private void ProcessPrimitiveAttribute(TextWriter writer, string entityName, string name, bool optional, string type)
        {
            writer.WriteLine($"DataPropertyDomain(:{name} :{entityName})");
            writer.WriteLine($"DataPropertyRange(:{name} {Schema.GetPrimitiveType(type)})");
            //FIXME: process optionality
        }
        private void ProcessReferenceAttribute(TextWriter writer, string entityName, string name, bool optional, string type)
        {
            writer.WriteLine($"ObjectPropertyDomain(:{name} :{entityName})");
            writer.WriteLine($"ObjectPropertyRange(:{name} :{type})");
            //FIXME: process optionality
        }

    }
}
