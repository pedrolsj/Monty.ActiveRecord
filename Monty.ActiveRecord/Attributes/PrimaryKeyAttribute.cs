using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Primary Key Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : FieldAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the custom generator.
        /// </summary>
        /// <value>
        /// The custom generator.
        /// </value>
        public Type CustomGenerator { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>
        /// The generator.
        /// </value>
        public PrimaryKeyType Generator { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is string type.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is string type; otherwise, <c>false</c>.
        /// </value>
        public bool IsStringType
        {
            get
            {
                if (CurrentPropertyInfo.PropertyType == typeof(double)
                    || CurrentPropertyInfo.PropertyType == typeof(float)
                    || CurrentPropertyInfo.PropertyType == typeof(decimal)
                    || CurrentPropertyInfo.PropertyType == typeof(int)
                    || CurrentPropertyInfo.PropertyType == typeof(short))
                    return false;
                else
                    return true;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        public PrimaryKeyAttribute()
        {
            ColumnType = "INT";
            Generator = PrimaryKeyType.Identity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public PrimaryKeyAttribute(PrimaryKeyType generator)
        {
            Generator = generator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public PrimaryKeyAttribute(string column)
        {
            Column = column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="column">The column.</param>
        public PrimaryKeyAttribute(PrimaryKeyType generator, string column)
        {
            Generator = generator;
            Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Includes the in insert.
        /// </summary>
        /// <returns></returns>
        public override bool IncludeInInsert()
        {
            return Generator == PrimaryKeyType.Assigned;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override object GetValue(ActiveRecordBase item)
        {
            if (CurrentPropertyInfo == null)
                return null;

            try
            {
                return CurrentPropertyInfo.GetValue(item, null);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(ActiveRecordBase item, object value)
        {
            if (CurrentPropertyInfo == null)
                return;

            try
            {
                CurrentPropertyInfo.SetValue(item, Convert.ChangeType(value.ToString(), CurrentPropertyInfo.PropertyType), null);
            }
            catch
            {
            }
        }

        #endregion
    }
}
