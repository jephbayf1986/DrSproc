using DrSproc.Exceptions;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System;

namespace DrSproc.EntityMapping
{
    public abstract class CustomMapper<T>
    {
        private InProcessStoredProc _conditions; 
        
        internal void SetInProcessConditions(InProcessStoredProc conditions)
        {
            _conditions = conditions;   
        }

        public abstract T Map();

        protected string ReadString(string fieldName, bool allowNull = true, string defaultIfNull = null)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            return value?.ToString() ?? defaultIfNull;
        }

        protected int ReadInt(string fieldName, bool allowNull = false, int defaultIfNull = default)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = int.TryParse(value.ToString(), out int result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(int), value.GetType(), value);

            return result;
        }

        protected int? ReadNullableInt(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadInt(fieldName);
        }

        protected double ReadDouble(string fieldName, bool allowNull = false, double defaultIfNull = default)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = double.TryParse(value.ToString(), out double result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(double), value.GetType(), value);

            return result;
        }

        protected double? ReadNullableDouble(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDouble(fieldName);
        }

        protected decimal ReadDecimal(string fieldName, bool allowNull = false, decimal defaultIfNull = default)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = decimal.TryParse(value.ToString(), out decimal result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(decimal), value.GetType(), value);

            return result;
        }

        protected decimal? ReadNullableDecimal(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDecimal(fieldName);
        }

        protected bool ReadBoolean(string fieldName, bool allowNull = false, bool defaultIfNull = false)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var stringVal = value.ToString()
                                 .ToLower()
                                 .Replace("yes", "true")
                                 .Replace("y", "true")
                                 .Replace("no", "false")
                                 .Replace("n", "false");

            var isValidType = bool.TryParse(stringVal, out bool result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(bool), value.GetType(), value);

            return result;
        }

        protected bool? ReadNullableBoolean(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadBoolean(fieldName);
        }

        protected DateTime ReadDateTime(string fieldName, bool allowNull = false, DateTime defaultIfNull = default)
        {
            var value = _conditions.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = DateTime.TryParse(value.ToString(), out DateTime result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(DateTime), value.GetType(), value);

            return result;
        }

        protected DateTime? ReadNullableDateTime(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDateTime(fieldName);
        }

        protected Guid ReadGuid(string fieldName, bool generateNewIfNull = false)
        {
            var value = _conditions.GetField(fieldName);

            if (!generateNewIfNull) value.CheckNotNull(_conditions, fieldName);

            if (value.IsNull())
                return new Guid();

            var isValidType = Guid.TryParse(value.ToString(), out Guid result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_conditions, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        protected Guid? ReadNullableGuid(string fieldName)
        {
            var value = _conditions.GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadGuid(fieldName);
        }
    }
}