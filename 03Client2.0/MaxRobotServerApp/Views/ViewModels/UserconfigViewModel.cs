using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class UserconfigViewModel : ViewModel
    {
        #region 属性
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

        private ObservableCollection<Userinfo> _fakeSoruce;

        public ObservableCollection<Userinfo> FakeSource
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
        #endregion

        public UserconfigViewModel()
        {
            _fakeSoruce = new ObservableCollection<Userinfo>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
            RefreshDataGrid(0);
        }

        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(CurrentPage);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="currentPage"></param>
        public void RefreshDataGrid(int currentPage = 1)
        {
            UserManageDal dal = new UserManageDal();
            OutPage<UserInfo> outPage = dal.GetList(currentPage, PageSize);
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            List<Userinfo> lst = new List<Userinfo>();
            foreach (var item in outPage.Datas)
            {
                Userinfo obj = new Userinfo();
                obj.Id = item.Id;
                obj.Name = item.Name;
                obj.Password = item.Password;
                switch (item.UserType)
                {
                    case UserType.Admin:
                        obj.UserType = "管理员";
                        break;
                    case UserType.User:
                        obj.UserType = "普通用户";
                        break;
                }
                lst.Add(obj);
            }
            FakeSource.Clear();
            FakeSource.AddRange(lst);
        }
    }
}
