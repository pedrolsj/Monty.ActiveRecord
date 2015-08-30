using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Field Attribute
    /// </summary>
    public class FieldAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// SQL Column Name
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Not Null
        /// </summary>
        public bool NotNull { get; set; }

        /// <summary>
        /// Unique
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Column Type
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// Column Db Type
        /// </summary>
        public DbType ColumnDbType { get; set; }

        /// <summary>
        /// Property Info
        /// </summary>
        public PropertyInfo CurrentPropertyInfo { get; private set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Field Attribute
        /// </summary>
        public FieldAttribute()
        {
            ColumnDbType = DbType.Object;
        }

        /// <summary>
        /// Field Attribute
        /// </summary>
        /// <param name="column"></param>
        public FieldAttribute(string column)
            : this()
        {
            Column = column;
        }

        /// <summary>
        /// Field Attribute
        /// </summary>
        /// <param name="column"></param>
        /// <param name="type"></param>
        public FieldAttribute(string column, string type)
            : this(column)
        {
            ColumnType = type;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static FieldAttribute GetAttribute(Type type, string property)
        {
            foreach (var item in type.GetProperties())
            {
                if (item.Name == property)
                {
                    object[] attr = item.GetCustomAttributes(typeof(FieldAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        return ((FieldAttribute)attr.First()).AttributeWithProperty<FieldAttribute>(item);
                }
            }

            return null;
        }

        /// <summary>
        /// Includes the in insert.
        /// </summary>
        /// <returns></returns>
        public virtual bool IncludeInInsert()
        {
            return true;
        }

        /// <summary>
        /// Attributes the with property.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        public TAttributeType AttributeWithProperty<TAttributeType>(PropertyInfo info)
            where TAttributeType : FieldAttribute
        {
            CurrentPropertyInfo = info;

            return (TAttributeType)this;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public virtual object GetValue(ActiveRecordBase item)
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
        public virtual void SetValue(ActiveRecordBase item, object value)
        {
            if (CurrentPropertyInfo == null)
                return;

            try
            {
                CurrentPropertyInfo.SetValue(item, value, null);
            }
            catch
            {
            }
        }

        #endregion

        #region To String

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            return String.Format("{0}#{1}", base.ToString(), Column);
        }

        #endregion
    }
}