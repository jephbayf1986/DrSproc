using DrSproc.Exceptions;
using DrSproc.Main.EntityMapping;
using System;
using System.Data;

namespace DrSproc.EntityMapping
{
    public abstract class EntityMapper<T>
    {
        private IDataReader _reader;

        public abstract T Map();

        internal void SetReader(IDataReader reader)
        {
            _reader = reader;
        }

        protected string GetString(string fieldName, bool allowNull = true, string defaultIfNull = null)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

            return value?.ToString() ?? defaultIfNull;
        }

        protected int GetInt(string fieldName, bool allowNull = false, int defaultIfNull = default)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = int.TryParse(value.ToString(), out int result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(int), value.GetType(), value);

            return result;
        }

        protected int? GetNullableInt(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetInt(fieldName);
        }

        protected double GetDouble(string fieldName, bool allowNull = false, double defaultIfNull = default)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = double.TryParse(value.ToString(), out double result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(double), value.GetType(), value);

            return result;
        }

        protected double? GetNullableDouble(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetDouble(fieldName);
        }

        protected decimal GetDecimal(string fieldName, bool allowNull = false, decimal defaultIfNull = default)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = decimal.TryParse(value.ToString(), out decimal result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(decimal), value.GetType(), value);

            return result;
        }

        protected decimal? GetNullableDecimal(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetDecimal(fieldName);
        }

        protected bool GetBoolean(string fieldName, bool allowNull = false, bool defaultIfNull = false)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

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
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(bool), value.GetType(), value);

            return result;
        }

        protected bool? GetNullableBoolean(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetBoolean(fieldName);
        }

        protected DateTime GetDateTime(string fieldName, bool allowNull = false, DateTime defaultIfNull = default)
        {
            var value = _reader.GetField(fieldName);

            if (!allowNull) value.CheckNotNull(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = DateTime.TryParse(value.ToString(), out DateTime result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(DateTime), value.GetType(), value);

            return result;
        }

        protected DateTime? GetNullableDateTime(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetDateTime(fieldName);
        }

        protected Guid GetGuid(string fieldName, bool generateNewIfNull = false)
        {
            var value = _reader.GetField(fieldName);

            if (!generateNewIfNull) value.CheckNotNull(fieldName);

            if (value.IsNull())
                return new Guid();

            var isValidType = Guid.TryParse(value.ToString(), out Guid result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        protected Guid? GetNullableGuid(string fieldName)
        {
            var value = _reader.GetField(fieldName);

            if (value.IsNull())
                return null;

            return GetGuid(fieldName);
        }
    }
}