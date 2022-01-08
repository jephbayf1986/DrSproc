using DrSproc.Exceptions;
using DrSproc.Main.EntityMapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace DrSproc.EntityMapping
{
    /// <summary>
    /// <b>Custom Mapper</b> <br />
    /// Inherit from this class to create a custom mapping from a stored procedure query result to an entity model
    /// <para>
    /// Declare the return type in the map using the in-built Read**** methods.<br/><br/>
    /// <i>Example:</i> <br />
    /// <code xml:space="preserve">
    /// public override MyClass Map()
    /// {
    ///     return new MyClass()
    ///     {
    ///         Id = ReadInt(Id_Lookup, allowNull: false),
    ///         FirstName = ReadString(FirstName_Loookup),
    ///         LastName = ReadString(LastName_Loookup),
    ///         Description = ReadString(Description_Lookup),
    ///         Height = ReadDecimal(Height_Lookup, allowNull: true, defaultIfNull: Height_DefaultIfNull),
    ///         Width = ReadNullableDecimal(Width_Lookup),
    ///         DateOfBirth = ReadNullableDateTime(Dob_Lookup),
    ///     };
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <typeparam name="T">Entity Model to map to</typeparam>
    public abstract class CustomMapper<T> : IDisposable
    {
        private IDataReader _dataReader;
        private string _storedProcName;
        
        internal void SetReader(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        internal void SetStoredProcName(string storedProcName)
        {
            _storedProcName = storedProcName;
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>The Entity Desired from Mapping</returns>
        public abstract T Map();

        internal IEnumerable<T> MapMulti()
        {
            var mappedItems = new List<T>();

            while (_dataReader.Read())
            {
                mappedItems.Add(Map());
            }

            return mappedItems;
        }

        /// <summary>
        /// Read a String from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be used if found</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>String Generated from Read</returns>
        protected string ReadString(string fieldName, bool allowNull = true, string defaultIfNull = null)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            return value?.ToString() ?? defaultIfNull;
        }

        /// <summary>
        /// Read an Long from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Long Generated from Read</returns>
        protected long ReadLong(string fieldName, bool allowNull = false, long defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = long.TryParse(value.ToString(), out long result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Long from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Long Generated from Read</returns>
        protected long? ReadNullableLong(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadLong(fieldName);
        }

        /// <summary>
        /// Read an Int from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Int Generated from Read</returns>
        protected int ReadInt(string fieldName, bool allowNull = false, int defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = int.TryParse(value.ToString(), out int result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Int from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Int Generated from Read</returns>
        protected int? ReadNullableInt(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadInt(fieldName);
        }

        /// <summary>
        /// Read an Short from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Short Generated from Read</returns>
        protected short ReadShort(string fieldName, bool allowNull = false, short defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = short.TryParse(value.ToString(), out short result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Short from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Short Generated from Read</returns>
        protected short? ReadNullableShort(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadShort(fieldName);
        }

        /// <summary>
        /// Read an Byte from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Byte Generated from Read</returns>
        protected byte ReadByte(string fieldName, bool allowNull = false, byte defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = byte.TryParse(value.ToString(), out byte result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Byte from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Byte Generated from Read</returns>
        protected byte? ReadNullableByte(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadByte(fieldName);
        }

        /// <summary>
        /// Read an Double from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Double Generated from Read</returns>
        protected double ReadDouble(string fieldName, bool allowNull = false, double defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = double.TryParse(value.ToString(), out double result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Double from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Double Generated from Read</returns>
        protected double? ReadNullableDouble(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDouble(fieldName);
        }

        /// <summary>
        /// Read an Decimal from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Decimal Generated from Read</returns>
        protected decimal ReadDecimal(string fieldName, bool allowNull = false, decimal defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = decimal.TryParse(value.ToString(), out decimal result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Decimal from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Decimal Generated from Read</returns>
        protected decimal? ReadNullableDecimal(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDecimal(fieldName);
        }

        /// <summary>
        /// Read an Boolean from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>Boolean Generated from Read</returns>
        protected bool ReadBoolean(string fieldName, bool allowNull = false, bool defaultIfNull = false)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

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
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Boolean from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Boolean Generated from Read</returns>
        protected bool? ReadNullableBoolean(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadBoolean(fieldName);
        }

        /// <summary>
        /// Read an DateTime from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="allowNull">Allow a null value to be read if found; value from defaultIfNull is used</param>
        /// <param name="defaultIfNull">If nulls found, value to use instead</param>
        /// <returns>DateTime Generated from Read</returns>
        protected DateTime ReadDateTime(string fieldName, bool allowNull = false, DateTime defaultIfNull = default)
        {
            var value = GetField(fieldName);

            if (!allowNull && value.IsNull())
                ThrowNullError(fieldName);

            if (value.IsNull())
                return defaultIfNull;

            var isValidType = DateTime.TryParse(value.ToString(), out DateTime result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable DateTime from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable DateTime Generated from Read</returns>
        protected DateTime? ReadNullableDateTime(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadDateTime(fieldName);
        }

        /// <summary>
        /// Read an Guid from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <param name="generateNewIfNull">Allow Null and generate a new Guid if Null is read</param>
        /// <returns>Guid Generated from Read</returns>
        protected Guid ReadGuid(string fieldName, bool generateNewIfNull = false)
        {
            var value = GetField(fieldName);

            if (!generateNewIfNull && value.IsNull())
                    ThrowNullError(fieldName);

            if (value.IsNull())
                return new Guid();

            var isValidType = Guid.TryParse(value.ToString(), out Guid result);

            if (!isValidType)
                throw DrSprocEntityMappingException.FieldOfWrongDataType(_storedProcName, fieldName, typeof(Guid), value.GetType(), value);

            return result;
        }

        /// <summary>
        /// Read a Nullable Guid from the Query Result
        /// </summary>
        /// <param name="fieldName">The name of the column received</param>
        /// <returns>Nullable Guid Generated from Read</returns>
        protected Guid? ReadNullableGuid(string fieldName)
        {
            var value = GetField(fieldName);

            if (value.IsNull())
                return null;

            return ReadGuid(fieldName);
        }

        private object GetField(string fieldName)
        {
            var fieldFound = _dataReader.TryGetField(fieldName, out object value);

            if (!fieldFound)
                throw DrSprocEntityMappingException.FieldDoesntExist(_storedProcName, fieldName);

            return value;
        }

        private void ThrowNullError(string fieldName)
        {
            throw DrSprocEntityMappingException.RequiredFieldIsNull(_storedProcName, fieldName);
        }

        /// <summary>
        /// Dispose the Mapper
        /// </summary>
        public void Dispose()
        {
            if (_dataReader != null)
                _dataReader.Dispose();
        }
    }
}