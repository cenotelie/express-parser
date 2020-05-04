﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Express_Model
{
    interface IOwlGeneration
    {
        void GenerateOWL(TextWriter writer);
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
    public class Schema : NamedElement, IOwlGeneration
    {
        public static string[] primitiveTypes = { "NUMBER", "STRING", "BINARY", "LOGICAL", "BOOLEAN", "INTEGER", "REAL" };

        private string schemaBase;
        private List<string> imports = new List<string>();
        private Dictionary<string, string> defTypes = new Dictionary<string, string>();
        private List<SelectType> selectTypes = new List<SelectType>();
        private List<Entity> entities = new List<Entity>();
        private Dictionary<string, string> equivalentClasses = new Dictionary<string, string>();
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
        public void AddEquivalentClasses(string cl1, string cl2)
        {
            this.equivalentClasses.Add(cl1, cl2);
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
            foreach(Entity entity in this.entities)
            {
                entity.TypeDef = defList;
                entity.GenerateOWL(writer);
            }
            writer.WriteLine(")");
        }
    }
    public class SelectType : NamedElement, IOwlGeneration
    {
        private List<string> types = new List<string>();
        public SelectType(string name): base(name) { }
        public SelectType() : this(null) { }
        public void AddType(string type)
        {
            this.types.Add(type);
        }
        public void GenerateOWL(TextWriter writer)
        {
            writer.WriteLine($"EquivalentClasses(:{this.Name} ObjectUnionOf(");
            foreach(string type in this.types)
            {
                writer.WriteLine($":{type}");
            }
            writer.WriteLine("))");
        }
    }
    public class Entity : NamedElement, IOwlGeneration
    {
        private List<string> disjointUnion = new List<string>();
        private List<string> superTypes = new List<string>();
        private List<Property> properties = new List<Property>();
        
        public Entity(string name): base(name) 
        {
            this.Abstract = false;
        }
        public Entity() : this(null) { }
        public bool Abstract
        {
            get; set;
        }
        public List<string> TypeDef
        {
            get; set;
        }
        public void AddDisjointUnion(string type)
        {
            this.disjointUnion.Add(type);
        }
        public void AddSuperType(string type)
        {
            this.superTypes.Add(type);
        }
        public void AddProperty(Property property)
        {
            this.properties.Add(property);
        }
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
            foreach(string super in this.superTypes)
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
    public class Property : NamedElement, IOwlGeneration
    {
        public Property(string name, Entity owner): base(name) 
        {
            this.Owner = owner;
        }
        public Property() : base(null) { }
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
        public string PType
        {
            get; set;
        }
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
