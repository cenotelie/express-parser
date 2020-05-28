using System.Collections.Generic;

namespace Express_Model
{
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
    public partial class Schema : NamedElement
    {
        public static string[] primitiveTypes = { "NUMBER", "STRING", "BINARY", "LOGICAL", "BOOLEAN", "INTEGER", "REAL" };

        private string schemaBase;
        private List<string> imports = new List<string>();
        private Dictionary<string, string> defTypes = new Dictionary<string, string>();
        private List<Enumeration> enumerations = new List<Enumeration>();
        private List<SelectType> selectTypes = new List<SelectType>();
        private List<Entity> entities = new List<Entity>();
        private List<Property> properties = new List<Property>();
        private Dictionary<string, string> equivalentClasses = new Dictionary<string, string>();
        public Schema(string schemaBase): base(null) 
        {
            this.schemaBase = schemaBase;
        }
        public Schema(string name, string schemaBase): base(name) 
        {
            this.schemaBase = schemaBase;
        }
        public void AddImport(string import)
        {
            this.imports.Add(import);
        }
        public void AddDefType(string def, string type)
        {
            this.defTypes.Add(def, type);
        }
        public void AddEnumeration(Enumeration enumeration)
        {
            this.enumerations.Add(enumeration);
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
            this.equivalentClasses.Add(cl1, Schema.GetOwlPrimitiveType(cl2));
        }
    }
    public partial class Enumeration : NamedElement
    {
        private List<string> literals = new List<string>();
        public Enumeration(string name): base(name) { }
        public void AddLiteral(string literal)
        {
            this.literals.Add(literal);
        }
    }
    public partial class SelectType : NamedElement
    {
        private List<string> types = new List<string>();
        public SelectType(string name): base(name) { }
        public SelectType() : this(null) { }
        public void AddType(string type)
        {
            this.types.Add(type);
        }
    }
    public partial class Entity : NamedElement
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
    }
    public partial class Property : NamedElement
    {
        public Property(string name, Entity subject): base(name) 
        {
            this.Subject = subject;
        }
        public Property() : base(null) { }
        public Entity Subject
        {
            get; set;
        }
        public List<string> TypeDef
        {
            get; set;
        }
        public bool IsFunctional
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
    }

    public partial class ObjectProperty : Property
    {
        public ObjectProperty(string name, Entity subject, Entity _object): base(name, subject)
        {
            this.Object = _object;
        }
        public ObjectProperty() : base() { }
        public Entity Object
        {
            get; set;
        }

    }

    public partial class DataProperty : Property
    {
        public DataProperty(string name, Entity subject, string _object) : base(name, subject)
        {
            this.Object = _object;
        }
        public DataProperty() : base() { }
        public string Object
        {
            get; set;
        }

    }
}
