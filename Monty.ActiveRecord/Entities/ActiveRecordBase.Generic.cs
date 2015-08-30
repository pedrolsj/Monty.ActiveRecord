using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Monty.Log;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Active Record Base
    /// </summary>
    /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
    public class ActiveRecordBase<TMappedClass> : ActiveRecordBase
        where TMappedClass : class
    {
        #region Methods

        /// <summary>
        /// Existses the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static bool Exists(object id)
        {
            return ActiveRecordBase<TMappedClass>.Exists(typeof(TMappedClass), id);
        }

        /// <summary>
        /// Existses the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static bool Exists(Criteria criteria)
        {
            return ActiveRecordBase<TMappedClass>.Exists(typeof(TMappedClass), criteria);
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            Criteria criteria = Criteria.For(typeof(TMappedClass));

            return ActiveRecordBase<TMappedClass>.Count(typeof(TMappedClass), criteria);
        }

        /// <summary>
        /// Counts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static int Count(Criteria criteria)
        {
            return ActiveRecordBase<TMappedClass>.Count(typeof(TMappedClass), criteria);
        }

        /// <summary>
        /// Pages the count.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static int PageCount(Criteria criteria, int pageSize)
        {
            return ActiveRecordBase<TMappedClass>.PageCount(typeof(TMappedClass), criteria, pageSize);
        }

        /// <summary>
        /// Pages the count.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static int PageCount(Criteria criteria, int pageSize, out int numberOfItems)
        {
            return ActiveRecordBase<TMappedClass>.PageCount(typeof(TMappedClass), criteria, pageSize, out numberOfItems);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public static List<TMappedClass> FindAll()
        {
            List<ActiveRecordBase> list = ActiveRecordBase.FindAll(typeof(TMappedClass));
            if (list == null)
                return null;

            List<TMappedClass> items = new List<TMappedClass>();

            foreach (var item in list)
                items.Add(item as TMappedClass);

            return items;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static List<TMappedClass> FindAll(Criteria criteria)
        {
            List<ActiveRecordBase> list = ActiveRecordBase.FindAll(typeof(TMappedClass), criteria);
            if (list == null)
                return null;

            List<TMappedClass> items = new List<TMappedClass>();

            foreach (var item in list)
                items.Add(item as TMappedClass);

            return items;
        }

        /// <summary>
        /// Finds all by property.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<TMappedClass> FindAllByProperty(string field, object value)
        {
            List<ActiveRecordBase> list = ActiveRecordBase.FindAllByProperty(typeof(TMappedClass), field, value);
            if (list == null)
                return null;

            List<TMappedClass> items = new List<TMappedClass>();

            foreach (var item in list)
                items.Add(item as TMappedClass);

            return items;
        }

        /// <summary>
        /// Projections the find all.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static List<TProjectionType> ProjectionFindAll<TProjectionType>(string field)
        {
            return ActiveRecordBase.ProjectionFindAll<TProjectionType>(typeof(TMappedClass), field);
        }

        /// <summary>
        /// Projections the find all.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static List<TProjectionType> ProjectionFindAll<TProjectionType>(Criteria criteria, string field)
        {
            return ActiveRecordBase.ProjectionFindAll<TProjectionType>(typeof(TMappedClass), criteria, field);
        }

        /// <summary>
        /// Sliceds the projection find all.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Criteria criteria, string field, int currentPage, int pageSize)
        {
            return ActiveRecordBase.SlicedProjectionFindAll<TProjectionType>(typeof(TMappedClass), criteria, field, currentPage, pageSize);
        }

        /// <summary>
        /// Sliceds the projection find all.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Criteria criteria, string field, int currentPage, int pageSize, out int numberOfPages)
        {
            return ActiveRecordBase.SlicedProjectionFindAll<TProjectionType>(typeof(TMappedClass), criteria, field, currentPage, pageSize, out numberOfPages);
        }

        /// <summary>
        /// Sliceds the projection find all.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Criteria criteria, string field, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems)
        {
            return ActiveRecordBase.SlicedProjectionFindAll<TProjectionType>(typeof(TMappedClass), criteria, field, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Sliceds the find all.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<TMappedClass> SlicedFindAll(int currentPage, int pageSize, out int numberOfPages)
        {
            int numberOfItems = 0;
            return ActiveRecordBase<TMappedClass>.SlicedFindAll(Criteria.For(typeof(TMappedClass)), currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Sliceds the find all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static List<TMappedClass> SlicedFindAll(Criteria criteria, int currentPage, int pageSize)
        {
            int numberOfPages = 0;
            int numberOfItems = 0;
            return ActiveRecordBase<TMappedClass>.SlicedFindAll(criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Sliceds the find all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<TMappedClass> SlicedFindAll(Criteria criteria, int currentPage, int pageSize, out int numberOfPages)
        {
            int numberOfItems = 0;
            return ActiveRecordBase<TMappedClass>.SlicedFindAll(criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Sliceds the find all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static List<TMappedClass> SlicedFindAll(Criteria criteria, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems)
        {
            List<ActiveRecordBase> list = ActiveRecordBase.SlicedFindAll(typeof(TMappedClass), criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
            if (list == null)
                return null;

            List<TMappedClass> items = new List<TMappedClass>();

            foreach (var item in list)
                items.Add(item as TMappedClass);

            return items;
        }

        /// <summary>
        /// Finds the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static TMappedClass Find(object id)
        {
            ActiveRecordBase item = ActiveRecordBase.Find(typeof(TMappedClass), id);

            if (item != null)
                return item as TMappedClass;

            return null;
        }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static TMappedClass FindOne(Criteria criteria)
        {
            ActiveRecordBase item = ActiveRecordBase.FindOne(typeof(TMappedClass), criteria);

            if (item != null)
                return item as TMappedClass;

            return null;
        }

        /// <summary>
        /// Finds the one by property.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TMappedClass FindOneByProperty(string field, object value)
        {
            ActiveRecordBase item = ActiveRecordBase.FindOneByProperty(typeof(TMappedClass), field, value);

            if (item != null)
                return item as TMappedClass;

            return null;
        }

        /// <summary>
        /// Finds the first.
        /// </summary>
        /// <returns></returns>
        public static TMappedClass FindFirst()
        {
            ActiveRecordBase item = ActiveRecordBase.FindFirst(typeof(TMappedClass));

            if (item != null)
                return item as TMappedClass;

            return null;
        }

        /// <summary>
        /// Finds the first.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static TMappedClass FindFirst(Criteria criteria)
        {
            ActiveRecordBase item = ActiveRecordBase.FindFirst(typeof(TMappedClass), criteria);

            if (item != null)
                return item as TMappedClass;

            return null;
        }

        /// <summary>
        /// Tries the find by property.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TMappedClass TryFindByProperty(string field, object value)
        {
            TMappedClass item = null;
            if (ActiveRecordBase<TMappedClass>.TryFindByProperty(field, value, out item))
                return item;

            return null;
        }

        /// <summary>
        /// Tries the find by property.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFindByProperty(string field, object value, out TMappedClass result)
        {
            try
            {
                Criteria criteria = Criteria.For(typeof(TMappedClass));
                criteria
                    .AddFilter(criteria.Eq(field, value));

                TMappedClass item = ActiveRecordBase<TMappedClass>.TryFind(criteria);
                result = item;

                return item != null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase<>.TryFindByProperty(type: [{0}])", typeof(TMappedClass)), ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Tries the find.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static TMappedClass TryFind(object id)
        {
            try
            {
                TMappedClass item = null;
                if (ActiveRecordBase<TMappedClass>.TryFind(id, out item))
                    return item;

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase<>.TryFind(type: [{0}])", typeof(TMappedClass)), ex);
                return null;
            }
        }

        /// <summary>
        /// Tries the find.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFind(object id, out TMappedClass result)
        {
            try
            {
                TMappedClass item = ActiveRecordBase<TMappedClass>.Find(id);
                result = item;

                return item != null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase<>.TryFind(type: [{0}])", typeof(TMappedClass)), ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Tries the find.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static TMappedClass TryFind(Criteria criteria)
        {
            try
            {
                TMappedClass item = ActiveRecordBase<TMappedClass>.FindOne(criteria);

                return item;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase<>.TryFind(type: [{0}])", typeof(TMappedClass)), ex);
                return null;
            }
        }

        /// <summary>
        /// Tries the find.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static bool TryFind(out TMappedClass result, Criteria criteria)
        {
            try
            {
                TMappedClass item = ActiveRecordBase<TMappedClass>.FindOne(criteria);
                result = item;

                return item != null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase<>.TryFind(type: [{0}])", typeof(TMappedClass)), ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Customs the find all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="customSQL">The custom SQL.</param>
        /// <returns></returns>
        public static List<TMappedClass> CustomFindAll(Criteria criteria, string customSQL)
        {
            List<ActiveRecordBase> list = ActiveRecordBase.CustomFindAll(typeof(TMappedClass), criteria, customSQL);
            if (list == null)
                return null;

            List<TMappedClass> items = new List<TMappedClass>();

            foreach (var item in list)
                items.Add(item as TMappedClass);

            return items;
        }

        /// <summary>
        /// Customs the non query criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static bool CustomNonQueryCriteria(Criteria criteria)
        {
            return ActiveRecordBase.CustomNonQueryCriteria(typeof(TMappedClass), criteria);
        }

        #endregion
    }
}