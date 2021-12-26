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

            return returnType.CreateTypeFromValues<T>(propAndValues);
        }

        private static IEnumerable<TypeProperty> GetPropertiesWithMatchingValues(this Type type, IDataReader reader, string parentProperty = null)
        {
            return type.GetProperties()
                       .Select(x => {

                           var nullable = x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                           var xType = Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType;

                           var value = reader[x.Name];

                           return new TypeProperty
                           {
                               Name = x.Name,
                               Type = xType,
                               IsNullable = nullable,
                               SubProperties = xType.GetPropertiesWithMatchingValues(reader, parentProperty: x.Name),
                               ValueFound = reader[x.Name]
                           };
                        });
        }

        private static T CreateTypeFromValues<T>(this Type type, IEnumerable<TypeProperty> props)
        {
            T instance = Activator.CreateInstance<T>();

            foreach(var prop in props)
            {
                if (prop.ValueFound != null)
                {
                    type.GetProperty(prop.Name)
                        .SetValue(instance, prop.ValueFound);
                }
            }

            return instance;
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
