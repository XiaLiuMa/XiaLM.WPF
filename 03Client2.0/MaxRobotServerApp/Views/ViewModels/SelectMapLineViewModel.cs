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
    public class SelectMapLineViewModel : ViewModel
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

        private ObservableCollection<MapLineMod> _mapLineMods;

        public ObservableCollection<MapLineMod> MapLineMods
        {
            get
            {
                return _mapLineMods;
            }
            set
            {
                if (_mapLineMods != value)
                {
                    _mapLineMods = value;
                    OnPropertyChanged("MapLineMod");
                }
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(_mapid, _lineType, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(_mapid, _lineType, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(_mapid, _lineType, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(_mapid, _lineType, CurrentPage);
        }


        #endregion
        private string _mapid = string.Empty;
        private LineType _lineType = LineType.major;
        public SelectMapLineViewModel()
        {
            _mapLineMods = new ObservableCollection<MapLineMod>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
        }
        public void RefreshDataGrid(string mapid, LineType lineType, int currentPage=1)
        {
            if (string.IsNullOrEmpty(mapid)) return;
            _mapid = mapid;
            _lineType = lineType;
            var dal = new MapResourceDal();
            var outPage = dal.GetMapLineInfoList(new InPage() { Current = currentPage, Row = PageSize }, _lineType, _mapid);
            var list = new List<MapLineMod>();
            outPage.Datas.ForEach(r =>
            {
                var resource = new MapLineMod()
                {
                    EnName = r.EnName,
                    Id = r.Id,
                    LineId = r.LineId,
                    MapId = r.MapId,
                    Name = r.Name
                };
                list.Add(resource);
            });
            MapLineMods.Clear();
            MapLineMods.AddRange(list);
        }
        public void RefreshDataGrid()
        {
            RefreshDataGrid(_mapid, _lineType, CurrentPage);
        }
    }
}
