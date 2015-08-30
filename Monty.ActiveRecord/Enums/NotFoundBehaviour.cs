using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Not Found Behaviour
    /// </summary>
    [Serializable]
    public enum NotFoundBehaviour
    {
        /// <summary>
        /// Default
        /// </summary>
        Default,
        /// <summary>
        /// Exception
        /// </summary>
        Exception,
        /// <summary>
        /// Ignore
        /// </summary>
        Ignore
    }
}
