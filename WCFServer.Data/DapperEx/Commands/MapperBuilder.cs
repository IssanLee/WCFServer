using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WCFServer.Data.DapperEx.Attributies;

namespace WCFServer.Data.DapperEx.Commands
{
    /// <summary>
    /// 映射生成脚本
    /// </summary>
    public class MapperBuilder
    {
        private static readonly object objLock = new object();

        /// <summary>
        /// 动态类缓存
        /// </summary>
        protected static Dictionary<string, Object> DynamicCache = new Dictionary<string, Object>();

        /// <summary>
        /// 参数前缀
        /// </summary>
        public string ParameterPrefix { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string DbTableName { get; set; }

        /// <summary>
        /// 生成的SQL脚本
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 属性map
        /// </summary>
        public Dictionary<String, PropertyEx> PropertiesMap { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="parameterPrefix"></param>
        public MapperBuilder(string parameterPrefix) 
        {
            ParameterPrefix = parameterPrefix;
        }

        /// <summary>
        /// 脚本生成器
        /// </summary>
        /// <typeparam name="T">entities泛型</typeparam>
        /// <param name="cmdName">sql键名</param>
        /// <returns>MapperBuilder</returns>
        public MapperBuilder Build<T>(string cmdName) where T : class
        {
            this.DbTableName = GetDbTableName<T>();
            switch (cmdName)
            {
                case "Insert":
                    this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = BuildInsertSql(DbTableName, PropertiesMap, ParameterPrefix);
                    break;
                case "UpdateByKey":
                    this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = BuildUpdateSql(DbTableName, PropertiesMap, ParameterPrefix);
                    break;
                case "DeleteByKey":
                    this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = BuildDeleteSql(DbTableName, PropertiesMap, ParameterPrefix);
                    break;
                case "Delete":
                    //this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = string.Format(@"DELETE FROM {0} WHERE 1=1 #WHERE#", DbTableName);
                    break;
                case "UpdateByExpr":
                    //this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = string.Format(@"UPDATE {0} SET #SET# WHERE 1=1 #WHERE#", DbTableName);
                    break;
                case "Select":
                    this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = BuildSelectSql(DbTableName, PropertiesMap, ParameterPrefix);
                    break;
                case "Count":
                    //this.PropertiesMap = GetPropertiesMap<T>();
                    Sql = string.Format(@"SELECT COUNT(1) FROM {0} WHERE 1=1 #WHERE#", DbTableName);
                    break;

                default:
                    break;
            }
            return this;
        }

        
        /// <summary>
        /// 获取DB表名
        /// </summary>
        /// <typeparam name="T">Entities泛型</typeparam>
        /// <returns></returns>
        public static string GetDbTableName<T>() where T : class
        {
            var t = typeof(T);
            string DbName = t.Name;
            if (GetCache<string>(t.FullName, ref DbName))
                return DbName;
            else
            {
                lock (objLock)
                {
                    var attr = t.GetCustomAttributes(typeof(BaseAttribute), true).FirstOrDefault();
                    if (attr is TableAttribute && (!string.IsNullOrEmpty((attr as BaseAttribute).Name)))
                        DbName = (attr as BaseAttribute).Name;
                    SetCache(t.FullName, DbName);
                }
                return DbName;
            }
        }

        /// <summary>
        /// 参数映射生成
        /// </summary>
        /// <typeparam name="T">entities泛型</typeparam>
        /// <param name="Include"></param>
        /// <returns></returns>
        public static Dictionary<String, PropertyEx> GetPropertiesMap<T>(bool Include = false) where T : class
        {
            var entityMap = new Dictionary<String, PropertyEx>();
            var t = typeof(T);

            if (Include)
            {
                if (GetCache<Dictionary<String, PropertyEx>>(t.FullName + "_PropertyInclude", ref entityMap))
                    return entityMap;
                else
                {
                    lock (objLock)
                    {
                        foreach (var propertyInfo in t.GetProperties().Where(x => !x.GetMethod.IsVirtual))
                        {
                            Object colAttr = null;
                            bool isKey = false;
                            foreach (var attr in propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true))
                            {
                                if (attr is IgnoreAttribute)
                                {
                                    colAttr = null;
                                    continue;
                                }
                                else if (attr is ColumnAttribute)
                                    colAttr = attr;
                                else if (attr is KeyAttribute)
                                    isKey = true;
                            }
                            string name = (colAttr == null || string.IsNullOrEmpty((colAttr as BaseAttribute).Name)) ? propertyInfo.Name : (colAttr as BaseAttribute).Name;
                            entityMap.Add(propertyInfo.Name, new PropertyEx() { DbName = name, Name = propertyInfo.Name, IsKey = isKey, IsVirtual = false });
                        }

                        bool noVirtualFlag = false;
                        foreach (var propertyInfo in t.GetProperties().Where(x => x.GetMethod.IsVirtual))
                        {
                            noVirtualFlag = true;
                            Object colAttr = null, forcolAttr = null;
                            foreach (var attr in propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true))
                            {
                                if (attr is IgnoreAttribute)
                                {
                                    colAttr = null;
                                    continue;
                                }
                                else if (attr is ColumnAttribute)
                                    colAttr = attr;
                                else if (attr is ForeignAttribute)
                                    forcolAttr = attr;
                            }

                            string name = (colAttr == null || string.IsNullOrEmpty((colAttr as BaseAttribute).Name)) ? propertyInfo.Name : (colAttr as BaseAttribute).Name;
                            //db字段名称，db关联字段名称，模型关联字段名称
                            List<String> dbForeignKey = new List<String>(), dbOnKey = new List<String>(), colOnKey = new List<String>();
                            var tableClass = (forcolAttr as ForeignAttribute).Table;

                            foreach (var item in (forcolAttr as ForeignAttribute).Key)
                            {
                                string newitem = item;
                                //支持直接定义关联字段
                                var items = item.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                                if (items.Count() > 1)//有自定义关联字段
                                {
                                    newitem = items[0];
                                    colOnKey.Add(items[1]);
                                }
                                else//无自定义关联字段
                                    colOnKey.Add("");

                                PropertyInfo p = t.GetProperties().FirstOrDefault(x => x.Name == newitem);
                                if (p == null)
                                    dbForeignKey.Add(newitem);//@
                                else
                                    dbForeignKey.Add(p.GetCustomAttribute<ColumnAttribute>(true).Name);
                            }

                            var dbForeignTable = tableClass.GetCustomAttribute<TableAttribute>(true).Name;
                            var keyColumn = tableClass.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>(true) != null);

                            int pass = 0;
                            foreach (var item in dbForeignKey)
                            {
                                PropertyInfo ca = null;

                                //先判断有没有自定义关联键
                                string colOnName = colOnKey[dbForeignKey.IndexOf(item)];
                                if (!string.IsNullOrEmpty(colOnName))
                                {
                                    ca = tableClass.GetProperties().FirstOrDefault(x => x.Name == colOnName);
                                    if (ca != null)
                                    {
                                        dbOnKey.Add(ca.GetCustomAttribute<ColumnAttribute>(true).Name);
                                        pass++;
                                        continue;
                                    }
                                }

                                //没有关联键的情况下用字段名来判断是否存在关联键
                                ca = keyColumn.FirstOrDefault(x => item.IndexOf(x.GetCustomAttribute<ColumnAttribute>(true).Name) >= 0);
                                if (ca == null)
                                {
                                    var p = keyColumn.Where(x => !dbForeignKey.Contains(x.GetCustomAttribute<ColumnAttribute>(true).Name));
                                    if (p.Count() > 1 && p.Count() - pass > 1)
                                        throw new Exception("实体类中存在有多个不确定外键！");    //KnownException
                                    else
                                        dbOnKey.Add(p.Where(x => !dbOnKey.Contains(x.GetCustomAttribute<ColumnAttribute>(true).Name)).FirstOrDefault().GetCustomAttribute<ColumnAttribute>(true).Name);
                                }
                                else
                                {
                                    dbOnKey.Add(ca.GetCustomAttribute<ColumnAttribute>(true).Name);
                                    pass++;
                                }
                            }
                            entityMap.Add(propertyInfo.Name, new PropertyEx() { DbName = name, Name = propertyInfo.Name, IsKey = false, IsVirtual = true, DbForeignTable = dbForeignTable, DbForeignKey = dbForeignKey.ToArray(), DbOnKey = dbOnKey.ToArray() });
                        }
                        if (!noVirtualFlag) throw new Exception("实体类中不存在关联键！");     //KnownException

                        SetCache(t.FullName + "_Property", entityMap);
                        return entityMap;
                    }
                }
            }
            else
            {
                if (GetCache<Dictionary<String, PropertyEx>>(t.FullName + "_Property", ref entityMap))
                    return entityMap;
                else
                {
                    lock (objLock)
                    {
                        foreach (var propertyInfo in t.GetProperties().Where(x => !x.GetMethod.IsVirtual))
                        {
                            Object colAttr = null;
                            bool isKey = false;
                            foreach (var attr in propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true))
                            {
                                if (attr is IgnoreAttribute)
                                {
                                    colAttr = null;
                                    continue;
                                }
                                else if (attr is ColumnAttribute)
                                    colAttr = attr;
                                else if (attr is KeyAttribute)
                                    isKey = true;
                            }
                            string name = (colAttr == null || string.IsNullOrEmpty((colAttr as BaseAttribute).Name)) ? propertyInfo.Name : (colAttr as BaseAttribute).Name;
                            entityMap.Add(propertyInfo.Name, new PropertyEx() { DbName = name, Name = propertyInfo.Name, IsKey = isKey });
                        }
                        SetCache(t.FullName + "_Property", entityMap);
                        return entityMap;
                    }
                }
            }
        }

        #region 脚本生成器

        /// <summary>
        /// 插入SQL脚本生成器
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="properties">参数</param>
        /// <param name="ParamPrefix">参数前缀</param>
        /// <returns></returns>
        private static string BuildInsertSql(string tbName, Dictionary<String, PropertyEx> properties, string ParamPrefix)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("INSERT INTO {0} (", tbName);
            bool _1stIndex = true;  // 第一次拼接标志位
            foreach (var item in properties)
            {
                if (_1stIndex)
                {
                    sql.Append(item.Value.DbName);
                    _1stIndex = false;
                }
                else sql.AppendFormat(",{0}", item.Value.DbName);
            }
            sql.Append(")");
            sql.Append(" VALUES(");

            _1stIndex = true;       // 第一次拼接标志位
            foreach (var item in properties)
            {
                if (_1stIndex) { sql.AppendFormat("{0}{1}", ParamPrefix, item.Value.Name); _1stIndex = false; }
                else sql.AppendFormat(",{0}{1}", ParamPrefix, item.Value.Name);
            }
            sql.Append(") ");
            return sql.ToString();
        }

        /// <summary>
        /// 更新SQL脚本生成器
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="properties">参数</param>
        /// <param name="ParamPrefix">参数前缀</param>
        /// <returns></returns>
        private static string BuildUpdateSql(string tbName, Dictionary<String, PropertyEx> properties, string ParamPrefix)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("UPDATE {0} SET", tbName);

            var updProperties = properties.Where(x => x.Value.IsKey == false);
            bool _1stIndex = true;
            foreach (var item in updProperties)
            {
                if (_1stIndex) _1stIndex = false;
                else sql.Append(",");
                sql.AppendFormat(" {0}  = {1}", item.Value.DbName, ParamPrefix + item.Value.Name);
            }
            sql.Append(" WHERE 1=1");
            var keyProperties = properties.Where(x => x.Value.IsKey == true);
            foreach (var item in keyProperties)
                sql.AppendFormat(" AND {0} = {1}", item.Value.DbName, ParamPrefix + item.Value.Name);

            return sql.ToString();
        }

        /// <summary>
        /// 删除SQL脚本生成器
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="properties">参数</param>
        /// <param name="ParamPrefix">参数前缀</param>
        /// <returns></returns>
        private static string BuildDeleteSql(string tbName, Dictionary<String, PropertyEx> properties, string ParamPrefix)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("DELETE FROM {0}", tbName);
            sql.Append(" WHERE 1=1");
            var keyProperties = properties.Where(x => x.Value.IsKey == true);
            foreach (var item in keyProperties)
                sql.AppendFormat(" AND {0} = {1}", item.Value.DbName, ParamPrefix + item.Value.Name);
            return sql.ToString();
        }

        /// <summary>
        /// 查询SQL脚本生成器
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="properties">参数</param>
        /// <param name="ParamPrefix">参数前缀</param>
        /// <returns></returns>
        private static string BuildSelectSql(string tbName, Dictionary<String, PropertyEx> properties, string ParamPrefix)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");

            bool _1stIndex = true;
            foreach (var item in properties)
            {
                if (_1stIndex) _1stIndex = false;
                else sql.Append(",");
                sql.AppendFormat(@" {0}  AS ""{1}"" ", item.Value.DbName, item.Value.Name);
            }
            sql.AppendFormat(" FROM {0} WHERE 1=1 #WHERE#", tbName);
            return sql.ToString();
        }
        #endregion

        #region 缓存get set
        /// <summary>
        /// 动态类设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool GetCache<T>(string key, ref T t)
        {
            if (DynamicCache.ContainsKey(key))
            {
                t = (T)DynamicCache[key];
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取动态类缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SetCache(string key, object value)
        {
            DynamicCache[key] = value;
        }
        #endregion
    }
}
