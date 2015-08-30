using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Monty.ActiveRecord.Test.Entities
{
    /// <summary>
    /// Person
    /// </summary>
    [ActiveRecord("montyTPerson")]
    public class Person : Base.BaseTest<Person>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [PrimaryKey("PersonId", Generator = PrimaryKeyType.Identity)]
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
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>
        /// The birthday.
        /// </value>
        [Property("Birthday", NotNull = false)]
        public DateTime? Birthday { get; set; }

        private Job _CurrentJob;
        /// <summary>
        /// Gets or sets the current job.
        /// </summary>
        /// <value>
        /// The current job.
        /// </value>
        [BelongsTo("CurrentJob", NotNull = false)]
        public Job CurrentJob
        {
            get
            {
                if (_CurrentJob == null)
                    _CurrentJob = LazyLoad<Job>("CurrentJob");

                return _CurrentJob;
            }
            set { _CurrentJob = value; }
        }

        private List<Document> _Documents;
        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        [HasMany(typeof(Document), "Owner", "montyTDocument", Lazy = true, CascadeSave = false, CascadeDelete = true, OrderBy = "Name ASC")]
        public List<Document> Documents
        {
            get
            {
                if (_Documents == null)
                    _Documents = LazyLoadMany<Document>("Documents");

                return _Documents;
            }
            set { _Documents = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
            Documents = new List<Document>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Counts all with letter.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static int CountAllWithLetter(string letter, MatchMode mode)
        {
            try
            {
                Criteria criteria = Criteria.For<Person>();
                criteria
                    .Add(criteria.Like("Name", letter, mode));

                return Person.Count(criteria);
            }
            catch (Exception ex)
            {
                //Do something
                throw ex;
            }
        }

        /// <summary>
        /// Counts the peoploe starting with a or b.
        /// </summary>
        /// <returns></returns>
        public static int CountStartingWithAOrB()
        {
            try
            {
                Criteria criteria = Criteria.For<Person>();

                criteria
                    .Add(
                        criteria.Or()
                            .AddSubFilter(criteria.Like("Name", "a", MatchMode.Start))
                            .AddSubFilter(criteria.Like("Name", "b", MatchMode.Start))
                    )
                    .Add(criteria.Asc("Name"));

                return Person.Count(criteria);
            }
            catch (Exception ex)
            {
                //Do something
                throw ex;
            }
        }

        /// <summary>
        /// Finds all with letter.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static List<Person> FindAllWithLetter(string letter, MatchMode mode)
        {
            try
            {
                Criteria criteria = Criteria.For<Person>();

                criteria
                    .Add(criteria.Like("Name", letter, mode))
                    .Add(criteria.Asc("Name"));
                
                return Person.FindAll(criteria);
            }
            catch (Exception ex)
            {
                //Do something
                throw ex;
            }
        }

        /// <summary>
        /// Finds the random people.
        /// </summary>
        /// <param name="numberOfPeople">The number of people.</param>
        /// <returns></returns>
        public static List<Person> FindRandomPeople(int numberOfPeople)
        {
            try
            {
                Criteria criteria = Criteria.For<Person>();

                criteria
                    .Add(Criteria.Order.Random());

                return Person.SlicedFindAll(criteria, 0, numberOfPeople);
            }
            catch (Exception ex)
            {
                //Do something
                throw ex;
            }
        }

        #endregion
    }
}
