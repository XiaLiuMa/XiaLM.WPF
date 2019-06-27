using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk;
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
    public class MapManagerViewModel : ViewModel
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

        private ObservableCollection<MapResource> _mapResources;

        public ObservableCollection<MapResource> MapResources
        {
            get
            {
                return _mapResources;
            }
            set
            {
                if (_mapResources != value)
                {
                    _mapResources = value;
                    OnPropertyChanged("MapResource");
                }
            }
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
        #endregion

        public MapManagerViewModel() {
            _mapResources = new ObservableCollection<MapResource>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
            RefreshDataGrid(0);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="currentPage"></param>
        public void RefreshDataGrid(int currentPage = 1)
        {
            var dal = new MapResourceDal();
            var outPage = dal.GetMapInfos(new InPage() { Current = currentPage, Row = PageSize });
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            var list = new List<MapResource>();
            outPage.Datas.ForEach(r =>
            {
                var resource = new MapResource()
                {
                    Az=r.Az,
                    EnName=r.EnName,
                    Id=r.Id,
                    MapId=r.MapId,
                    MaxZoom=r.MaxZoom,
                    MinZoom=r.MinZoom,
                    Name=r.Name,
                    Url=$"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{r.Url}",
                    Zoom=r.Zoom,
                    Width=r.Width,
                    Height=r.Height
                };
                list.Add(resource);
            });
            MapResources.Clear();
            MapResources.AddRange(list);
        }
    }
}
