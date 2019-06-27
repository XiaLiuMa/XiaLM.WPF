using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.ViewModels
{
    /// <summary>
    /// 语义列表模型
    /// </summary>
    public class SemanticViewModel : ViewModel
    {
        #region 属性
        private string genre;   //语义库类型

        private ICommand _firstPageCommand;

        public ICommand FirstPageCommand
        {
            get
            {
                return _firstPageCommand;
            }

            set
            {
                _firstPageCommand = value;
            }
        }

        private ICommand _previousPageCommand;

        public ICommand PreviousPageCommand
        {
            get
            {
                return _previousPageCommand;
            }

            set
            {
                _previousPageCommand = value;
            }
        }

        private ICommand _nextPageCommand;

        public ICommand NextPageCommand
        {
            get
            {
                return _nextPageCommand;
            }

            set
            {
                _nextPageCommand = value;
            }
        }

        private ICommand _lastPageCommand;

        public ICommand LastPageCommand
        {
            get
            {
                return _lastPageCommand;
            }

            set
            {
                _lastPageCommand = value;
            }
        }

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged("PageSize");
                }
            }
        }

        private int _currentPage = 1;

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }

            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged("CurrentPage");
                }
            }
        }

        private int _totalPage;

        public int TotalPage
        {
            get
            {
                return _totalPage;
            }

            set
            {
                if (_totalPage != value)
                {
                    _totalPage = value;
                    OnPropertyChanged("TotalPage");
                }
            }
        }

        private ObservableCollection<SemanticInfo> _fakeSoruce;

        public ObservableCollection<SemanticInfo> FakeSource
        {
            get
            {
                return _fakeSoruce;
            }
            set
            {
                if (_fakeSoruce != value)
                {
                    _fakeSoruce = value;
                    OnPropertyChanged("FakeSource");
                }
            }
        }

        /// <summary>
        /// 语义库列表
        /// </summary>
        private ObservableCollection<SemanticGenreInfo> _comboxSoruce;

        public ObservableCollection<SemanticGenreInfo> ComboxSource
        {
            get
            {
                return _comboxSoruce;
            }
            set
            {
                if (_comboxSoruce != value)
                {
                    _comboxSoruce = value;
                    OnPropertyChanged("ComboxSource");
                }
            }
        }
        #endregion

        public SemanticViewModel()
        {
            _fakeSoruce = new ObservableCollection<SemanticInfo>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);

            _comboxSoruce = new ObservableCollection<SemanticGenreInfo>();
            RefreshComBox();
        }

        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(genre, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(genre, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(genre, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(genre, CurrentPage);
        }

        /// <summary>
        /// 刷新语义库列表
        /// </summary>
        public void RefreshComBox()
        {
            SemanticmanagerDal dal = new SemanticmanagerDal();
            List<SemanticGenreInfo> lst = dal.GetSemanticGenreInfos(0, int.MaxValue).Datas;
            if (lst == null) lst = new List<SemanticGenreInfo>();
            _comboxSoruce.Clear();
            _comboxSoruce.AddRange(lst);
            var obj = ComboxSource.FirstOrDefault();
            if (obj != null) RefreshDataGrid(ComboxSource.FirstOrDefault().Name);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="currentPage"></param>
        public void RefreshDataGrid(string genre, int currentPage = 1)
        {
            this.genre = genre;
            SemanticmanagerDal dal = new SemanticmanagerDal();
            OutPage<SemanticInfo> outPage = new OutPage<SemanticInfo>();
            outPage = dal.GetSemanticInfos(genre, currentPage, PageSize);
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            _fakeSoruce.Clear();
            _fakeSoruce.AddRange(outPage.Datas);
        }
    }
}
