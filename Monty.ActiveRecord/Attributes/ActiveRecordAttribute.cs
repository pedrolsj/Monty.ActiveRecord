using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Active Record Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ActiveRecordAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Is Mutable
        /// </summary>
        public bool Mutable { get; set; }

        /// <summary>
        /// Schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Table
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Where
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName
        {
            get
            {
                if (!String.IsNullOrEmpty(Schema))
                    return String.Format("{0}.{1}", this.Schema, this.Table);
                else
                    return this.Table;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Active Record Attribute
        /// </summary>
        public ActiveRecordAttribute()
        {
            Mutable = true;
        }

        /// <summary>
        /// Active Record Attribute
        /// </summary>
        /// <param name="table"></param>
        public ActiveRecordAttribute(string table)
            :this()
        {
            this.Table = table;
        }

        /// <summary>
        /// Active Record Attribute
        /// </summary>
        /// <param name="table"></param>
        /// <param name="schema"></param>
        public ActiveRecordAttribute(string table, string schema)
            :this()
        {
            this.Schema = schema;
            this.Table = table;
        }

        #endregion
    }
}