using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Data.DapperEx.Context;
using WCFServer.Service.Demo.Contract;
using Xunit;

namespace WCFServer.Test
{
    public class TestTypeGet
    {
        [Fact]
        public void GetIntfType()
        {

        }

        private static IEnumerable<Type> GetType(Type interfaceType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            yield return type;
                            break;
                        }
                    }
                }
            }
        }

        public void SQLServerEntitiesCreate()
        {
            using (var db = new DapperExContext())
            {
                string sql = string.Format(@"SELECT
		                                        A.TABLE_NAME AS TableName,
		                                        H.TableDec AS TableDec,
		                                        A.COLUMN_NAME AS ColumnName,
		                                        H.ColumnDec AS ColumnDec,
		                                        A.COLUMN_DEFAULT AS DefaultValue,
		                                        CASE
			                                        WHEN A.IS_NULLABLE = 'YES' THEN 'true'
			                                        WHEN A.IS_NULLABLE = 'NO'	 THEN 'false'
		                                        END AS IsNullable,
		                                        A.DATA_TYPE AS ColumnType,
		                                        A.CHARACTER_MAXIMUM_LENGTH AS Length,
		                                        CASE
			                                        WHEN B.COLUMN_NAME IS NULL THEN 'false'
			                                        WHEN B.COLUMN_NAME IS NOT NULL THEN 'true'
		                                        END AS IsPrimaryKey
	                                        FROM
		                                        INFORMATION_SCHEMA.COLUMNS A
	                                        LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE B ON B.COLUMN_NAME = A.COLUMN_NAME AND B.TABLE_NAME = A.TABLE_NAME
	                                        LEFT JOIN (
			                                        SELECT
				                                        D.name AS TableName,
				                                        E.name AS ColumnName,
				                                        F.VALUE AS ColumnDec,
				                                        G.VALUE AS TableDec
			                                        FROM
				                                        sys.tables D
			                                        INNER JOIN sys.columns E ON E.object_id = D.object_id
			                                        LEFT JOIN sys.extended_properties F ON F.major_id = E.object_id	AND F.minor_id = E.column_id
			                                        LEFT JOIN sys.extended_properties G ON G.major_id = E.object_id AND G.minor_id = '0'
	                                        ) H ON H.TableName = A.TABLE_NAME AND H.ColumnName = A.COLUMN_NAME
                                            WHERE A.TABLE_NAME = 'sys_server_info_mst'
	                                        ORDER BY A.TABLE_NAME ASC");
                var result = db.Query<TableSchema>(sql).ToList();
                var resultDictionary = result.GroupBy(x => x.TableName).ToDictionary(k => k.Key, v => v.ToList());
                CultureInfo cultureInfo = new CultureInfo("en", false);
                string content = string.Empty;

                foreach (KeyValuePair<String, List<TableSchema>> item in resultDictionary)
                {
                    // 表名去下划线 & 驼峰
                    string[] tableNameArray = item.Key.ToLower().Split('_');
                    string nTableName = string.Empty;
                    foreach (string n in tableNameArray)
                    {
                        nTableName += cultureInfo.TextInfo.ToTitleCase(n);
                    }

                    Directory.CreateDirectory("D:\\DapperEntities\\");
                    using (FileStream fs = new FileStream("D:\\DapperEntities\\" + nTableName + ".cs", FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            content = string.Format(
@"/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{{
    /// <summary>
    /// {0} 【{1}】
    /// </summary>
    [Table(""{2}"")]
    public class {3}
    {{", nTableName, item.Value.FirstOrDefault().TableDec, item.Key, nTableName);

                            sw.WriteLine(content);

                            foreach (TableSchema colItem in item.Value)
                            {
                                string colType = string.Empty;
                                if (colItem.ColumnType.ToLower() == "nvarchar" || colItem.ColumnType.ToLower() == "nchar" || colItem.ColumnType.ToLower() == "varchar" || colItem.ColumnType.ToLower() == "text")
                                    colType = "string";
                                if (colItem.ColumnType.ToLower() == "int" || colItem.ColumnType.ToLower() == "smallint" || colItem.ColumnType.ToLower() == "tinyint")
                                    colType = "int";
                                if (colItem.ColumnType.ToLower() == "decimal" || colItem.ColumnType.ToLower() == "float")
                                    colType = "double";
                                if (colItem.ColumnType.ToLower() == "datetime" || colItem.ColumnType.ToLower() == "date")
                                    colType = "DateTime";

                                string[] colNameArray = colItem.ColumnName.ToLower().Split('_');
                                string nColName = string.Empty;
                                foreach (string n in colNameArray)
                                {
                                    nColName += cultureInfo.TextInfo.ToTitleCase(n);
                                }

                                content = string.Format(
@"
        /// <summary>
        /// {0} 【{1}】
        /// </summary>", nColName, colItem.ColumnDec);
                                sw.WriteLine(content);

                                content = string.Format(
@"        [{0}Column(""{1}"")]
        public {2} {3} {{ get; set; }}", colItem.IsPrimaryKey ? "Key, " : "", colItem.ColumnName, colType, nColName);
                                sw.WriteLine(content);
                            }
                            sw.WriteLine(
@"      }
}");
                        }
                    }
                }

            }
        }

    }

    /// <summary>
    /// 表信息
    /// </summary>
    public class TableSchema
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName;
        /// <summary>
        /// 表注释
        /// </summary>
        public string TableDec;
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// 列注释
        /// </summary>
        public string ColumnDec;
        /// <summary>
        /// 列类型
        /// </summary>
        public string ColumnType;
        /// <summary>
        /// 是否值主键
        /// </summary>
        public bool IsPrimaryKey;
        /// <summary>
        /// 长度
        /// </summary>
        public int Length;
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
    }

}
