using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Monty.ActiveRecord.Test.Views
{
    /// <summary>
    /// People Names
    /// </summary>
    [ActiveRecord("montyVPeopleNames", Mutable = false)]
    public class PeopleNames : Base.BaseTest<PeopleNames>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [PrimaryKey("Name", Generator = PrimaryKeyType.Assigned)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the occurrences.
        /// </summary>
        /// <value>
        /// The occurrences.
        /// </value>
        [BelongsTo("Occurrences")]
        public int Occurrences { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the most common name.
        /// </summary>
        /// <returns></returns>
        public static string FindMostCommonName()
        {
            try
            {
                Criteria criteria = Criteria.For<PeopleNames>();

                criteria
                    .Add(criteria.Desc("Occurrences"))
                    .Add(criteria.Asc("Name"));

                List<String> names = PeopleNames.SlicedProjectionFindAll<String>(criteria, "Name", 0, 1);

                return names != null && names.Count == 1 ? names[0] : String.Empty;

                /*
                Could also be:

                PeopleNames.FindFirst(criteria).Name
                */
            }
            catch (Exception ex)
            {
                //Do something
                throw ex;
            }
        }

        /// <summary>
        /// Finds some random names.
        /// </summary>
        /// <param name="numberOfNames">The number of names.</param>
        /// <returns></returns>
        public static List<String> FindRandomNames(int numberOfNames)
        {
            try
            {
                Criteria criteria = Criteria.For<PeopleNames>();

                criteria
                    .Add(Criteria.Order.Random());

                return PeopleNames.SlicedProjectionFindAll<String>(criteria, "Name", 0, numberOfNames);
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
