using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Monty.Log
{
    /// <summary>
    /// Log Category
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// for historical log
        /// </summary>
        Info,
        /// <summary>
        /// for expected exceptions
        /// </summary>
        Warn,
        /// <summary>
        /// for debug only
        /// </summary>
        Debug,
        /// <summary>
        /// for default system operation exceptions
        /// </summary>
        Error,
        /// <summary>
        /// for default system execution exceptions
        /// </summary>
        Fatal
    }
}
