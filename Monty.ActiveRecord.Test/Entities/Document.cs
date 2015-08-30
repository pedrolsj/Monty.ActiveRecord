using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord.Test.Entities
{
    /// <summary>
    /// Document
    /// </summary>
    [ActiveRecord("montyTDocument")]
    public class Document : Base.BaseTest<Document>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [PrimaryKey("DocumentId", Generator = PrimaryKeyType.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Property("Name", ColumnType = "varchar(200)", NotNull = true, Length = 200)]
        public string Name { get; set; }

        private Person _Owner;
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        [BelongsTo("Owner", NotNull = true)]
        public Person Owner
        {
            get
            {
                if (_Owner == null)
                    _Owner = LazyLoad<Person>("Owner");

                return _Owner;
            }
            set { _Owner = value; }
        }
        
        #endregion
    }
}
