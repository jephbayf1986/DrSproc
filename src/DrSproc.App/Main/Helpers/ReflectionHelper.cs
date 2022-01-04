using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DrSproc.Main.Helpers
{
    internal static class ReflectionHelper
    {
        public static T CreateWithReflection<T>(this IDataReader reader)
        {
            var returnType = typeof(T);

            var propAndValues = returnType.GetPropertiesWithMatchingValues(reader);

            T instance = Activator.CreateInstance<T>();

            instance.SetInstancePropertyValues(propAndValues);

            return instance;
        }

        private static IEnumerable<TypeProperty> GetPropertiesWithMatchingValues(this Type type, IDataReader reader, string parentProperty = null)
        {
            return type.GetProperties()
                       .Where(x => x.CanWrite)
                       .Select(x => {

                           var nullable = x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                           var xType = Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType;

                           var value = reader[x.Name];

                           var fullNameWithParent = parentProperty == null ? x.Name : parentProperty + x.Name;

                           return new TypeProperty
                           {
                               Name = x.Name,
                               Type = xType,
                               IsNullable = nullable,
                               SubProperties = xType.GetPropertiesWithMatchingValues(reader, parentProperty: fullNameWithParent),
                               ValueFound = reader[fullNameWithParent]
                           };
                        });
        }

        private static void SetInstancePropertyValues(this object instance, IEnumerable<TypeProperty> props)
        {
            var type = instance.GetType();

            foreach(var prop in props)
            {
                if (prop.ValueFound != null)
                {
                    type.GetProperty(prop.Name)
                        .SetValue(instance, prop.ValueFound);
                }
                else if (prop.SubProperties.Any())
                {
                    var subInstance = Activator.CreateInstance(prop.Type);

                    subInstance.SetInstancePropertyValues(prop.SubProperties);

                    type.GetProperty(prop.Name)
                        .SetValue(instance, subInstance);
                }
            }
        }

        private class TypeProperty
        {
            public string Name { get; set; }

            public Type Type { get; set; } 

            public bool IsNullable { get; set; }

            public IEnumerable<TypeProperty> SubProperties { get; set; }

            public object ValueFound { get; set; }
        }
    }
}
