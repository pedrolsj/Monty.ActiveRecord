using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Monty.Log;
namespace Monty.ActiveRecord
{
    /// <summary>
    /// Criteria
    /// </summary>
    public class Criteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current type.
        /// </summary>
        /// <value>
        /// The type of the current.
        /// </value>
        protected Type CurrentType { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public List<DbParameter> Parameters { get; protected set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public List<Criteria.Filter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public List<Criteria.Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public DbCommand Command { get; protected set; }

        /// <summary>
        /// Gets or sets the SQL string.
        /// </summary>
        /// <value>
        /// The SQL string.
        /// </value>
        public string SQLString { get { return Command.CommandText; } set { Command.CommandText = value; } }

        /// <summary>
        /// Gets or sets a value indicating whether [include order by].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include order by]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeOrderBy { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Criteria"/> class from being created.
        /// </summary>
        /// <param name="currentType">Type of the current.</param>
        private Criteria(Type currentType)
        {
            CurrentType = currentType;
            IncludeOrderBy = true;
            Parameters = new List<DbParameter>();
            Filters = new List<Filter>();
            Orders = new List<Order>();
            Command = ActiveRecordMaster.Command();
            SQLString = ActiveRecordMaster.MakeFullSelect(currentType);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fors this instance.
        /// </summary>
        /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
        /// <returns></returns>
        public static Criteria For<TMappedClass>()
            where TMappedClass : ActiveRecordBase
        {
            Criteria criteria = new Criteria(typeof(TMappedClass));

            return criteria;
        }

        /// <summary>
        /// Fors the specified current type.
        /// </summary>
        /// <param name="currentType">Type of the current.</param>
        /// <returns></returns>
        public static Criteria For(Type currentType)
        {
            return new Criteria(currentType);
        }

        /// <summary>
        /// Sets the SQL string.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public void SetSQLString(string sql)
        {
            Command.CommandText = sql;
        }

        /// <summary>
        /// Commits the filters.
        /// </summary>
        public void CommitFilters()
        {
            //Filters
            string filters = String.Empty;
            foreach (var filter in Filters)
                AddParameter(ref filters, "AND", filter);

            this.SQLString = String.Format("{0} {1}", this.SQLString, filters);

            if (this.IncludeOrderBy)
            {
                //Order By
                string orders = String.Empty;
                foreach (var order in Orders)
                    if (order != null && !String.IsNullOrEmpty(order.Format))
                    {
                        //Add Sql
                        orders = String.Format("{0}{1}, ", orders, order.Format);
                    }

                if (!String.IsNullOrEmpty(orders))
                    orders = String.Format("ORDER BY {0}", orders.Substring(0, orders.Length - 2));

                this.SQLString = String.Format("{0} {1}", this.SQLString, orders);
            }
        }

        /// <summary>
        /// Makes the paginator.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <exception cref="System.InvalidOperationException">Failed to find a FROM clause</exception>
        public void MakePaginator(int pageNumber, int pageSize)
        {
            string sql = this.SQLString;
            if (ActiveRecordMaster.DataBaseType == DataBaseType.SQLServer)
            {
                string pk = ActiveRecordMaster.FormatField(ActiveRecordMaster.GetAttribute(CurrentType).FullName, ActiveRecordMaster.GetPrimaryKey(CurrentType).Column);
                string clearPK = ActiveRecordMaster.FormatField(ActiveRecordMaster.GetAttribute(CurrentType).FullName, ActiveRecordMaster.GetPrimaryKey(CurrentType).Column).Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", String.Empty);

                int fromIndex = sql.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
                int whereIndex = sql.IndexOf("WHERE", fromIndex, StringComparison.OrdinalIgnoreCase);
                int groupByIndex = sql.IndexOf("GROUP BY", StringComparison.OrdinalIgnoreCase);
                int orderByIndex = sql.IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);

                var selectColumns = String.Empty;

                //Remove nome das tabelas do subselect
                var clearSelectColumns = String.Empty;
                foreach (var row in sql.Substring(7, fromIndex - 7).TrimEnd().Split(','))
                    try
                    {
                        clearSelectColumns += String.Format("{0},", row.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", String.Empty));
                    }
                    catch { clearSelectColumns += String.Format("{0},", row.ToString()); }

                if (clearSelectColumns.Length > 0)
                    clearSelectColumns = clearSelectColumns.Substring(0, clearSelectColumns.Length - 1);

                //Adiciona os AS ''
                foreach (var row in sql.Substring(7, fromIndex - 7).TrimEnd().Split(','))
                    selectColumns += String.Format("{0} AS {1},", row, row.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", String.Empty));

                if (selectColumns.Length > 0)
                    selectColumns = selectColumns.Substring(0, selectColumns.Length - 1);


                string tables = String.Empty;
                if (fromIndex == -1)
                    throw new InvalidOperationException("Failed to find a FROM clause");
                else
                {
                    var startPos = fromIndex + 5;
                    var endPos = whereIndex != -1
                        ? whereIndex
                        : groupByIndex != -1
                                 ? groupByIndex
                                 : orderByIndex != -1
                                       ? orderByIndex
                                       : sql.Length;
                    tables = sql.Substring(startPos, endPos - startPos).TrimEnd();
                }

                // where
                var where = String.Empty;
                if (whereIndex != -1)
                {
                    var startPos = whereIndex + 6;
                    var endPos = groupByIndex != -1
                                 ? groupByIndex
                                 : orderByIndex != -1
                                       ? orderByIndex
                                       : sql.Length;

                    where = sql.Substring(startPos, endPos - startPos).TrimEnd();
                }

                // group by
                var groupByColumns = String.Empty;
                if (groupByIndex != -1)
                {
                    var startPos = groupByIndex + 9;
                    var endPos = orderByIndex != -1
                                       ? orderByIndex
                                       : sql.Length;

                    groupByColumns = sql.Substring(startPos, endPos - startPos).TrimEnd();
                }

                //Remove table names from group by
                var clearGroupBy = String.Empty;
                foreach (var row in groupByColumns.Split(','))
                    clearGroupBy += String.Format("{0},", row.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", String.Empty));

                if (clearGroupBy.Length > 0)
                    clearGroupBy = clearGroupBy.Substring(0, clearGroupBy.Length - 1);

                groupByColumns = clearGroupBy;


                // order by
                var orderByColumns = String.Empty;
                if (orderByIndex == -1)
                    orderByColumns = pk;
                else
                {
                    var startPos = orderByIndex + 9;
                    var endPos = sql.Length;
                    orderByColumns = sql.Substring(startPos, endPos - startPos);
                }

                //Remove table names from order by
                var clearOrderBy = String.Empty;
                foreach (var row in orderByColumns.Split(','))
                    clearOrderBy += String.Format("{0},", row.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", String.Empty));

                if (clearOrderBy.Length > 0)
                    clearOrderBy = clearOrderBy.Substring(0, clearOrderBy.Length - 1);

                //Calculate page number
                if (pageNumber == 0)
                    pageNumber = 1;

                var firstRow = ((pageNumber - 1) * pageSize) + 1;
                var lastRow = firstRow + pageSize - 1;

                var innerFrom = tables;
                if (!string.IsNullOrEmpty(where))
                    innerFrom += "\r\n    WHERE " + where + "\r\n";

                /*WITH Members  AS
    (
    SELECT	M_NAME, M_POSTS, M_LASTPOSTDATE, M_LASTHEREDATE, M_DATE, M_COUNTRY,
    ROW_NUMBER() OVER (ORDER BY M_POSTS DESC) AS RowNumber
    FROM	dbo.FORUM_MEMBERS
    )
    SELECT	RowNumber, M_NAME, M_POSTS, M_LASTPOSTDATE, M_LASTHEREDATE, M_DATE, M_COUNTRY
    FROM	Members
    WHERE	RowNumber BETWEEN 1 AND 20
    ORDER BY RowNumber ASC;
    */
                sql = string.Format(@"WITH Paged AS 
(
    SELECT {0},
    ROW_NUMBER() OVER (ORDER BY {1}) AS RowNumber
    FROM {2}
)
SELECT RowNumber, {5}
FROM Paged
WHERE RowNumber BETWEEN {3} AND {4}
", selectColumns, orderByColumns, innerFrom, firstRow, lastRow, clearSelectColumns);

                if (groupByColumns != String.Empty)
                    sql += "GROUP BY " + groupByColumns + "\r\n";
                sql += "ORDER BY " + clearOrderBy;


            }
            else if (ActiveRecordMaster.DataBaseType == DataBaseType.MySQL)
            {
                sql += String.Format(" LIMIT {0},{1}", pageNumber * pageSize, pageSize);
            }

            this.SQLString = sql;
        }

        /// <summary>
        /// Adds a filter
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Criteria Add(Filter filter)
        {
            return AddFilter(filter);
        }

        /// <summary>
        /// Adds a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Criteria AddFilter(Filter filter)
        {
            Filters.Add(filter);

            return this;
        }

        /// <summary>
        /// Adds a order by
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Criteria Add(Order order)
        {
            return AddOrder(order);
        }

        /// <summary>
        /// Adds a order by
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Criteria AddOrder(Order order)
        {
            Orders.Add(order);

            return this;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="aggregator">The aggregator.</param>
        /// <param name="filter">The filter.</param>
        public void AddParameter(ref string sql, string aggregator, Filter filter)
        {
            //Add Sql
            if (!String.IsNullOrEmpty(filter.Format))
                sql = String.Format("{0}{1} {2} ", sql, aggregator, filter.Format);
            else if (filter.SubFilters.Count > 0 && String.IsNullOrEmpty(filter.PreFilterFormat))//Se nao tem formato eh agregador OR ou AND
                sql = String.Format("{0}{1} (", sql, aggregator);
            else if (filter.SubFilters.Count > 0 && !String.IsNullOrEmpty(filter.PreFilterFormat))//PARA NOT()
                sql = String.Format("{0}{1} {2} (", sql, aggregator, filter.PreFilterFormat);
            else//Filtro AND ou OR Vazio
                return;

            //Add Parameters
            if (filter.Attribute != null || filter.Values != null)
            {
                //Busca parametros dentro do value
                foreach (var value in filter.Values)
                {
                    if (value.Value != null && value.Value as System.Collections.IList != null)
                    {
                        //Fix for in
                        for (int i = 0; i < (value.Value as System.Collections.IList).Count; i++)
                            AddParameter(String.Format("{0}in{1}", value.Key, i), filter.Attribute.CurrentPropertyInfo.Name, (value.Value as System.Collections.IList)[i]);
                    }
                    else
                        AddParameter(value.Key, filter.Attribute.CurrentPropertyInfo.Name, value.Value);
                }
            }

            //Adiciona sub filtros (recursivamente
            for (int i = 0; i < filter.SubFilters.Count; i++)
                AddParameter(ref sql, (i == 0 ? String.Empty : filter.SubFiltersAggregator), filter.SubFilters[i]);

            //
            if (String.IsNullOrEmpty(filter.Format))
                sql = String.Format("{0}) ", sql);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Criteria AddParameter(string key, string property, object value)
        {
            if (this.Parameters.Exists(delegate (DbParameter param) { return param.ParameterName.ToLower() == key.ToLower(); }))
                return this;

            DbParameter parameter = Command.CreateParameter();
            parameter.ParameterName = key;

            FieldAttribute attr = ActiveRecordMaster.GetProperty(CurrentType, property);

            if (attr == null)
                attr = ActiveRecordMaster.GetBelongsTo(CurrentType, property);

            if (attr == null)
                attr = ActiveRecordMaster.GetPrimaryKeyProperty(CurrentType, property);

            if (attr != null)
            {
                parameter.IsNullable = !attr.NotNull;

                if (attr.Length > 0)
                    parameter.Size = attr.Length;

                if (attr.ColumnDbType != System.Data.DbType.Object)
                    parameter.DbType = attr.ColumnDbType;
            }

            parameter.Value = value == null ? DBNull.Value : value;

            this.Parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Criteria AddParameter(string name, object value, DbType type)
        {
            if (this.Parameters.Exists(delegate (DbParameter param) { return param.ParameterName.ToLower() == name.ToLower(); }))
                return this;

            DbParameter parameter = Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value == null ? DBNull.Value : value;

            this.Parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Clears all filters.
        /// </summary>
        public void ClearAllFilters()
        {
            Filters = new List<Filter>();
        }

        /// <summary>
        /// Clears all orders.
        /// </summary>
        public void ClearAllOrders()
        {
            Orders = new List<Order>();
        }

        #endregion

        #region Filter Methods

        /// <summary>
        /// Not - Not($subfilter$)
        /// </summary>
        /// <param name="subfilter">The subfilter.</param>
        /// <returns></returns>
        public Filter Not(Filter subfilter)
        {
            return Filter.Not(subfilter);
        }

        /// <summary>
        /// Use the AddSubFilter to link filters
        /// </summary>
        /// <returns></returns>
        public Filter And()
        {
            return Filter.And();
        }

        /// <summary>
        /// Use the AddSubFilter to link filters
        /// </summary>
        /// <returns></returns>
        public Filter Or()
        {
            return Filter.Or();
        }

        /// <summary>
        /// SQL - $sql$
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public Filter Sql(string sql)
        {
            return Filter.Sql(sql);
        }

        /// <summary>
        /// Like - $property$ like $value$
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Like(string property, string value)
        {
            return Filter.Like(CurrentType, property, value);
        }

        /// <summary>
        /// Like - $property$ like $value$
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="matchMode">The match mode.</param>
        /// <returns></returns>
        public Filter Like(string property, string value, MatchMode matchMode)
        {
            return Filter.Like(CurrentType, property, value, matchMode);
        }

        /// <summary>
        /// Between - $property$ between $valueL$ and $valueH$
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="valueL">The value l.</param>
        /// <param name="valueH">The value h.</param>
        /// <returns></returns>
        public Filter Between(string property, object valueL, object valueH)
        {
            return Filter.Between(CurrentType, property, valueL, valueH);

        }

        /// <summary>
        /// Equality - $property$ = $value$
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Eq(string property, object value)
        {
            return Filter.Eq(CurrentType, property, value);
        }

        /// <summary>
        /// Nots the eq.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter NotEq(string property, object value)
        {
            return Filter.NotEq(CurrentType, property, value);
        }

        /// <summary>
        /// Les the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Le(string property, object value)
        {
            return Filter.Le(CurrentType, property, value);
        }

        /// <summary>
        /// Lts the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Lt(string property, object value)
        {
            return Filter.Lt(CurrentType, property, value);
        }

        /// <summary>
        /// Ges the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Ge(string property, object value)
        {
            return Filter.Ge(CurrentType, property, value);
        }

        /// <summary>
        /// Gts the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Filter Gt(string property, object value)
        {
            return Filter.Gt(CurrentType, property, value);
        }

        /// <summary>
        /// Determines whether the specified property is null.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public Filter IsNull(string property)
        {
            return Filter.IsNull(CurrentType, property);
        }

        /// <summary>
        /// Determines whether [is not null] [the specified property].
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public Filter IsNotNull(string property)
        {
            return Filter.IsNotNull(CurrentType, property);
        }

        /// <summary>
        /// Determines whether [is not null or empty] [the specified property].
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public Filter IsNotNullOrEmpty(string property)
        {
            return Filter.IsNotNullOrEmpty(CurrentType, property);
        }

        /// <summary>
        /// Informations the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public Filter In(string property, System.Collections.IList values)
        {
            return Filter.In(CurrentType, property, values);
        }

        #endregion

        #region Order Methods

        /// <summary>
        /// Orders by random.
        /// </summary>
        /// <returns></returns>
        public Order OrderByRandom()
        {
            ClearAllOrders();

            return Order.Random();
        }

        /// <summary>
        /// Order by Asc
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public Order Asc(string property)
        {
            return Order.Asc(CurrentType, property);
        }

        /// <summary>
        /// Order by Desc
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public Order Desc(string property)
        {
            return Order.Desc(CurrentType, property);
        }

        #endregion

        /// <summary>
        /// Filter
        /// </summary>
        public class Filter
        {
            #region Properties

            /// <summary>
            /// Gets or sets the format.
            /// </summary>
            /// <value>
            /// The format.
            /// </value>
            public string Format { get; set; }

            /// <summary>
            /// Gets or sets the values.
            /// </summary>
            /// <value>
            /// The values.
            /// </value>
            public List<KeyValuePair<String, Object>> Values { get; set; }

            /// <summary>
            /// Gets or sets the attribute.
            /// </summary>
            /// <value>
            /// The attribute.
            /// </value>
            public FieldAttribute Attribute { get; set; }

            /// <summary>
            /// Gets or sets the custom type.
            /// </summary>
            /// <value>
            /// The type of the custom.
            /// </value>
            public Type CustomType { get; set; }

            /// <summary>
            /// Gets or sets the sub filters.
            /// </summary>
            /// <value>
            /// The sub filters.
            /// </value>
            public List<Filter> SubFilters { get; set; }

            /// <summary>
            /// Gets or sets the sub filters aggregator.
            /// </summary>
            /// <value>
            /// The sub filters aggregator.
            /// </value>
            public string SubFiltersAggregator { get; set; }

            /// <summary>
            /// Gets or sets the pre filter format.
            /// </summary>
            /// <value>
            /// The pre filter format.
            /// </value>
            public string PreFilterFormat { get; set; }

            #endregion

            #region Contructors

            /// <summary>
            /// Prevents a default instance of the <see cref="Filter"/> class from being created.
            /// </summary>
            private Filter()
            {
                Values = new List<KeyValuePair<string, object>>();
                SubFilters = new List<Filter>();
                SubFiltersAggregator = "AND";
                PreFilterFormat = String.Empty;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Creates this instance.
            /// </summary>
            /// <returns></returns>
            public static Filter Create()
            {
                return new Filter();
            }

            /// <summary>
            /// Adds a sub filter.
            /// </summary>
            /// <param name="subFilter">The sub filter.</param>
            /// <returns></returns>
            public Filter AddSubFilter(Filter subFilter)
            {
                this.SubFilters.Add(subFilter);

                return this;
            }

            /// <summary>
            /// Gets the value key.
            /// </summary>
            /// <returns></returns>
            public string GetValueKey()
            {
                if (Values == null || Values.Count == 0)
                    return String.Empty;

                return Values[0].Key;
            }

            /// <summary>
            /// Sets the value for column.
            /// </summary>
            /// <param name="column">The column.</param>
            /// <param name="value">The value.</param>
            public void SetValueForColumn(string column, object value)
            {
                this.Values.Add(new KeyValuePair<string, object>(ActiveRecordMaster.FormatParameterKey(column), value));
            }

            #endregion

            #region Filter Methods

            /// <summary>
            /// Not - Not($subfilter$)
            /// </summary>
            /// <param name="subfilter">The subfilter.</param>
            /// <returns></returns>
            public static Filter Not(Filter subfilter)
            {
                Filter filter = Filter.Create();

                filter.Format = null;
                filter.PreFilterFormat = "NOT";
                filter.AddSubFilter(subfilter);

                return filter;
            }

            /// <summary>
            /// Use the AddSubFilter to link filters
            /// </summary>
            /// <returns></returns>
            public static Filter And()
            {
                Filter filter = Filter.Create();

                filter.Format = null;
                filter.SubFiltersAggregator = "AND";

                return filter;
            }

            /// <summary>
            /// Use the AddSubFilter to link filters
            /// </summary>
            /// <returns></returns>
            public static Filter Or()
            {
                Filter filter = Filter.Create();

                filter.Format = null;
                filter.SubFiltersAggregator = "OR";

                return filter;
            }

            /// <summary>
            /// SQL - $sql$
            /// </summary>
            /// <param name="sql">The SQL.</param>
            /// <returns></returns>
            public static Filter Sql(string sql)
            {
                Filter filter = Filter.Create();

                filter.Format = sql;

                return filter;
            }

            /// <summary>
            /// Like - $property$ like $value$
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Like(Type type, string property, string value)
            {
                return Like(type, property, value, MatchMode.Anywhere);
            }

            /// <summary>
            /// Like - $property$ like $value$
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <param name="matchMode">The match mode.</param>
            /// <returns></returns>
            public static Filter Like(Type type, string property, string value, MatchMode matchMode)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (ActiveRecordMaster.DataBaseType == DataBaseType.SQLServer)
                {
                    filter.SetValueForColumn(filter.Attribute.Column, value);
                    switch (matchMode)
                    {
                        case MatchMode.Exact:
                            filter.Format = String.Format(" {0} = {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                            break;
                        case MatchMode.End:
                            filter.Format = String.Format(" {0} LIKE '%' + {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                            break;
                        case MatchMode.Start:
                            filter.Format = String.Format(" {0} LIKE {1} + '%' ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                            break;
                        case MatchMode.Anywhere:
                        default:
                            filter.Format = String.Format(" {0} LIKE '%' + {1} + '%' ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                            break;
                    }
                }
                else if (ActiveRecordMaster.DataBaseType == DataBaseType.MySQL)
                {
                    switch (matchMode)
                    {
                        case MatchMode.Exact:
                            filter.SetValueForColumn(filter.Attribute.Column, value);
                            break;
                        case MatchMode.End:
                            filter.SetValueForColumn(filter.Attribute.Column, String.Format("%{0}", value));
                            break;
                        case MatchMode.Start:
                            filter.SetValueForColumn(filter.Attribute.Column, String.Format("{0}%", value));
                            break;
                        case MatchMode.Anywhere:
                        default:
                            filter.SetValueForColumn(filter.Attribute.Column, String.Format("%{0}%", value));
                            break;
                    }
                    filter.Format = String.Format(" {0} LIKE {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                }

                return filter;
            }

            /// <summary>
            /// Between - $property$ between $valueL$ and $valueH$
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="valueL">The value l.</param>
            /// <param name="valueH">The value h.</param>
            /// <returns></returns>
            public static Filter Between(Type type, string property, object valueL, object valueH)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (filter.Attribute.GetType() == typeof(BelongsToAttribute) && (valueL as ActiveRecordBase) != null)
                {
                    filter.SetValueForColumn(filter.Attribute.Column, (valueL as ActiveRecordBase).GetId());
                    filter.SetValueForColumn(filter.Attribute.Column, (valueH as ActiveRecordBase).GetId());

                    filter.Format = String.Format(" {0} BETWEEN {1} AND {2} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.Values[0].Key, filter.Values[1].Key);
                }
                else
                {
                    filter.SetValueForColumn(filter.Attribute.Column, valueL);
                    filter.SetValueForColumn(filter.Attribute.Column, valueH);

                    filter.Format = String.Format(" {0} BETWEEN {1} AND {2} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.Values[0].Key, filter.Values[1].Key);
                }

                return filter;
            }

            /// <summary>
            /// Eqs the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Eq(Type type, string property, object value)
            {
                try
                {
                    Filter filter = Filter.Create();
                    filter.Attribute = FieldAttribute.GetAttribute(type, property);

                    ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                    if (filter.Attribute.GetType() == typeof(BelongsToAttribute) && (value as ActiveRecordBase) != null)
                    {
                        filter.SetValueForColumn(filter.Attribute.Column, (value as ActiveRecordBase).GetId());
                        filter.Format = String.Format(" {0} = {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                    }
                    else
                    {
                        filter.SetValueForColumn(filter.Attribute.Column, value);
                        filter.Format = String.Format(" {0} = {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                    }

                    return filter;
                }
                catch (Exception ex)
                {
                    LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute Criteria.Eq(type: [{0}], property: [{1}], value: [{2}])", type, property, value), ex);
                    throw ex;
                }
            }

            /// <summary>
            /// Nots the eq.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter NotEq(Type type, string property, object value)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (filter.Attribute.GetType() == typeof(BelongsToAttribute) && (value as ActiveRecordBase) != null)
                {
                    filter.SetValueForColumn(filter.Attribute.Column, (value as ActiveRecordBase).GetId());
                    filter.Format = String.Format(" {0} <> {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                }
                else
                {
                    filter.SetValueForColumn(filter.Attribute.Column, value);
                    filter.Format = String.Format(" {0} <> {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());
                }

                return filter;
            }

            /// <summary>
            /// Les the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Le(Type type, string property, object value)
            {
                Filter filter = Filter.Create();
                filter.Attribute = filter.Attribute = FieldAttribute.GetAttribute(type, property); ;

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                filter.SetValueForColumn(filter.Attribute.Column, value);
                filter.Format = String.Format(" {0} <= {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());

                return filter;
            }

            /// <summary>
            /// Lts the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Lt(Type type, string property, object value)
            {
                Filter filter = Filter.Create();
                filter.Attribute = filter.Attribute = FieldAttribute.GetAttribute(type, property); ;

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                filter.SetValueForColumn(filter.Attribute.Column, value);
                filter.Format = String.Format(" {0} < {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());

                return filter;
            }

            /// <summary>
            /// Ges the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Ge(Type type, string property, object value)
            {
                Filter filter = Filter.Create();
                filter.Attribute = filter.Attribute = FieldAttribute.GetAttribute(type, property); ;

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                filter.SetValueForColumn(filter.Attribute.Column, value);
                filter.Format = String.Format(" {0} >= {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());

                return filter;
            }

            /// <summary>
            /// Gts the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Gt(Type type, string property, object value)
            {
                Filter filter = Filter.Create();
                filter.Attribute = filter.Attribute = FieldAttribute.GetAttribute(type, property); ;

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                filter.SetValueForColumn(filter.Attribute.Column, value);
                filter.Format = String.Format(" {0} > {1} ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), filter.GetValueKey());

                return filter;
            }

            /// <summary>
            /// Determines whether the specified type is null.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNull(Type type, string property)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (filter.Attribute.GetType() == typeof(BelongsToAttribute))
                {
                    filter.Format = String.Format(" {0} IS NULL ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }
                else
                {
                    filter.Format = String.Format(" {0} IS NULL ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }

                return filter;
            }

            /// <summary>
            /// Determines whether [is not null] [the specified type].
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNotNull(Type type, string property)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (filter.Attribute.GetType() == typeof(BelongsToAttribute))
                {
                    filter.Format = String.Format(" {0} IS NOT NULL ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }
                else
                {
                    filter.Format = String.Format(" {0} IS NOT NULL ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }

                return filter;
            }

            /// <summary>
            /// Determines whether [is not null or empty] [the specified type].
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNotNullOrEmpty(Type type, string property)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                if (filter.Attribute.GetType() == typeof(BelongsToAttribute))
                {
                    filter.Format = String.Format(" {0} IS NOT NULL AND {0} <> '' ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }
                else
                {
                    filter.Format = String.Format(" {0} IS NOT NULL AND {0} <> ''", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column));
                }

                return filter;
            }

            /// <summary>
            /// Informations the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <param name="values">The values.</param>
            /// <returns></returns>
            public static Filter In(Type type, string property, System.Collections.IList values)
            {
                Filter filter = Filter.Create();
                filter.Attribute = FieldAttribute.GetAttribute(type, property);

                ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(filter.Attribute.CurrentPropertyInfo.DeclaringType);

                string sqlin = String.Empty;
                for (int i = 0; i < values.Count; i++)
                {
                    filter.SetValueForColumn(filter.Attribute.Column, values[i]);
                    sqlin += String.Format("{0},", filter.Values[i].Key);
                }

                if (sqlin.Length > 0)
                    sqlin = sqlin.Trim().Substring(0, sqlin.Length - 1);

                filter.Format = String.Format(" {0} IN ({1}) ", ActiveRecordMaster.FormatField(attribute.FullName, filter.Attribute.Column), sqlin);

                return filter;
            }

            #endregion

            #region Generic Filter Methods

            /// <summary>
            /// Likes the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Like<TMappedClass>(string property, string value)
            {
                return Like(typeof(TMappedClass), property, value, MatchMode.Anywhere);
            }

            /// <summary>
            /// Likes the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <param name="matchMode">The match mode.</param>
            /// <returns></returns>
            public static Filter Like<TMappedClass>(string property, string value, MatchMode matchMode)
            {
                return Like(typeof(TMappedClass), property, value, matchMode);
            }

            /// <summary>
            /// Betweens the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="valueL">The value l.</param>
            /// <param name="valueH">The value h.</param>
            /// <returns></returns>
            public static Filter Between<TMappedClass>(string property, object valueL, object valueH)
            {
                return Between(typeof(TMappedClass), property, valueL, valueH);
            }

            /// <summary>
            /// Eqs the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Eq<TMappedClass>(string property, object value)
            {
                return Eq(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Nots the eq.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter NotEq<TMappedClass>(string property, object value)
            {
                return NotEq(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Les the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Le<TMappedClass>(string property, object value)
            {
                return Le(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Lts the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Lt<TMappedClass>(string property, object value)
            {
                return Lt(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Ges the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Ge<TMappedClass>(string property, object value)
            {
                return Ge(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Gts the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static Filter Gt<TMappedClass>(string property, object value)
            {
                return Gt(typeof(TMappedClass), property, value);
            }

            /// <summary>
            /// Determines whether the specified property is null.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNull<TMappedClass>(string property)
            {
                return IsNull(typeof(TMappedClass), property);
            }

            /// <summary>
            /// Determines whether [is not null] [the specified property].
            /// </summary>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNotNull<TMappedClass>(string property)
            {
                return IsNotNull(typeof(TMappedClass), property);
            }

            /// <summary>
            /// Determines whether [is not null or empty] [the specified property].
            /// </summary>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Filter IsNotNullOrEmpty<TMappedClass>(string property)
            {
                return IsNotNullOrEmpty(typeof(TMappedClass), property);
            }

            /// <summary>
            /// Informations the specified property.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="values">The values.</param>
            /// <returns></returns>
            public static Filter In<TMappedClass>(string property, System.Collections.IList values)
            {
                return In(typeof(TMappedClass), property, values);
            }

            #endregion
        }

        /// <summary>
        /// Order
        /// </summary>
        public class Order
        {
            #region Properties

            /// <summary>
            /// Gets or sets the direction.
            /// </summary>
            /// <value>
            /// The direction.
            /// </value>
            public OrderDirection Direction { get; set; }

            /// <summary>
            /// Gets or sets the attribute.
            /// </summary>
            /// <value>
            /// The attribute.
            /// </value>
            public FieldAttribute Attribute { get; set; }

            /// <summary>
            /// Gets or sets the type of the custom.
            /// </summary>
            /// <value>
            /// The type of the custom.
            /// </value>
            public Type CustomType { get; set; }

            /// <summary>
            /// Gets the format.
            /// </summary>
            public string Format
            {
                get
                {
                    try
                    {
                        if (Direction == OrderDirection.Random)
                        {
                            if (ActiveRecordMaster.DataBaseType == DataBaseType.SQLServer)
                                return "NEWID()";
                            else if (ActiveRecordMaster.DataBaseType == DataBaseType.MySQL)
                                return "RAND()";
                        }

                        ActiveRecordAttribute attr = ActiveRecordMaster.GetAttribute(CustomType);

                        return String.Format("{0} {1}", ActiveRecordMaster.FormatField(attr.FullName, Attribute.Column), Direction == OrderDirection.Asc ? "ASC" : "DESC");
                    }
                    catch { return null; }
                }
            }

            #endregion

            #region Contructors

            /// <summary>
            /// Prevents a default instance of the <see cref="Filter"/> class from being created.
            /// </summary>
            private Order()
            {
            }

            #endregion

            #region Static Methods

            /// <summary>
            /// Creates this instance.
            /// </summary>
            /// <returns></returns>
            public static Order Create()
            {
                return new Order();
            }

            #endregion

            #region Order Methods

            /// <summary>
            /// Orders by random.
            /// </summary>
            /// <returns></returns>
            public static Order Random()
            {
                Order order = Order.Create();

                order.CustomType = null;
                order.Attribute = null;
                order.Direction = OrderDirection.Random;

                return order;
            }

            /// <summary>
            /// Order by Asc
            /// </summary>
            /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Order Asc<TMappedClass>(string property)
            {
                return Asc(typeof(TMappedClass), property);
            }

            /// <summary>
            /// Order by Asc
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Order Asc(Type type, string property)
            {
                Order order = Order.Create();

                order.CustomType = type;
                order.Attribute = FieldAttribute.GetAttribute(type, property);
                order.Direction = OrderDirection.Asc;

                return order;
            }

            /// <summary>
            /// Order by Desc
            /// </summary>
            /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Order Desc<TMappedClass>(string property)
            {
                return Desc(typeof(TMappedClass), property);
            }

            /// <summary>
            /// Order by Desc
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="property">The property.</param>
            /// <returns></returns>
            public static Order Desc(Type type, string property)
            {
                Order order = Order.Create();

                order.CustomType = type;
                order.Attribute = FieldAttribute.GetAttribute(type, property);
                order.Direction = OrderDirection.Desc;

                return order;
            }

            #endregion
        }
    }
}