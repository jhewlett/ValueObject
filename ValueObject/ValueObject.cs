using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ValueObject
{
    //public class IgnoreAttribute : Attribute
    //{
    //}

    public class ValueObject<T> : IEquatable<T> where T : class
    {
        public static bool operator ==(ValueObject<T> obj1, ValueObject<T> obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject<T> obj1, ValueObject<T> obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(T obj)
        {
            return Equals(obj as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !typeof(ValueObject<T>).IsAssignableFrom(obj.GetType())) return false;
            
            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {       
            return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return object.Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                //.Where(p => p.GetCustomAttribute(typeof(IgnoreAttribute)) != null);
        }

        private FieldInfo[] GetFields()
        {
            return GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        public override int GetHashCode()
        {
            unchecked   //allow overflow
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {   
                    var value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach(var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        private int HashValue(int seed, object value)
        {
            var currentHash = value != null
                ? value.GetHashCode()
                : 0;

            return seed * 23 + currentHash;
        }
    }
}
