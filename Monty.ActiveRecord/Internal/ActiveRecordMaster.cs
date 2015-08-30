using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Monty.Log;
using System.Reflection;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// Active Record Master
    /// </summary>
    public class ActiveRecordMaster
    {
        #region Properties

        static string __ConnectionString = null;

        static DataBaseType __DataBaseType = DataBaseType.Undefined;

        #endregion

        #region Contructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ActiveRecordMaster"/> class from being created.
        /// </summary>
        private ActiveRecordMaster()
        {

        }

        #endregion

        #region Configuration

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        internal static string ConnectionString
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(__ConnectionString))
                        __ConnectionString = ConfigurationManager.AppSettings["Monty:ActiveRecord:Connection"];

                    return __ConnectionString;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the type of the data base.
        /// </summary>
        /// <value>
        /// The type of the data base.
        /// </value>
        public static DataBaseType DataBaseType
        {
            get
            {
                try
                {
                    if (__DataBaseType != ActiveRecord.DataBaseType.Undefined)
                        return __DataBaseType;

                    if (ConfigurationManager.AppSettings["Monty:ActiveRecord:Server"].ToLower() == "mysql")
                        return DataBaseType.MySQL;
                    else
                        return DataBaseType.SQLServer;

                }
                catch
                {
                    return DataBaseType.SQLServer;
                }
            }
        }

        /// <summary>
        /// Connections this instance.
        /// </summary>
        /// <returns></returns>
        public static DbConnection Connection()
        {
            if (ConnectionString == null)
                return null;

            DbConnection connection = null;
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    connection = new MySqlConnection(ConnectionString);
                    break;
                case DataBaseType.SQLServer:
                default:
                    connection = new SqlConnection(ConnectionString);
                    break;
            }

            return connection;
        }

        /// <summary>
        /// Commands this instance.
        /// </summary>
        /// <returns></returns>
        public static DbCommand Command()
        {
            DbCommand command = null;
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    command = new MySqlCommand();
                    break;
                case DataBaseType.SQLServer:
                default:
                    command = new SqlCommand();
                    break;
            }

            return command;
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Gets the columns name by ordinal.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnsNameByOrdinal(IDataReader dr)
        {
            Dictionary<string, int> columns = new Dictionary<string, int>();

            if (dr == null)
                return columns;

            for (int i = 0; i < dr.FieldCount; i++)
                columns[dr.GetName(i)] = i;

            return columns;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return __ConnectionString;
        }

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void SetConnectionString(string connectionString)
        {
            __ConnectionString = connectionString;
        }

        /// <summary>
        /// Sets the type of the data base.
        /// </summary>
        /// <param name="dataBaseType">Type of the data base.</param>
        public static void SetDataBaseType(DataBaseType dataBaseType)
        {
            __DataBaseType = dataBaseType;
        }

        /// <summary>
        /// Inserts the identity.
        /// </summary>
        /// <returns></returns>
        public static string InsertIdentity()
        {
            string identity = String.Empty;
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    identity = ";SELECT last_insert_id() AS 'Identity';";
                    break;
                case DataBaseType.SQLServer:
                default:
                    identity = "SELECT @@IDENTITY AS 'Identity'";
                    break;
            }

            return identity;
        }

        /// <summary>
        /// Removes the SQL injection.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RemoveSQLInjection(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            return value.Replace("'", "''");
        }

        /// <summary>
        /// Formats the field.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string FormatField(string table, string field)
        {
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    field = String.Format("`{0}`.`{1}`", table, field);
                    break;
                case DataBaseType.SQLServer:
                default:
                    field = String.Format("[{0}].[{1}]", table, field);
                    break;
            }

            return field;
        }

        /// <summary>
        /// Formats the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string FormatField(string field)
        {
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    field = String.Format("`{0}`", field);
                    break;
                case DataBaseType.SQLServer:
                default:
                    field = String.Format("[{0}]", field);
                    break;
            }

            return field;
        }

        /// <summary>
        /// Formats the table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string FormatTable(string table)
        {
            string field = String.Empty;
            switch (DataBaseType)
            {
                case DataBaseType.MySQL:
                    field = String.Format("`{0}`", table);
                    break;
                case DataBaseType.SQLServer:
                default:
                    field = String.Format("[{0}]", table);
                    break;
            }

            return field;
        }

        /// <summary>
        /// Formats the parameter key.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string FormatParameterKey(string field)
        {
            if (String.IsNullOrEmpty(field))
                return field;

            return String.Format("@{0}{1}", field.ToLower().Replace(" ", String.Empty), Guid.NewGuid().ToString().Replace("-", String.Empty));
        }

        /// <summary>
        /// Formats the parameter key simple.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string FormatParameterKeySimple(string field)
        {
            if (String.IsNullOrEmpty(field))
                return field;

            return String.Format("@{0}", field.Replace(" ", String.Empty));
        }

        #endregion

        #region Create Methods

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public static bool Create(Type type, ActiveRecordBase record)
        {
            //Inner Create
            if (!ActiveRecordMaster.InnerCreate(type, record))
                return false;

            //Cascade Save Belongs To
            foreach (var attr in ActiveRecordMaster.GetBelongsTo(type))
                if (attr.CascadeSave)
                {
                    ActiveRecordBase item = attr.Value(record);

                    if (item != null && !item.IsSaved)
                        item.Save();
                }

            //Cascade Save Has Many Properties
            foreach (var attr in ActiveRecordMaster.GetHasMany(type))
                if (attr.CascadeSave)
                    foreach (ActiveRecordBase item in attr.Values(record))
                        if (!item.IsSaved)
                            item.Save();

            record.IsSaved = true;

            return true;
        }

        /// <summary>
        /// Inners the create.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        internal static bool InnerCreate(Type type, ActiveRecordBase record)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            try
            {
                PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

                if (pk == null)
                    return false;

                Criteria criteria = Criteria.For(type);
                criteria.SQLString = ActiveRecordMaster.MakeInsert(type);

                //PK + Property
                foreach (var item in ActiveRecordMaster.GetFields(type))
                    if (item.IncludeInInsert())
                        criteria.AddParameter(ActiveRecordMaster.FormatParameterKeySimple(item.Column), item.CurrentPropertyInfo.Name, item.GetValue(record));
                //Belongs
                foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                    if (item.IncludeInInsert())
                        criteria.AddParameter(ActiveRecordMaster.FormatParameterKeySimple(item.Column), item.CurrentPropertyInfo.Name, item.GetValue(record));

                //Get the id generated value
                if (pk.Generator == PrimaryKeyType.Identity)
                    record.SetId(ActiveRecordMaster.ExecuteScalarQueryCriteria(type, criteria));
                else
                    ActiveRecordMaster.ExecuteNonQueryCriteria(type, criteria);

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.InnerCreate()", ex);
                return false;
            }
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Updates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public static bool Update(Type type, ActiveRecordBase record)
        {
            //Inner Update
            if (!ActiveRecordMaster.InnerUpdate(type, record))
                return false;

            //Cascade Save Belongs To
            foreach (var attr in ActiveRecordMaster.GetBelongsTo(type))
                if (attr.CascadeSave)
                {
                    ActiveRecordBase item = attr.Value(record);

                    if (item != null && !item.IsSaved)
                        item.Save();
                }

            //Cascade Save Has Many Properties
            foreach (var attr in ActiveRecordMaster.GetHasMany(type))
                if (attr.CascadeSave)
                    foreach (ActiveRecordBase item in attr.Values(record))
                        if (!item.IsSaved)
                            item.Save();

            record.IsSaved = true;

            return true;
        }

        /// <summary>
        /// Inners the update.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        private static bool InnerUpdate(Type type, ActiveRecordBase record)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            try
            {
                PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

                if (pk == null)
                    return false;

                Criteria criteria = Criteria.For(type);
                criteria.SQLString = ActiveRecordMaster.MakeUpdate(type);

                //Where Parameter
                criteria.AddParameter("@CODE", pk.CurrentPropertyInfo.Name, record.GetId());

                //Property
                foreach (var item in ActiveRecordMaster.GetFields(type))
                    if (item.IncludeInInsert())
                        criteria.AddParameter(ActiveRecordMaster.FormatParameterKeySimple(item.Column), item.CurrentPropertyInfo.Name, item.GetValue(record));
                //Belongs
                foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                    if (item.IncludeInInsert())
                        criteria.AddParameter(ActiveRecordMaster.FormatParameterKeySimple(item.Column), item.CurrentPropertyInfo.Name, item.GetValue(record));

                //Execute Query
                return ActiveRecordMaster.ExecuteNonQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.InnerUpdate()", ex);
                return false;
            }
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Deletes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public static bool Delete(Type type, ActiveRecordBase record)
        {
            //Cascade Delete Has Many Properties
            foreach (var attr in ActiveRecordMaster.GetHasMany(type))
                if (attr.CascadeDelete)
                    foreach (ActiveRecordBase item in attr.Values(record))
                        item.Delete();

            //Inner Delete
            if (!ActiveRecordMaster.InnerDelete(type, record))
                return false;

            //Belongs To
            foreach (var attr in ActiveRecordMaster.GetBelongsTo(type))
                if (attr.CascadeDelete)
                {
                    ActiveRecordBase item = attr.Value(record);

                    if (item != null)
                        item.Delete();
                }

            return true;
        }

        /// <summary>
        /// Inners the delete.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        private static bool InnerDelete(Type type, ActiveRecordBase record)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            try
            {
                PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

                if (pk == null)
                    return false;

                Criteria criteria = Criteria.For(type);
                criteria.SQLString = ActiveRecordMaster.MakeDelete(type);

                criteria.AddParameter("@CODE", pk.CurrentPropertyInfo.Name, pk.CurrentPropertyInfo.GetValue(record, null));

                return ActiveRecordMaster.ExecuteNonQueryCriteria(type, criteria);
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordBase.InnerDelete()", ex);
                return false;
            }
        }

        #endregion

        #region SQL Methods

        /// <summary>
        /// Makes the has many select.
        /// </summary>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="keyProperty">The key property.</param>
        /// <returns></returns>
        internal static string MakeHasManySelect(Type keyType, string keyProperty)
        {
            //Get the property
            HasManyAttribute many = ActiveRecordMaster.GetHasMany(keyType, keyProperty);

            if (many == null || many.MapType == null)
                return null;

            //Get the mappedType
            ActiveRecordAttribute mappedTypeAttribute = ActiveRecordMaster.GetAttribute(many.MapType);

            if (mappedTypeAttribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            //Fields
            sql.Append(MakeSelectFields(many.MapType));

            sql.AppendFormat("FROM {0} ", ActiveRecordMaster.MakeSelectFrom(many.MapType));
            sql.AppendFormat("WHERE {0} = @CODE ", ActiveRecordMaster.FormatField(many.Column));

            //Default Where
            if (!String.IsNullOrEmpty(mappedTypeAttribute.Where))
                sql.AppendFormat("AND ({0})", mappedTypeAttribute.Where);

            //Relation Where
            if (!String.IsNullOrEmpty(many.Where))
                sql.AppendFormat("AND ({0})", many.Where);

            //Relation Order By
            if (!String.IsNullOrEmpty(many.OrderBy))
                sql.AppendFormat(" ORDER BY {0}", many.OrderBy);

            return sql.ToString();
        }

        /// <summary>
        /// Makes the primary key select.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakePrimaryKeySelect(Type type)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            if (pk == null)
                return null;

            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            //Fields
            sql.Append(MakeSelectFields(type));

            sql.AppendFormat(" FROM {0} ", ActiveRecordMaster.MakeSelectFrom(type));
            sql.AppendFormat(" WHERE {0} = @CODE ", ActiveRecordMaster.FormatField(attribute.FullName, pk.Column));

            if (!String.IsNullOrEmpty(attribute.Where))
                sql.AppendFormat(" AND ({0})", attribute.Where);

            return sql.ToString();
        }

        /// <summary>
        /// Makes the count select.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeCountSelect(Type type)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            if (pk == null)
                return null;

            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT COUNT(*) ");
            sql.AppendFormat("FROM {0} ", ActiveRecordMaster.MakeSelectFrom(type));
            sql.Append("WHERE 1 = 1 ");

            if (!String.IsNullOrEmpty(attribute.Where))
                sql.AppendFormat("AND ({0})", attribute.Where);

            return sql.ToString();
        }
        
        /// <summary>
        /// Makes the full select.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string MakeFullSelect(Type type)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            //Fields
            sql.Append(MakeSelectFields(type));
            //From
            sql.AppendFormat(" FROM {0} ", ActiveRecordMaster.MakeSelectFrom(type));

            sql.AppendFormat(" WHERE 1 = 1 ");
            if (!String.IsNullOrEmpty(attribute.Where))
                sql.AppendFormat(" AND ({0})", attribute.Where);

            return sql.ToString();
        }

        /// <summary>
        /// Makes the projection full select.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string MakeProjectionFullSelect(Type type, string field)
        {
            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            //Fields
            sql.Append(MakeSelectField(type, field));

            //From
            sql.AppendFormat(" FROM {0} ", ActiveRecordMaster.MakeSelectFrom(type));

            sql.AppendFormat(" WHERE 1 = 1 ");
            if (!String.IsNullOrEmpty(attribute.Where))
                sql.AppendFormat(" AND ({0})", attribute.Where);

            return sql.ToString().Trim();
        }

        /// <summary>
        /// Makes the select fields.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeSelectFields(Type type)
        {
            ActiveRecordAttribute attr = ActiveRecordMaster.GetAttribute(type);

            StringBuilder sql = new StringBuilder();

            //PK's And Properties
            foreach (var item in ActiveRecordMaster.GetFields(type))
                sql.AppendFormat("{0},", ActiveRecordMaster.FormatField(attr.FullName, item.Column));

            //Belongs And Many
            foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                sql.AppendFormat("{0},", ActiveRecordMaster.FormatField(attr.FullName, item.Column));

            //Parent
            if (type.BaseType != null && ActiveRecordMaster.GetAttribute(type.BaseType) != null)
                sql.Append(ActiveRecordMaster.MakeSelectFields(type.BaseType));

            return sql.ToString().EndsWith(",") ? sql.ToString().Substring(0, sql.ToString().Length - 1) : sql.ToString();
        }

        /// <summary>
        /// Makes the select field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        internal static string MakeSelectField(Type type, string field)
        {
            FieldAttribute item = ActiveRecordMaster.GetField(type, field);

            ActiveRecordAttribute attr = ActiveRecordMaster.GetAttribute(ActiveRecordMaster.GetFieldOwner(type, field));

            StringBuilder sql = new StringBuilder();

            if (item != null)
                sql.AppendFormat("{0}", ActiveRecordMaster.FormatField(attr.FullName, item.Column));
            else
                return null;


            return sql.ToString();
        }

        /// <summary>
        /// Makes the select from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeSelectFrom(Type type)
        {
            ActiveRecordAttribute table = ActiveRecordMaster.GetAttribute(type);

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(" {0} ", ActiveRecordMaster.FormatField(table.FullName));

            //Join
            if (ActiveRecordMaster.HasJoinedBase(type) != null)
                sql.Append(MakeSelectFromRecursive(type));
            /*
            {
                ActiveRecordAttribute joinTable = ActiveRecordMaster.GetAttribute(type.BaseType);

                if (joinTable != null)
                    sql.AppendFormat(" LEFT JOIN {0} ON {1}.{2} = {0}.{3} ", ActiveRecordMaster.FormatField(joinTable.FullName), ActiveRecordMaster.FormatField(table.FullName), ActiveRecordMaster.GetPrimaryKey(type).Column, ActiveRecordMaster.GetPrimaryKey(type.BaseType).Column);
            }
            */

            return sql.ToString();
        }

        /// <summary>
        /// Makes the select from recursive.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeSelectFromRecursive(Type type)
        {
            try
            {
                ActiveRecordAttribute table = ActiveRecordMaster.GetAttribute(type);

                StringBuilder sql = new StringBuilder();

                ActiveRecordAttribute joinTable = ActiveRecordMaster.GetAttribute(type.BaseType);

                if (joinTable != null)
                    sql.AppendFormat(" LEFT JOIN {0} ON {1}.{2} = {0}.{3} ", ActiveRecordMaster.FormatField(joinTable.FullName), ActiveRecordMaster.FormatField(table.FullName), ActiveRecordMaster.GetPrimaryKey(type).Column, ActiveRecordMaster.GetPrimaryKey(type.BaseType).Column);

                //Join
                if (ActiveRecordMaster.HasJoinedBase(type.BaseType) != null)
                    sql.Append(MakeSelectFromRecursive(type.BaseType));

                return sql.ToString();
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute MakeSelectFromRecursive(type: [{0}]);", type.Name), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Makes the delete.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeDelete(Type type)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            if (pk == null)
                return null;

            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("DELETE FROM {0} ", ActiveRecordMaster.FormatField(attribute.FullName));
            sql.AppendFormat("WHERE {0} = @CODE ", ActiveRecordMaster.FormatField(pk.Column));

            return sql.ToString();
        }

        /// <summary>
        /// Makes the insert.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeInsert(Type type)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            if (pk == null)
                return null;

            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("INSERT INTO {0} ", ActiveRecordMaster.FormatField(attribute.FullName));

            string fields = String.Empty;
            //PK + Property
            foreach (var item in ActiveRecordMaster.GetFields(type))
                if (item.IncludeInInsert())
                    fields = String.Format("{0}{1},", fields, ActiveRecordMaster.FormatField(item.Column));
            //Belongs
            foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                if (item.IncludeInInsert())
                    fields = String.Format("{0}{1},", fields, ActiveRecordMaster.FormatField(item.Column));
            //Fields
            sql.AppendFormat(" ({0}) ", fields.Substring(0, fields.Length - 1));

            string values = String.Empty;
            //PK + Property
            foreach (var item in ActiveRecordMaster.GetFields(type))
                if (item.IncludeInInsert())
                    values = String.Format("{0}{1},", values, ActiveRecordMaster.FormatParameterKeySimple(item.Column));
            //Belongs
            foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                if (item.IncludeInInsert())
                    values = String.Format("{0}{1},", values, ActiveRecordMaster.FormatParameterKeySimple(item.Column));
            //Values
            sql.AppendFormat(" VALUES ({0}) {1}", values.Substring(0, values.Length - 1), ActiveRecordMaster.InsertIdentity());

            return sql.ToString();
        }

        /// <summary>
        /// Makes the update.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string MakeUpdate(Type type)
        {
            PrimaryKeyAttribute pk = ActiveRecordMaster.GetPrimaryKey(type);

            if (pk == null)
                return null;

            ActiveRecordAttribute attribute = ActiveRecordMaster.GetAttribute(type);

            if (attribute == null)
                throw new AttributeNotFoundException();

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("UPDATE {0} SET ", ActiveRecordMaster.FormatField(attribute.FullName));

            string fields = String.Empty;
            //Property
            foreach (var item in ActiveRecordMaster.GetFields(type))
                if (item.IncludeInInsert())
                    fields = String.Format("{0}{1} = {2} ,", fields, ActiveRecordMaster.FormatField(item.Column), ActiveRecordMaster.FormatParameterKeySimple(item.Column));
            //Belongs
            foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                if (item.IncludeInInsert())
                    fields = String.Format("{0}{1} = {2} ,", fields, ActiveRecordMaster.FormatField(item.Column), ActiveRecordMaster.FormatParameterKeySimple(item.Column));
            //Fields
            sql.AppendFormat(" {0} ", fields.Substring(0, fields.Length - 1));

            //Update Condition
            sql.AppendFormat("WHERE {0} = @CODE ", ActiveRecordMaster.FormatField(pk.Column));

            return sql.ToString();
        }

        #endregion

        #region Attribute Methods

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ActiveRecordAttribute GetAttribute(Type type)
        {
            object[] attr = type.GetCustomAttributes(typeof(ActiveRecordAttribute), true);

            if (attr != null && attr.Count() == 1)
                return (ActiveRecordAttribute)attr.First();

            return null;
        }

        /// <summary>
        /// Determines whether [has joined base] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static JoinedBaseAttribute HasJoinedBase(Type type)
        {
            object[] attr = type.GetCustomAttributes(typeof(JoinedBaseAttribute), true);

            if (attr != null && attr.Count() == 1)
                return (JoinedBaseAttribute)attr.First();

            return null;
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static IEnumerable<FieldAttribute> GetFields(Type type)
        {
            foreach (var item in type.GetProperties())
                if (item.DeclaringType == type)
                {
                    object[] attr = item.GetCustomAttributes(typeof(PropertyAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        yield return ((PropertyAttribute)attr.First()).AttributeWithProperty<PropertyAttribute>(item);
                }

            PrimaryKeyAttribute pk = GetPrimaryKey(type);

            if (pk != null)
                yield return pk;
        }

        /// <summary>
        /// Gets the has many.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static IEnumerable<HasManyAttribute> GetHasMany(Type type)
        {
            foreach (var item in type.GetProperties())
                if (item.DeclaringType == type)
                {
                    object[] attr = item.GetCustomAttributes(typeof(HasManyAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        yield return ((HasManyAttribute)attr.First()).AttributeWithProperty<HasManyAttribute>(item);
                }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static IEnumerable<PropertyAttribute> GetProperties(Type type)
        {
            foreach (var item in type.GetProperties())
                if (item.DeclaringType == type)
                {
                    object[] attr = item.GetCustomAttributes(typeof(PropertyAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        yield return ((PropertyAttribute)attr.First()).AttributeWithProperty<PropertyAttribute>(item);
                }
        }

        /// <summary>
        /// Gets the relation properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static IEnumerable<RelationAttribute> GetRelationProperties(Type type)
        {
            foreach (var item in ActiveRecordMaster.GetHasMany(type))
                yield return item;

            foreach (var item in ActiveRecordMaster.GetBelongsTo(type))
                yield return item;
        }

        /// <summary>
        /// Gets the belongs to.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static IEnumerable<BelongsToAttribute> GetBelongsTo(Type type)
        {
            foreach (var item in type.GetProperties())
                if (item.DeclaringType == type)
                {
                    object[] attr = item.GetCustomAttributes(typeof(BelongsToAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        yield return ((BelongsToAttribute)attr.First()).AttributeWithProperty<BelongsToAttribute>(item);
                }
        }

        /// <summary>
        /// Gets the primary key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static PrimaryKeyAttribute GetPrimaryKey(Type type)
        {
            foreach (var item in type.GetProperties())
                if (item.DeclaringType == type)
                {
                    object[] attr = item.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                    if (attr != null && attr.Count() == 1)
                        return ((PrimaryKeyAttribute)attr.First()).AttributeWithProperty<PrimaryKeyAttribute>(item);
                }

            return null;
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static FieldAttribute GetField(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                object[] attr = item.GetCustomAttributes(typeof(FieldAttribute), true);
                if (attr != null && attr.Count() == 1)
                    return ((FieldAttribute)attr.First()).AttributeWithProperty<FieldAttribute>(item);

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetField", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the field owner.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static Type GetFieldOwner(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                return item.DeclaringType;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetFieldOwner", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static PropertyAttribute GetProperty(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                object[] attr = item.GetCustomAttributes(typeof(PropertyAttribute), true);
                if (attr != null && attr.Count() == 1)
                    return ((PropertyAttribute)attr.First()).AttributeWithProperty<PropertyAttribute>(item);

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetProperty", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the belongs to.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static BelongsToAttribute GetBelongsTo(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                object[] attr = item.GetCustomAttributes(typeof(BelongsToAttribute), true);
                if (attr != null && attr.Count() == 1)
                    return ((BelongsToAttribute)attr.First()).AttributeWithProperty<BelongsToAttribute>(item);

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetBelongsTo", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the has many.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static HasManyAttribute GetHasMany(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                object[] attr = item.GetCustomAttributes(typeof(HasManyAttribute), true);
                if (attr != null && attr.Count() == 1)
                    return ((HasManyAttribute)attr.First()).AttributeWithProperty<HasManyAttribute>(item);

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetBelongsTo", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the primary key property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static PrimaryKeyAttribute GetPrimaryKeyProperty(Type type, string property)
        {
            try
            {
                PropertyInfo item = type.GetProperty(property);

                object[] attr = item.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (attr != null && attr.Count() == 1)
                    return ((PrimaryKeyAttribute)attr.First()).AttributeWithProperty<PrimaryKeyAttribute>(item);

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, "Error when tries to execute ActiveRecordMaster.GetPrimaryKeyProperty", ex);
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(String sql, params DbParameter[] parameters)
        {
            List<ActiveRecordBase> items = new List<ActiveRecordBase>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = ActiveRecordMaster.Command();
                command.Connection = connection;
                command.CommandText = sql;

                //Parameters
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteNonQuery(sql: [{0}], parameters: [{1}])", sql, parameters), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the query criteria.
        /// </summary>
        /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static List<TMappedClass> ExecuteQueryCriteria<TMappedClass>(Criteria criteria)
            where TMappedClass : ActiveRecordBase
        {
            List<TMappedClass> items = new List<TMappedClass>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = criteria != null ? criteria.Command : ActiveRecordMaster.Command();
                command.Connection = connection;

                //Parameters
                if (criteria != null)
                    foreach (var parameter in criteria.Parameters)
                        if (!command.Parameters.Contains(parameter.ParameterName))
                            command.Parameters.Add(parameter);

                //Execute Query
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dataReader.Read())
                    {
                        TMappedClass item = CreateInstance(typeof(TMappedClass), dataReader) as TMappedClass;

                        if (item != null)
                            items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteQueryCriteria(type: [{0}], criteria: [{1}]) \n\n SQL: {2}\n\n", typeof(TMappedClass), criteria, criteria.SQLString), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the query criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static List<ActiveRecordBase> ExecuteQueryCriteria(Type type, Criteria criteria)
        {
            List<ActiveRecordBase> items = new List<ActiveRecordBase>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = criteria != null ? criteria.Command : ActiveRecordMaster.Command();
                command.Connection = connection;

                string log = String.Empty;

                //Parameters
                if (criteria != null)
                    foreach (var parameter in criteria.Parameters)
                        if (!command.Parameters.Contains(parameter.ParameterName))
                            command.Parameters.Add(parameter);

                //Execute Query
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dataReader.Read())
                    {
                        ActiveRecordBase item = CreateInstance(type, dataReader);

                        if (item != null)
                            items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteQueryCriteria(type: [{0}], criteria: [{1}]) \n\n SQL: {2}\n\n", type, criteria, criteria.SQLString), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the non query criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static bool ExecuteNonQueryCriteria(Type type, Criteria criteria)
        {
            List<ActiveRecordBase> items = new List<ActiveRecordBase>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = criteria != null ? criteria.Command : ActiveRecordMaster.Command();
                command.Connection = connection;

                //Parameters
                if (criteria != null)
                    foreach (var parameter in criteria.Parameters)
                        if (!command.Parameters.Contains(parameter.ParameterName))
                            command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteNonQueryCriteria(type: [{0}], criteria: [{1}])", type, criteria), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the scalar query criteria.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static object ExecuteScalarQueryCriteria(Type type, Criteria criteria)
        {
            List<ActiveRecordBase> items = new List<ActiveRecordBase>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = criteria != null ? criteria.Command : ActiveRecordMaster.Command();
                command.Connection = connection;

                //Parameters
                if (criteria != null)
                    foreach (var parameter in criteria.Parameters)
                        if (!command.Parameters.Contains(parameter.ParameterName))
                            command.Parameters.Add(parameter);

                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteNonQueryCriteria(type: [{0}], criteria: [{1}])", type, criteria), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the projection query criteria.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static List<TProjectionType> ExecuteProjectionQueryCriteria<TProjectionType>(Criteria criteria)
        {
            List<TProjectionType> items = new List<TProjectionType>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = criteria != null ? criteria.Command : ActiveRecordMaster.Command();
                command.Connection = connection;

                string log = String.Empty;

                //Parameters
                if (criteria != null)
                    foreach (var parameter in criteria.Parameters)
                        if (!command.Parameters.Contains(parameter.ParameterName))
                            command.Parameters.Add(parameter);

                //Execute Query
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                            items.Add((TProjectionType)dataReader.GetValue(0));
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteProjectionQueryCriteria(criteria: [{0}])", criteria), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the projection query criteria.
        /// </summary>
        /// <typeparam name="TProjectionType">The type of the projection type.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public static List<TProjectionType> ExecuteProjectionQueryCriteria<TProjectionType>(string sql)
        {
            List<TProjectionType> items = new List<TProjectionType>();

            DbConnection connection = ActiveRecordMaster.Connection();
            try
            {
                //Opens connection
                connection.Open();

                DbCommand command = ActiveRecordMaster.Command();
                command.Connection = connection;
                command.CommandText = sql;

                //Execute Query
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                            items.Add((TProjectionType)dataReader.GetValue(0));
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error when tries to execute ActiveRecordBase.ExecuteProjectionQueryCriteria(sql: [{0}])", sql), ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        internal static ActiveRecordBase CreateInstance(Type type, IDataReader item)
        {
            ActiveRecordBase record = (ActiveRecordBase)Activator.CreateInstance(type, true);

            //Update
            ActiveRecordMaster.UpdateInstance(ref record, type, item);

            //Cascade Find
            if (ActiveRecordMaster.HasJoinedBase(type) != null)
                ActiveRecordMaster.UpdateInstance(ref record, type.BaseType, item);

            return record;
        }

        /// <summary>
        /// Updates the instance.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        internal static void UpdateInstance(ref ActiveRecordBase record, Type type, IDataReader item)
        {

            Dictionary<string, int> columns = ActiveRecordMaster.GetColumnsNameByOrdinal(item);

            //PK + Properties
            foreach (var property in ActiveRecordMaster.GetFields(type))
            {
                PropertyInfo info = property.CurrentPropertyInfo;
                try
                {
                    string key = property.Column;
                    if (!columns.ContainsKey(key))
                        key = String.Format("{0}{1}", ActiveRecordMaster.GetAttribute(info.DeclaringType).FullName, property.Column);

                    if (item.IsDBNull(columns[key]))
                        info.SetValue(record, null, null);
                    else
                        if (info.PropertyType == typeof(string))
                            info.SetValue(record, item.GetString(columns[key]), null);
                        else if (info.PropertyType == typeof(Single))
                            info.SetValue(record, Convert.ToSingle(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(float))
                            info.SetValue(record, (float)Convert.ToSingle(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(double))
                            info.SetValue(record, Convert.ToDouble(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(decimal))
                            info.SetValue(record, Convert.ToDecimal(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(Int32))
                            info.SetValue(record, Convert.ToInt32(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(Int64))
                            info.SetValue(record, Convert.ToInt64(item.GetValue(columns[key])), null);
                        else if (info.PropertyType == typeof(bool))
                            info.SetValue(record, item.GetBoolean(columns[key]), null);
                        else if (info.PropertyType == typeof(DateTime))
                            info.SetValue(record, item.GetDateTime(columns[key]), null);
                        else if (info.PropertyType == typeof(DateTime?))
                            info.SetValue(record, item.GetDateTime(columns[key]), null);
                        else if (info.PropertyType.IsEnum)
                            info.SetValue(record, item.GetInt32(columns[key]), null);
                        else if (IsNullableEnumType(info.PropertyType))//Nullable Enum
                            info.SetValue(record, Enum.ToObject(info.PropertyType.GetGenericArguments()[0], item.GetInt32(columns[key])), null);
                        else if (IsNullable(info.PropertyType))//Nullable
                            SetNullableTypeValue(ref record, ref item, ref info, columns[key]);
                        else
                            LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Could not update the property {0}.{1}", type.Name, property.CurrentPropertyInfo.Name));
                }
                catch (Exception ex)
                {
                    LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error on ActiveRecordBase.CreateAndFillInstance when tries to set a value to the property {0}", property.CurrentPropertyInfo.Name), ex);
                }
            }

            //Relations
            if (record.LazyObjects == null)
                record.LazyObjects = new Dictionary<String, Object>();

            //Save BelongsTo Lazy
            foreach (var property in ActiveRecordMaster.GetBelongsTo(type))
            {
                try
                {
                    PropertyInfo info = property.CurrentPropertyInfo;

                    string key = property.Column;
                    if (!columns.ContainsKey(key))
                        key = String.Format("{0}{1}", ActiveRecordMaster.GetAttribute(info.DeclaringType).FullName, property.Column);

                    record.LazyObjects[property.CurrentPropertyInfo.Name] = item.GetValue(columns[key]);
                }
                catch (Exception ex)
                {
                    LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Error on ActiveRecordBase.CreateAndFillInstance when tries to set a value to the property {0}", property.CurrentPropertyInfo.Name), ex);
                }
            }
        }

        /// <summary>
        /// Sets the nullable type value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="item">The item.</param>
        /// <param name="info">The information.</param>
        /// <param name="column">The column.</param>
        internal static void SetNullableTypeValue(ref ActiveRecordBase record, ref IDataReader item, ref PropertyInfo info, int column)
        {
            Type argumentType = info.PropertyType.GetGenericArguments()[0];

            if (argumentType == typeof(Single))
                info.SetValue(record, new Nullable<Single>(Convert.ToSingle(item.GetValue(column))), null);
            else if (argumentType == typeof(float))
                info.SetValue(record, new Nullable<float>((float)Convert.ToSingle(item.GetValue(column))), null);
            else if (argumentType == typeof(double))
                info.SetValue(record, new Nullable<double>(Convert.ToDouble(item.GetValue(column))), null);
            else if (argumentType == typeof(decimal))
                info.SetValue(record, new Nullable<decimal>(Convert.ToDecimal(item.GetValue(column))), null);
            else if (argumentType == typeof(Int32))
                info.SetValue(record, new Nullable<Int32>(Convert.ToInt32(item.GetValue(column))), null);
            else if (argumentType == typeof(Int64))
                info.SetValue(record, new Nullable<Int64>(Convert.ToInt64(item.GetValue(column))), null);
            else if (argumentType == typeof(bool))
                info.SetValue(record, new Nullable<bool>(item.GetBoolean(column)), null);
            else if (argumentType == typeof(DateTime))
                info.SetValue(record, new Nullable<DateTime>(item.GetDateTime(column)), null);
            else if (argumentType.IsEnum)
                info.SetValue(record, new Nullable<Single>(item.GetInt32(column)), null);
            else
                LogManager.Log("Monty.ActiveRecord", LogCategory.Error, String.Format("Could not update the nullable property {0}.{1}", info.DeclaringType.Name, info.Name));
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static bool IsNullable(Type type)
        {
            try
            {
                return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
            }
            catch { return false; }
        }

        /// <summary>
        /// Determines whether [is nullable enum type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static bool IsNullableEnumType(Type type)
        {
            try
            {
                return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && type.GetGenericArguments()[0].IsEnum;
            }
            catch { return false; }
        }

        #endregion
    }
}