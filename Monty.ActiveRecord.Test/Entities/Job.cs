using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord.Test.Entities
{
    /// <summary>
    /// Job
    /// </summary>
    [ActiveRecord("montyTJob")]
    public class Job : Base.BaseTest<Job>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [PrimaryKey("JobId", Generator = PrimaryKeyType.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Property("Name", ColumnType = "varchar(200)", NotNull = true, Length = 200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Property("Description", ColumnType = "varchar(MAX)", NotNull = false)]
        public string Description { get; set; }

        #endregion
    }
}
