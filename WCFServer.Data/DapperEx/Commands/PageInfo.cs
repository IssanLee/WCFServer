using System;

namespace WCFServer.Data.DapperEx.Commands
{
    public class PageInfo
    {
        #region 私有变量

        private int _pageSize = 20;
        private int _currentPage = 1;

        #endregion

        #region 公开属性

        /// <summary>
        /// 页长
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("值必须大于0！");
                _pageSize = value;
            }
        }

        /// <summary>
        /// 当前面
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("值必须大于0！");
                _currentPage = value;
            }
        }

        #endregion

        #region 构造函数

        public PageInfo(int curPage, int pageSize)
        {
            this.CurrentPage = curPage;
            this.PageSize = pageSize;
        }

        #endregion
    }
}
