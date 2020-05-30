using System.Collections.Generic;
using System.Linq;

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

        private readonly string schemaBase;
        private readonly List<string> imports = new List<string>();
        private readonly Dictionary<string, string> defTypes = new Dictionary<string, string>();
        private readonly List<Enumeration> enumerations = new List<Enumeration>();
        private readonly List<SelectType> selectTypes = new List<SelectType>();
        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<Property> properties = new List<Property>();
        private readonly Dictionary<string, string> equivalentClasses = new Dictionary<string, string>();
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
        public Property GetProperty(string name)
        {
            List<Property> props = this.properties.Where<Property>(p => p.Name == name).ToList();
            Property prop;
            if (props.Count == 0)
            {
                prop = new Property(name);
                this.properties.Add(prop);
            } else
            {
                prop = props.First();
            }
            return prop;
        }
        public void AddProperty(Property property)
        {
            this.properties.Add(property);
        }
    }
    public partial class Enumeration : NamedElement
    {
        private readonly List<string> literals = new List<string>();
        public Enumeration(string name): base(name) { }
        public void AddLiteral(string literal)
        {
            this.literals.Add(literal);
        }
    }
    public partial class SelectType : NamedElement
    {
        private readonly List<string> types = new List<string>();
        public SelectType(string name): base(name) { }
        public SelectType() : this(null) { }
        public void AddType(string type)
        {
            this.types.Add(type);
        }
    }
    public partial class Entity : NamedElement
    {
        private readonly List<string> disjointUnion = new List<string>();
        private readonly List<string> superTypes = new List<string>();
        
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
    }
    public partial class Property : NamedElement
    {
        private readonly List<Entity> subjects = new List<Entity>();
        private readonly List<string> objects = new List<string>();
        public Property(string name): base(name) { }
        public void AddSubject(Entity entity)
        {
            if (!this.subjects.Contains(entity))
            this.subjects.Add(entity);
        }
        public void AddObject(string name)
        {
            if (!this.objects.Contains(name))
            {
                this.objects.Add(name);
            }
        }
        public bool IsFunctional
        {
            get; set;
        }
        public bool Optional
        {
            get; set;
        }
    }
}
