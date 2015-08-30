using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Has Many Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HasManyAttribute : RelationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the map.
        /// </summary>
        /// <value>
        /// The type of the map.
        /// </value>
        public Type MapType { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HasManyAttribute"/> class.
        /// </summary>
        public HasManyAttribute()
            : base()
        {
            ColumnDbType = System.Data.DbType.Int32;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasManyAttribute"/> class.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        public HasManyAttribute(Type mapType)
            : this()
        {
            MapType = mapType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasManyAttribute"/> class.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="column">The column.</param>
        /// <param name="table">The table.</param>
        public HasManyAttribute(Type mapType, string column, string table)
            : this(mapType)
        {
            Column = column;
            Table = table;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Valueses the specified holder.
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <returns></returns>
        public IEnumerable<ActiveRecordBase> Values(object holder)
        {
            if (CurrentPropertyInfo != null)
                foreach (object item in (System.Collections.IEnumerable)CurrentPropertyInfo.GetValue(holder, null))
                    yield return (ActiveRecordBase)item;
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
            return String.Format("{0}-{1}#{2}", base.ToString(), this.MapType, this.Column);
        }

        #endregion
    }
}