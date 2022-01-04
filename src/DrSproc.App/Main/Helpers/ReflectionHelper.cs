using DrSproc.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DrSproc.Main.Helpers
{
    internal static class ReflectionHelper
    {
        public static T CreateWithReflection<T>(this IDataReader reader, string storedProcName)
        {
            var returnType = typeof(T);

            var propAndValues = returnType.GetPropertiesWithMatchingValues(reader, storedProcName);

            T instance = Activator.CreateInstance<T>();

            instance.SetInstancePropertyValues(propAndValues, storedProcName);

            return instance;
        }

        private static IEnumerable<TypeProperty> GetPropertiesWithMatchingValues(this Type type, IDataReader reader, string storedProcName, string parentProperty = null)
        {
            return type.GetProperties()
                       .Where(x => x.CanWrite)
                       .Select(x => {

                           var nullable = x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                           nullable = !x.PropertyType.IsValueType || nullable;

                           var underlyingType = Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType;

                           var value = reader[x.Name];

                           var fullNameWithParent = parentProperty == null ? x.Name : parentProperty + x.Name;

                           return new TypeProperty
                           {
                               Name = x.Name,
                               Type = underlyingType,
                               IsNullable = nullable,
                               SubProperties = underlyingType.GetPropertiesWithMatchingValues(reader, storedProcName, parentProperty: fullNameWithParent),
                               ValueFound = reader.TryGetValue(fullNameWithParent, underlyingType, nullable, storedProcName)
                           };
                        });
        }

        private static void SetInstancePropertyValues(this object instance, IEnumerable<TypeProperty> props, string storedProcName)
        {
            var type = instance.GetType();

            foreach(var prop in props)
            {
                if (prop.ValueFound != null)
                {
                    type.GetProperty(prop.Name)
                        .SetValue(instance, prop.ValueFound);

                    continue;
                }
                
                if (prop.SubProperties.Any())
                {
                    var subInstance = Activator.CreateInstance(prop.Type);

                    subInstance.SetInstancePropertyValues(prop.SubProperties, storedProcName);

                    type.GetProperty(prop.Name)
                        .SetValue(instance, subInstance);

                    continue;
                }
                
                if (prop.IsNullable)
                {
                    type.GetProperty(prop.Name)
                        .SetValue(instance, null);
                }
            }
        }

        private static object TryGetValue(this IDataReader reader, string fieldName, Type typeRequired, bool nullable, string storedProcName)
        {
            var fieldValue = reader[fieldName];

            if (fieldValue == null)
            {
                if (nullable)
                    return null;

                throw DrSprocEntityMappingException.RequiredFieldIsNull(storedProcName, fieldName);
            }

            if (typeRequired == fieldValue.GetType())
                return fieldValue;

            try
            {
                return Convert.ChangeType(fieldValue, typeRequired);
            }
            catch
            {
                throw DrSprocEntityMappingException.FieldOfWrongDataType(storedProcName, fieldName, typeRequired, fieldValue.GetType(), fieldValue);
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
