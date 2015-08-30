using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Monty.ActiveRecord
{
    /// <summary>
    /// More Than One Object Not Found Exception
    /// </summary>
    [Serializable]
    public class MoreThanOneObjectNotFoundException : System.Exception
    {
        #region Properties

        /// <summary>
        /// Gets or sets the number of objects found.
        /// </summary>
        /// <value>
        /// The number of objects found.
        /// </value>
        public int NumberOfObjectsFound { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        public MoreThanOneObjectNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MoreThanOneObjectNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MoreThanOneObjectNotFoundException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        /// <param name="numberOfObjectsFound">The number of objects found.</param>
        public MoreThanOneObjectNotFoundException(int numberOfObjectsFound) : base() { NumberOfObjectsFound = numberOfObjectsFound; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        /// <param name="numberOfObjectsFound">The number of objects found.</param>
        public MoreThanOneObjectNotFoundException(int numberOfObjectsFound, string message) : base(message) { NumberOfObjectsFound = numberOfObjectsFound; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreThanOneObjectNotFoundException"/> class.
        /// </summary>
        /// <param name="numberOfObjectsFound">The number of objects found.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MoreThanOneObjectNotFoundException(int numberOfObjectsFound, string message, System.Exception inner) : base(message, inner) { NumberOfObjectsFound = numberOfObjectsFound; }

        #endregion
    }
}
