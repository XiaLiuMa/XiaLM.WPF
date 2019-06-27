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
    public class TaskManagerViewModel : ViewModel
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

        private ObservableCollection<TaskInfoMod> _taskInfos;

        public ObservableCollection<TaskInfoMod> TaskInfos
        {
            get
            {
                return _taskInfos;
            }
            set
            {
                if (_taskInfos != value)
                {
                    _taskInfos = value;
                    OnPropertyChanged("TaskInfoMod");
                }
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(_query, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(_query, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(_query, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(_query, CurrentPage);
        }
        #endregion
        private string _query = string.Empty;
        public TaskManagerViewModel()
        {
            _taskInfos = new ObservableCollection<TaskInfoMod>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
            RefreshDataGrid("");
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="currentPage"></param>
        public void RefreshDataGrid(string query, int currentPage = 1)
        {
            _query = query;
            var dal = new TaskMangerDal();
            var outPage = dal.GetTasks(query, new InPage() { Current = currentPage, Row = PageSize });
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            var list = new List<TaskInfoMod>();
            int num = 0;
            outPage.Datas.ForEach(r =>
            {
                num++;
                var resource = new TaskInfoMod()
                {
                    SerialNumber = num,
                    CirculateCount = r.CirculateCount,
                    Desc = r.Desc,
                    Id = r.Id,
                    Mapid = r.Mapid,
                    Name = r.Name,
                    Time = r.Time,
                    TaskType = r.TaskType,
                    TaskTypeStr = r.TaskType == TaskType.cruise ? "巡航" : r.TaskType == TaskType.line ? "连续线" : "连续点"
                };
                list.Add(resource);
            });
            TaskInfos.Clear();
            TaskInfos.AddRange(list);
        }
    }
}
