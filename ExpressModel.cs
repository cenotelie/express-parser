using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Express_Model
{
    interface OwlGeneration
    {
        void GenerateOWl(TextWriter writer);
    }
    public abstract class NamedElement
    {
        public NamedElement(string name)
        {
            this.Name = name;
        }
        public string Name
        {
            get; set;
        }
    }
    public class Schema : NamedElement, OwlGeneration
    {
        public static string[] primitiveTypes = { "NUMBER", "STRING" };

        private string schemaBase;
        private List<string> imports = new List<string>();
        private Dictionary<string, string> defTypes = new Dictionary<string, string>();
        private List<SelectType> selectTypes = new List<SelectType>();
        private List<Entity> entities = new List<Entity>();
        public Schema(string schemaBase): base(null) 
        {
            this.schemaBase = schemaBase;
        }
        public Schema(string name, string schemaBase): base(name) 
        {
            this.schemaBase = schemaBase;
        }
        public static string GetPrimitiveType(string key)
        {
            switch (key)
            {
                case "STRING":
                    return "xsd:string";
                case "NUMBER":
                    return "xsd:int";
                default:
                    return $":{key}";
            }
        }
        public void AddImport(string import)
        {
            this.imports.Add(import);
        }
        public void AddDefType(string def, string type)
        {
            this.defTypes.Add(def, type);
        }
        public void AddSelectType(SelectType type)
        {
            this.selectTypes.Add(type);
        }
        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }
        public void GenerateOWl(TextWriter writer)
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
            foreach(SelectType type in this.selectTypes)
            {
                type.GenerateOWl(writer);
            }
            List<string> defList = new List<string>(this.defTypes.Keys);
            foreach(Entity entity in this.entities)
            {
                entity.TypeDef = defList;
                entity.GenerateOWl(writer);
            }
            writer.WriteLine(")");
        }
    }
    public class SelectType : NamedElement, OwlGeneration
    {
        private List<string> types = new List<string>();
        public SelectType(): base(null) { }
        public SelectType(string name): base(name) { }
        public void AddType(string type)
        {
            this.types.Add(type);
        }
        public void GenerateOWl(TextWriter writer)
        {
            writer.WriteLine($"EquivalentClasses(:{this.Name} ObjectUnionOf(");
            foreach(string type in this.types)
            {
                writer.WriteLine($":{type}");
            }
            writer.WriteLine("))");
        }
    }
    public class Entity : NamedElement, OwlGeneration
    {
        private List<Attribute> attributes = new List<Attribute>();
        public Entity() : base(null) { }
        public Entity(string name): base(name) { }
        public List<string> TypeDef
        {
            get; set;
        }
        public void AddAttribute(Attribute attribute)
        {
            this.attributes.Add(attribute);
        }
        public void GenerateOWl(TextWriter writer)
        {
            writer.WriteLine($"Declaration(Class(:{this.Name}))");
            //Attributes
            foreach(Attribute attribute in this.attributes)
            {
                attribute.TypeDef = this.TypeDef;
                attribute.GenerateOWl(writer);
            }
        }
    }
    public class Attribute : NamedElement, OwlGeneration
    {
        public Attribute() : base(null) { }
        public Attribute(string name, Entity owner): base(name) 
        {
            this.Owner = owner;
        }
        public Entity Owner
        {
            get; set;
        }
        public List<string> TypeDef
        {
            get; set;
        }
        public bool Optional
        {
            get; set;
        }
        public string AType
        {
            get; set;
        }
        public void GenerateOWl(TextWriter writer)
        {
            if (Schema.primitiveTypes.Contains(this.AType) || this.TypeDef.Contains(this.AType))
            {
                this.ProcessPrimitiveAttribute(writer, Owner.Name, this.Name, this.Optional, this.AType);
                return;
            }
            this.ProcessReferenceAttribute(writer, Owner.Name, this.Name, this.Optional, this.AType);
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
