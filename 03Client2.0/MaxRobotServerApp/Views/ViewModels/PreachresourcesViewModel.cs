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
    public class PreachresourcesViewModel : ViewModel
    {
        #region 属性
        private int filetype;   //查询类型


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

        private ObservableCollection<Preachresource> _fakeSoruce;

        public ObservableCollection<Preachresource> FakeSource
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

        public PreachresourcesViewModel()
        {
            _fakeSoruce = new ObservableCollection<Preachresource>();
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
            RefreshDataGrid(filetype, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(filetype, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(filetype, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(filetype,CurrentPage);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="type"></param>
        public void RefreshDataGrid(int type,int currentPage = 1)
        {
            filetype = type;
            PreachresourceDal dal = new PreachresourceDal();
            OutPage<ResourceInfo> outPage = new OutPage<ResourceInfo>();
            if (type != 0)  //根据文件类型查询
            {
                switch (type)
                {
                    case 1:
                        outPage = dal.GetPreachresources(currentPage, PageSize, ResourceType.Video);
                        break;
                    case 2:
                        outPage = dal.GetPreachresources(currentPage, PageSize, ResourceType.Voice);
                        break;
                    case 3:
                        outPage = dal.GetPreachresources(currentPage, PageSize, ResourceType.Text);
                        break;
                    case 4:
                        outPage = dal.GetPreachresources(currentPage, PageSize, ResourceType.Picture);
                        break;
                }
            }
            else    //查询全部
            {
                outPage = dal.GetPreachresources(currentPage, PageSize);
            }
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            List<Preachresource> preachresources = new List<Preachresource>();
            foreach (var item in outPage.Datas)
            {
                Preachresource obj = new Preachresource();
                obj.Id = item.Id;
                obj.Name = item.Name;
                obj.Desc = item.Desc;
                obj.FileSize = item.FileSize;
                switch (item.ResourceType)
                {
                    case ResourceType.Video:
                        obj.ResourceType = "视频";
                        break;
                    case ResourceType.Voice:
                        obj.ResourceType = "音频";
                        break;
                    case ResourceType.Text:
                        obj.ResourceType = "文本";
                        break;
                    case ResourceType.Picture:
                        obj.ResourceType = "图片";
                        break;
                }
                obj.ThumbnailFileInfoPath = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{item.ThumbnailFileInfoPath}";
                obj.FileInfoPath = item.FileInfoPath;
                obj.Time = item.Time;
                preachresources.Add(obj);
            }
            FakeSource.Clear();
            FakeSource.AddRange(preachresources);
        }
    }
}
