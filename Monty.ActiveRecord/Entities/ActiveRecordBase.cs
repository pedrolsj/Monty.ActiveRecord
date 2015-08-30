using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Monty.Log;
using System.Collections;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Active Record Base
    /// </summary>
    public class ActiveRecordBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the lazy objects.
        /// </summary>
        /// <value>
        /// The lazy objects.
        /// </value>
        internal Dictionary<String, Object> LazyObjects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is saved; otherwise, <c>false</c>.
        /// </value>
        internal bool IsSaved { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveRecordBase"/> class.
        /// </summary>
        public ActiveRecordBase()
        {
            this.IsSaved = false;
            this.LazyObjects = new Dictionary<string, object>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <returns></returns>
        public object GetId()
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(this.GetType());

            if (pk == null)
                return null;

            return pk.GetValue(this);
        }

        /// <summary>
        /// Sets the id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void SetId(object id)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(this.GetType());

            if (pk == null)
                return;

            pk.SetValue(this, id);
        }

        #endregion

        #region Custom Find Methods

        /// <summary>
        /// Customs the find all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="customSQL">The custom SQL.</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static List<ActiveRecordBase> CustomFindAll(Type type, Criteria criteria, string customSQL)
        {
            CheckTypeAtrribute(type);

            try
            {
                criteria.SQLString = customSQL;

                //Commit Filters
                criteria.CommitFilters();

                return ActiveRecordMaster.ExecuteQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.CustomFindAll(type: [{0}], customSQL: [{1}])", type, customSQL), ex);
                return null;
            }
        }

        /// <summary>
        /// Customs the non query criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static bool CustomNonQueryCriteria(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                //Commit Filters
                criteria.CommitFilters();

                return ActiveRecordMaster.ExecuteNonQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.CustomNonQueryCriteria(type: [{0}], criteria: [{1}])", type, criteria), ex);
                return false;
            }
        }

        #endregion

        #region Find Static Methods

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> FindAll(Type type)
        {
            Criteria criteria = Criteria.For(type);

            return FindAll(type, criteria);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> FindAll(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                criteria.SQLString = ActiveRecordMaster.MakeFullSelect(type);

                //Commit Filters
                criteria.CommitFilters();

                return ActiveRecordMaster.ExecuteQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindAll(type: [{0}])", type), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Finds all by property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> FindAllByProperty(Type type, string field, object value)
        {
            Criteria criteria = Criteria.For(type);
            criteria
                .AddFilter(criteria.Eq(field, value));

            return ActiveRecordBase.FindAll(type, criteria);
        }

        /// <summary>
        /// Returns only a selected field of all itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static List<TProjectionType> ProjectionFindAll<TProjectionType>(Type type, string field)
        {
            return ProjectionFindAll<TProjectionType>(type, Criteria.For(type), field);
        }

        /// <summary>
        /// Returns only a selected field of all itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static List<TProjectionType> ProjectionFindAll<TProjectionType>(Type type, Criteria criteria, string field)
        {
            CheckTypeAtrribute(type);

            try
            {
                criteria.SQLString = ActiveRecordMaster.MakeProjectionFullSelect(type, field);

                //Commit Filters
                criteria.CommitFilters();

                return ActiveRecordMaster.ExecuteProjectionQueryCriteria<TProjectionType>(criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindAll(type: [{0}])", type), ex);
                return null;
            }
        }

        /// <summary>
        /// Returns only a selected field of a portion itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Type type, Criteria criteria, string field, int currentPage, int pageSize)
        {
            int numberOfPages = 0;
            int numberOfItems = 0;
            return SlicedProjectionFindAll<TProjectionType>(type, criteria, field, currentPage, pageSize, out numberOfPages, out numberOfItems, false);
        }

        /// <summary>
        /// Returns only a selected field of a portion itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Type type, Criteria criteria, string field, int currentPage, int pageSize, out int numberOfPages)
        {
            int numberOfItems = 0;
            return SlicedProjectionFindAll<TProjectionType>(type, criteria, field, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Returns only a selected field of a portion itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Type type, Criteria criteria, string field, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems)
        {
            return SlicedProjectionFindAll<TProjectionType>(type, criteria, field, currentPage, pageSize, out numberOfPages, out numberOfItems, true);
        }

        /// <summary>
        /// Returns only a selected field of a portion itens on criteria
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="field">The field.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static List<TProjectionType> SlicedProjectionFindAll<TProjectionType>(Type type, Criteria criteria, string field, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems, bool findNumberOfPages)
        {
            CheckTypeAtrribute(type);

            try
            {
                if (findNumberOfPages)
                {
                    //Count
                    float count = ActiveRecordBase.Count(type, criteria);
                    numberOfItems = (int)count;

                    count = ((float)count / pageSize);
                    numberOfPages = (count) != ((int)count) ? ((int)count) + 1 : ((int)count);
                }
                else
                {
                    numberOfPages = -1;
                    numberOfItems = -1;
                }

                //Query
                criteria.IncludeOrderBy = true;
                criteria.SQLString = ActiveRecordMaster.MakeProjectionFullSelect(type, field);
                criteria.CommitFilters();

                criteria.MakePaginator(currentPage, pageSize);

                return ActiveRecordMaster.ExecuteProjectionQueryCriteria<TProjectionType>(criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.SlicedFindAll(type: [{0}])", type), ex);

                numberOfItems = 0;
                numberOfPages = 0;

                return null;
            }
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, int currentPage, int pageSize)
        {
            Criteria criteria = Criteria.For(type);

            int numberOfPages = 0;
            int numberOfItems = 0;
            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems, false);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, int currentPage, int pageSize, out int numberOfPages)
        {
            Criteria criteria = Criteria.For(type);

            int numberOfItems = 0;
            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems)
        {
            Criteria criteria = Criteria.For(type);

            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, Criteria criteria, int currentPage, int pageSize)
        {
            int numberOfPages = 0;
            int numberOfItems = 0;
            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems, false);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, Criteria criteria, int currentPage, int pageSize, out int numberOfPages)
        {
            int numberOfItems = 0;
            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, Criteria criteria, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems)
        {
            return SlicedFindAll(type, criteria, currentPage, pageSize, out numberOfPages, out numberOfItems, true);
        }

        /// <summary>
        /// Return a portion of the itens on the criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfPages">The number of pages.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <param name="findNumberOfPages">if set to <c>true</c> [find number of pages].</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static List<ActiveRecordBase> SlicedFindAll(Type type, Criteria criteria, int currentPage, int pageSize, out int numberOfPages, out int numberOfItems, bool findNumberOfPages)
        {
            CheckTypeAtrribute(type);

            try
            {
                if (findNumberOfPages)
                {
                    //Count
                    float count = ActiveRecordBase.Count(type, criteria);
                    numberOfItems = (int)count;

                    count = ((float)count / pageSize);
                    numberOfPages = (count) != ((int)count) ? ((int)count) + 1 : ((int)count);
                }
                else
                {
                    numberOfItems = -1;
                    numberOfPages = -1;
                }
                //Query
                criteria.IncludeOrderBy = true;
                criteria.SQLString = ActiveRecordMaster.MakeFullSelect(type);
                criteria.CommitFilters();
                //.CommitSlicedFilters((currentPage - 1) * pageSize, pageSize);
                //MakePaginator

                criteria.MakePaginator(currentPage, pageSize);

                return ActiveRecordMaster.ExecuteQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.SlicedFindAll(type: [{0}])", type), ex);

                numberOfItems = 0;
                numberOfPages = 0;

                return null;
            }
        }

        /// <summary>
        /// Finds an item by primary key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static ActiveRecordBase Find(Type type, object id)
        {
            CheckTypeAtrribute(type);

            try
            {
                PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

                if (pk == null)
                    return null;

                Criteria criteria = Criteria.For(type);
                criteria.SQLString = ActiveRecordMaster.MakePrimaryKeySelect(type);

                criteria.AddParameter("@CODE", pk.CurrentPropertyInfo.Name, id);

                List<ActiveRecordBase> list = ActiveRecordMaster.ExecuteQueryCriteria(type, criteria);

                if (list != null && list.Count > 0)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindAll(type: [{0}])", type), ex);
                return null;
            }
        }

        /// <summary>
        /// Finds a singular item.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ActiveRecordBase FindOneByProperty(Type type, string field, object value)
        {
            Criteria criteria = Criteria.For(type);
            criteria
                .AddFilter(criteria.Eq(field, value));

            return ActiveRecordBase.FindOne(type, criteria);
        }

        /// <summary>
        /// Finds a singular item.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        /// <exception cref="ObjectNotFoundException"></exception>
        /// <exception cref="Monty.ActiveRecord.MoreThanOneObjectNotFoundException"></exception>
        public static ActiveRecordBase FindOne(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                int count = Count(type, criteria);

                if (count == 0)
                    throw new ObjectNotFoundException();
                else if (count > 1)
                    throw new MoreThanOneObjectNotFoundException(count);

                return FindFirst(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindOne(type: [{0}])", type), ex);
                throw ex; ;
            }
        }

        /// <summary>
        /// Returns only the first row of the query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ActiveRecordBase FindFirst(Type type)
        {
            return FindFirst(type, Criteria.For(type));
        }

        /// <summary>
        /// Returns only the first row of the query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static ActiveRecordBase FindFirst(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                return SlicedFindAll(type, criteria, 0, 1)[0];
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindFirst(type: [{0}])", type), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Check if the criteria return results.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static bool Exists(Type type, object id)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            Criteria criteria = Criteria.For(type);
            criteria
                .AddFilter(criteria.Eq(pk.CurrentPropertyInfo.Name, id));

            return ActiveRecordBase.Exists(type, criteria);
        }

        /// <summary>
        /// Check if the criteria return results.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static bool Exists(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                return Count(type, criteria) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.Exists(type: [{0}])", type), ex);
                return false;
            }
        }

        /// <summary>
        /// Return the number of items
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static int Count(Type type)
        {
            Criteria criteria = Criteria.For(type);

            return ActiveRecordBase.Count(type, criteria);
        }

        /// <summary>
        /// Return the number of items
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static int Count(Type type, Criteria criteria)
        {
            CheckTypeAtrribute(type);

            try
            {
                criteria.SQLString = ActiveRecordMaster.MakeCountSelect(type);

                //Commit Filters
                criteria.IncludeOrderBy = false;
                criteria.CommitFilters();

                object count = ActiveRecordMaster.ExecuteScalarQueryCriteria(type, criteria);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.Count(type: [{0}])", type), ex);
                return 0;
            }
        }

        /// <summary>
        /// Return the number of pages by pageSize
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static int PageCount(Type type, Criteria criteria, int pageSize)
        {
            int numberOfItems = 0;
            return PageCount(type, criteria, pageSize, out numberOfItems);
        }

        /// <summary>
        /// Return the number of pages and itens by pageSize
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static int PageCount(Type type, Criteria criteria, int pageSize, out int numberOfItems)
        {
            CheckTypeAtrribute(type);

            int numberOfPages = 0;
            try
            {
                float mod = ActiveRecordBase.Count(type, criteria);

                numberOfItems = (int)mod;

                mod = ((float)mod / pageSize);
                numberOfPages = (mod) != ((int)mod) ? ((int)mod) + 1 : ((int)mod);
            }
            catch (DivideByZeroException)
            {
                numberOfItems = 0;
                numberOfPages = 0;
            }
            catch (Exception ex)
            {
                numberOfItems = 0;
                numberOfPages = 0;

                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.PageCount(type: [{0}])", type), ex);
            }

            return numberOfPages;
        }

        #endregion

        #region Save Methods

        /// <summary>
        /// May inserts or update.
        /// </summary>
        /// <returns></returns>
        public virtual bool Save()
        {
            if (ActiveRecordBase.Exists(this.GetType(), this.GetId()))
                return Update();
            else
                return Create();
        }

        #endregion

        #region Create Methods

        /// <summary>
        /// Execute a Insert.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException">Class attribute parameter 'Mutable' must be seted to 'true'</exception>
        public virtual bool Create()
        {
            ActiveRecordAttribute attribute = null;
            CheckTypeAtrribute(this.GetType(), ref attribute);

            if (!attribute.Mutable)
                throw new AttributeNotFoundException("Class attribute parameter 'Mutable' must be seted to 'true'");

            //Cascade Save
            if (ActiveRecordMaster.HasJoinedBase(this.GetType()) != null)
            {
                if (ActiveRecordMaster.GetAttribute(this.GetType().BaseType) != null)
                    if (!ActiveRecordMaster.Create(this.GetType().BaseType, this))
                        return false;
            }

            return ActiveRecordMaster.Create(this.GetType(), this);
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Execute a Update.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException">Class attribute parameter 'Mutable' must be seted to 'true'</exception>
        public virtual bool Update()
        {
            ActiveRecordAttribute attribute = null;
            CheckTypeAtrribute(this.GetType(), ref attribute);

            if (!attribute.Mutable)
                throw new AttributeNotFoundException("Class attribute parameter 'Mutable' must be seted to 'true'");

            //Cascade Update
            if (ActiveRecordMaster.HasJoinedBase(this.GetType()) != null)
            {
                if (ActiveRecordMaster.GetAttribute(this.GetType().BaseType) != null)
                    if (!ActiveRecordMaster.Update(this.GetType().BaseType, this))
                        return false;
            }

            return ActiveRecordMaster.Update(this.GetType(), this);
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Deletes this item
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException">Class attribute parameter 'Mutable' must be seted to 'true'</exception>
        public virtual bool Delete()
        {
            ActiveRecordAttribute attribute = null;
            CheckTypeAtrribute(this.GetType(), ref attribute);

            if (!attribute.Mutable)
                throw new AttributeNotFoundException("Class attribute parameter 'Mutable' must be seted to 'true'");

            if (!ActiveRecordMaster.Delete(this.GetType(), this))
                return false;

            //Cascade Delete
            if (ActiveRecordMaster.HasJoinedBase(this.GetType()) != null)
            {
                if (ActiveRecordMaster.GetAttribute(this.GetType().BaseType) != null)
                    if (!ActiveRecordMaster.Delete(this.GetType().BaseType, this))
                        return false;
            }

            return true;
        }

        #endregion

        #region Lazy Methods

        /// <summary>
        /// Lazy load a property (BelongsTo).
        /// </summary>
        /// <typeparam name="TPropertyClass">The type of the property class.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected TPropertyClass LazyLoad<TPropertyClass>(string property)
            where TPropertyClass : ActiveRecordBase
        {
            if (!this.LazyObjects.ContainsKey(property))
                return null;

            BelongsToAttribute attr = ActiveRecordMaster.GetBelongsTo(this.GetType(), property);

            if (this.LazyObjects[property] == null || String.IsNullOrEmpty(this.LazyObjects[property].ToString()))
                return null;

            TPropertyClass item = ActiveRecordBase<TPropertyClass>.Find(this.LazyObjects[property]);

            if (attr.NotFoundBehaviour == NotFoundBehaviour.Exception && item == null)
                throw new ObjectNotFoundException(typeof(TPropertyClass).Name);

            return item;
        }

        /// <summary>
        /// Lazy load a property (HasMany).
        /// </summary>
        /// <typeparam name="TPropertyClass">The type of the property class.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected List<TPropertyClass> LazyLoadMany<TPropertyClass>(string property)
            where TPropertyClass : ActiveRecordBase
        {
            HasManyAttribute attr = ActiveRecordMaster.GetHasMany(this.GetType(), property);

            List<TPropertyClass> items = this.LazyLoadMany<TPropertyClass>(attr);

            return items;
        }

        /// <summary>
        /// Lazy load a property (HasMany).
        /// </summary>
        /// <typeparam name="TPropertyClass">The type of the property class.</typeparam>
        /// <param name="attr">The attr.</param>
        /// <returns></returns>
        /// <exception cref="AttributeNotFoundException"></exception>
        private List<TPropertyClass> LazyLoadMany<TPropertyClass>(HasManyAttribute attr)
            where TPropertyClass : ActiveRecordBase
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(this.GetType());

            if (attribute == null)
                throw new AttributeNotFoundException();

            try
            {
                PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(this.GetType());

                if (pk == null)
                    return null;

                Criteria criteria = Criteria.For<TPropertyClass>();
                criteria.SQLString = ActiveRecordMaster.MakeHasManySelect(this.GetType(), attr.CurrentPropertyInfo.Name);

                criteria.AddParameter("@CODE", pk.CurrentPropertyInfo.Name, pk.CurrentPropertyInfo.GetValue(this, null));

                return ActiveRecordMaster.ExecuteQueryCriteria<TPropertyClass>(criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.FindByForeignKey(attr: [{0}])", attr), ex);
                return null;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks the type atrribute.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void CheckTypeAtrribute(Type type)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            CheckTypeAtrribute(type, ref attribute, ref pk);
        }

        /// <summary>
        /// Checks the type atrribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        public static void CheckTypeAtrribute(Type type, ref ActiveRecordAttribute attribute)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);
            CheckTypeAtrribute(type, ref attribute, ref pk);
        }

        /// <summary>
        /// Checks the type atrribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="AttributeNotFoundException">Class attribute not found</exception>
        public static void CheckTypeAtrribute(Type type, ref ActiveRecordAttribute attribute, ref PrimaryKeyAttribute pk)
        {
            if (attribute == null)
                throw new AttributeNotFoundException("Class attribute not found");

            if (pk == null)
                throw new AttributeNotFoundException("Primary key attribute not found");
        }

        #endregion

        #region To String

        /// <summary>
        /// Return and JSON-like string of this item.
        /// </summary>
        /// <returns></returns>
        public virtual string Explode()
        {
            return ActiveRecordBase.Explode(String.Empty, this);
        }

        /// <summary>
        /// Return and JSON-like string of this item with a padding.
        /// </summary>
        /// <param name="padding">The padding.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private static string Explode(string padding, object obj)
        {
            if (obj == null)
                return null;

            string subPadding = padding + "\t";

            Type type = obj.GetType();

            if (type.IsValueType || type.IsEnum || type.IsArray)
                return obj.ToString();

            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}({1}) {{ \n", padding, type.Name);

            foreach (PropertyInfo info in type.GetProperties())
                try
                {
                    if (info.CanRead)
                    {
                        object val = type.GetProperty(info.Name).GetValue(obj, null);
                        if (val == null)
                            result.AppendFormat("{0}{1} : [NULL],\n", subPadding, info.Name);
                        else
                            if ((val as ActiveRecordBase) != null && info.PropertyType != obj.GetType())
                            result.AppendFormat("{0}{1} : {2},\n", subPadding, info.Name, ActiveRecordBase.Explode(subPadding, val));
                        else if ((val as IList) != null)
                            result.AppendFormat("{0}{1} : {2},\n", subPadding, info.Name, (val as IList).Explode(subPadding));
                        else
                            result.AppendFormat("{0}{1} : \"{2}\",\n", subPadding, info.Name, val);
                    }
                    else
                        result.AppendFormat("{0}{1} : [NO GETTER],\n", subPadding, info.Name);
                }
                catch (Exception ex)
                {
                    result.AppendFormat("{0}{1} : [Error: {2}],\n", subPadding, info.Name, ex.Message);
                }
            result.AppendFormat("{0}}}", padding);
            return result.ToString();
        }

        /// <summary>
        /// Will return the ClassName#PrimaryKeyValue
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            return String.Format("{0}#{1}", base.ToString(), this.GetId());
        }

        #endregion
    }
}