using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class SelectMapPositionViewModel : ViewModel
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

        private ObservableCollection<MapPositionMod> _mapPositionMods;

        public ObservableCollection<MapPositionMod> MapPositionMods
        {
            get
            {
                return _mapPositionMods;
            }
            set
            {
                if (_mapPositionMods != value)
                {
                    _mapPositionMods = value;
                    OnPropertyChanged("MapPositionMod");
                }
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(_mapid, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(_mapid, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(_mapid, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(_mapid, CurrentPage);
        }
        public SelectMapPositionViewModel()
        {
            _mapPositionMods = new ObservableCollection<MapPositionMod>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
        }


        #endregion
        private string _mapid = string.Empty;
        public void RefreshDataGrid(string mapid, int currentPage = 1)
        {
            if (string.IsNullOrEmpty(mapid)) return;
            _mapid = mapid;
            var dal = new MapResourceDal();
            var outPage = dal.GetMapPositionInfoList(new InPage() { Current = currentPage, Row = PageSize }, _mapid);
            var list = new List<MapPositionMod>();
            outPage.Datas.ForEach(r =>
            {
                var resource = new MapPositionMod()
                {
                    Name = r.Name,
                    EnName = r.EnName,
                    Id = r.Id,
                    Mapid = r.Mapid,
                    PosId = r.PosId
                };
                list.Add(resource);
            });
            MapPositionMods.Clear();
            MapPositionMods.AddRange(list);
        }
        public void RefreshDataGrid()
        {
            RefreshDataGrid(_mapid, CurrentPage);
        }
    }
}
