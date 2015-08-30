using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Primary Key Type
    /// </summary>
    [Serializable]
    public enum PrimaryKeyType
    {
        /// <summary>
        /// Identity
        /// </summary>
        Identity,
        /// <summary>
        /// Guid
        /// </summary>
        Guid,
        /// <summary>
        /// Assigned
        /// </summary>
        Assigned
    }
}