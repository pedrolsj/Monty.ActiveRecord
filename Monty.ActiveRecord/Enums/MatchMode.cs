using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Monty.ActiveRecord
{
    /// <summary>
    /// Match Mode
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// The exact
        /// </summary>
        Exact,
        /// <summary>
        /// The anywhere
        /// </summary>
        Anywhere,
        /// <summary>
        /// The end
        /// </summary>
        End,
        /// <summary>
        /// The start
        /// </summary>
        Start
    }
}
