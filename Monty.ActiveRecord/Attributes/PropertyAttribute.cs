using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Property Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyAttribute : FieldAttribute
    {
        #region Properties

        /// <summary>
        /// Default
        /// </summary>
        public string Default { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Property Attribute
        /// </summary>
        public PropertyAttribute()
            : base()
        {
        }

        /// <summary>
        /// Property Attribute
        /// </summary>
        /// <param name="column"></param>
        public PropertyAttribute(string column)
            : base(column)
        {
        }

        /// <summary>
        /// Property Attribute
        /// </summary>
        /// <param name="column"></param>
        /// <param name="type"></param>
        public PropertyAttribute(string column, string type)
            : base(column, type)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Includes the in insert.
        /// </summary>
        /// <returns></returns>
        public override bool IncludeInInsert()
        {
            return true;
        }

        #endregion
    }
}