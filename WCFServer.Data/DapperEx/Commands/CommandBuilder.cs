using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace WCFServer.Data.DapperEx.Commands
{
    /// <summary>
    /// 生成SQL脚本类
    /// </summary>
    public class CommandBuilder
    {
        #region 私有变量

        private string _parameterPrefix = "@";                      //参数前缀
        private readonly string _placeHolderWhere = "#WHERE#";      //Where 过滤条件占位符
        private readonly string _placeHolderSet = "#SET#";          //Set 占位符
        private readonly string _placeHolderBetween = "#BETWEEN#";  //Between 占位符

        #endregion

        #region 公开属性

        /// <summary>
        /// 删除键
        /// </summary>
        public static readonly string Delete = "Delete";
        /// <summary>
        /// 删除键
        /// </summary>
        public static readonly string DeleteByKey = "DeleteByKey";
        /// <summary>
        /// 更新键
        /// </summary>
        public static readonly string Update = "Update";
        /// <summary>
        /// 更新键
        /// </summary>
        public static readonly string UpdateByKey = "UpdateByKey";
        /// <summary>
        /// 更新键
        /// </summary>
        public static readonly string UpdateByExpr = "UpdateByExpr";
        /// <summary>
        /// 插入键
        /// </summary>
        public static readonly string Insert = "Insert";
        /// <summary>
        /// 选取键
        /// </summary>
        public static readonly string Select = "Select";
        /// <summary>
        /// 选取键
        /// </summary>
        public static readonly string SelectJoin = "SelectJoin";
        /// <summary>
        /// 选取键
        /// </summary>
        public static readonly string SelectByKey = "SelectByKey";
        /// <summary>
        /// 选取键
        /// </summary>
        public static readonly string SelectByPaging = "SelectByPaging";
        /// <summary>
        /// 计数键
        /// </summary>
        public static readonly string Count = "Count";


        /// <summary>
        /// 参数查询前缀
        /// </summary>
        public string ParameterPrefix
        {
            get { return _parameterPrefix; }
        }

        #endregion

        #region 构造函数

        public CommandBuilder() : this("@")
        { }

        public CommandBuilder(string parameterPrefix)
        {
            _parameterPrefix = parameterPrefix;
        }

        #endregion

        #region 公开方法

        internal Command Build<T>(string cmdName, T TEntity) where T : class
        {
            Command cmd = this.GetCommand<T>(cmdName);
            return cmd;
        }

        internal Command Build<T>(Expression<Func<T, T>> updater, Expression<Func<T, bool>> predicate) where T : class
        {
            //UPDATE TABLE #SET# WHERE 1=1 And FieldName = @p0
            Command cmd = this.Build<T>(CommandBuilder.UpdateByExpr, predicate, null, null);

            MemberInitExpression updateExpr = (MemberInitExpression)updater.Body;
            StringBuilder sqlBuilder = new StringBuilder(Environment.NewLine);
            for (int i = 0; i < updateExpr.Bindings.Count; i++)
            {
                //SQL片断
                MemberAssignment member = (MemberAssignment)updateExpr.Bindings[i];

                var propertiesMap = MapperBuilder.GetPropertiesMap<T>();
                string dbName = member.Member.Name;
                if (propertiesMap.ContainsKey(member.Member.Name)) dbName = propertiesMap[member.Member.Name].DbName;
                sqlBuilder.AppendFormat("{0} = {1}{2}", dbName, _parameterPrefix, member.Member.Name);
                if (i < updateExpr.Bindings.Count - 1) sqlBuilder.Append(",");
                sqlBuilder.AppendLine();

                //SQL参数
                if (member.Expression is ConstantExpression)
                    cmd.DynamicParameters.Add(member.Member.Name, ((ConstantExpression)member.Expression).Value);
                else
                {
                    //计算常量
                    PartialEvaluator evaluator = new PartialEvaluator();
                    Expression evalExpr = evaluator.Eval(member.Expression);

                    cmd.DynamicParameters.Add(member.Member.Name, ((ConstantExpression)evalExpr).Value);
                }
            }

            cmd.Text = Regex.Replace(cmd.Text, _placeHolderSet, sqlBuilder.ToString(), RegexOptions.IgnoreCase);
            return cmd;
        }
        public Command Build<T>(string cmdName, Expression<Func<T, bool>> predicate, IDictionary<string, object> dynParameters, OrderBy orderBy)
                  where T : class
        {
            Command cmd = new Command();
            MapperBuilder mb = new MapperBuilder(_parameterPrefix).Build<T>(cmdName);

            cmd.Text = mb.Sql;
            //解析过滤条件
            if (predicate == null) predicate = Common.True<T>();
            ConditionBuilder builder = new ConditionBuilder(_parameterPrefix, cmdName == "SelectJoin" ? "A" : "");
            builder.Build(predicate, mb.PropertiesMap, dynParameters);
            cmd.Text = Regex.Replace(cmd.Text, _placeHolderWhere, builder.Condition, RegexOptions.IgnoreCase);

            //追加OrderBy
            if (orderBy != null && orderBy.Properties.Count() > 0)
            {
                cmd.Text += " Order By ";
                var list = orderBy.Properties.ToList();
                foreach (var item in list)
                {
                    PropertyEx p = null;
                    if (mb.PropertiesMap.TryGetValue(item, out p))
                    {
                        if (list.IndexOf(item) > 0)
                            cmd.Text += ",";
                        cmd.Text += p.DbName;
                    }
                }
                cmd.Text += " Desc ";
            }
            //添加脚本参数
            foreach (var bParameter in builder.Parameters)
            {
                //参数
                string parameterName = bParameter.Key;
                cmd.DynamicParameters.Add(parameterName, bParameter.Value);
            }
            return cmd;
        }
        #endregion

        #region 辅助方法

        protected Command GetCommand<T>(string cmdName) where T : class
        {
            MapperBuilder mb = new MapperBuilder(_parameterPrefix).Build<T>(cmdName);
            Command cmd = new Command() { Text = mb.Sql };
            return cmd;
        }

        #endregion
    }
}
