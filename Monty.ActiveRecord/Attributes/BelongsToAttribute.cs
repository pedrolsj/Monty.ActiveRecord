using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Belongs To Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BelongsToAttribute : RelationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the not found behaviour.
        /// </summary>
        /// <value>
        /// The not found behaviour.
        /// </value>
        public NotFoundBehaviour NotFoundBehaviour { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BelongsToAttribute"/> class.
        /// </summary>
        public BelongsToAttribute()
            : base()
        {
            ColumnDbType = System.Data.DbType.Int32;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BelongsToAttribute"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public BelongsToAttribute(string column)
            : this()
        {
            Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override object GetValue(ActiveRecordBase item)
        {
            try
            {
                return (this.CurrentPropertyInfo.GetValue(item, null) as ActiveRecordBase).GetId();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Values the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public ActiveRecordBase Value(ActiveRecordBase item)
        {
            return (this.CurrentPropertyInfo.GetValue(item, null) as ActiveRecordBase);
        }

        #endregion
    }
}