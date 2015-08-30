using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Relation Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RelationAttribute : FieldAttribute
    { 
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RelationAttribute"/> is lazy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if lazy; otherwise, <c>false</c>.
        /// </value>
        public bool Lazy { get; set; }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the where.
        /// </summary>
        /// <value>
        /// The where.
        /// </value>
        public string Where { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cascade delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cascade delete]; otherwise, <c>false</c>.
        /// </value>
        public bool CascadeDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cascade save].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cascade save]; otherwise, <c>false</c>.
        /// </value>
        public bool CascadeSave { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationAttribute"/> class.
        /// </summary>
        public RelationAttribute()
        {
            CascadeDelete = false;
            CascadeSave = false;
        }

        #endregion
    }
}
