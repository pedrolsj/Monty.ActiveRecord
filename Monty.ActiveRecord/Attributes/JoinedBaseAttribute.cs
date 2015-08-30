using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Joined Base Attribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JoinedBaseAttribute : Attribute
    {
        #region Contructors

        /// <summary>
        /// Active Record Attribute
        /// </summary>
        public JoinedBaseAttribute()
        {
        }

        #endregion
    }
}
