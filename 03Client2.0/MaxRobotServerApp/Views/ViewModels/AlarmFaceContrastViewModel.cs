﻿using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class AlarmFaceContrastViewModel : ViewModel
    {
        #region 属性
        private string tag = "";   //机器人标识
        private string sTime = "";   //开始时间
        private string eTime = "";   //结束时间

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

        private ObservableCollection<FaceContrastAlarm> _fakeSoruce;

        public ObservableCollection<FaceContrastAlarm> FakeSource
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

        public AlarmFaceContrastViewModel()
        {
            _fakeSoruce = new ObservableCollection<FaceContrastAlarm>();
            _firstPageCommand = new DelegateCommand(FirstPageAction);
            _previousPageCommand = new DelegateCommand(PreviousPageAction);
            _nextPageCommand = new DelegateCommand(NextPageAction);
            _lastPageCommand = new DelegateCommand(LastPageAction);
        }

        /// <summary>
        /// 首页
        /// </summary>
        private void FirstPageAction()
        {
            CurrentPage = 1;
            RefreshDataGrid(tag, sTime, eTime, CurrentPage);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPageAction()
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            RefreshDataGrid(tag, sTime, eTime, CurrentPage);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPageAction()
        {
            if (CurrentPage >= TotalPage) return;
            CurrentPage++;
            RefreshDataGrid(tag, sTime, eTime, CurrentPage);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        private void LastPageAction()
        {
            CurrentPage = TotalPage;
            RefreshDataGrid(tag, sTime, eTime, CurrentPage);
        }

        /// <summary>
        /// 修改PageSize
        /// </summary>
        /// <param name="size"></param>
        public void ChangePageSize(int size)
        {
            PageSize = size;
            RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public void RefreshDataGrid()
        {
            RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="type"></param>
        public void RefreshDataGrid(string _tag, string _sTime, string _eTime, int currentPage = 1)
        {
            tag = _tag;
            sTime = _sTime;
            eTime = _eTime;
            OutPage<FaceContrastAlarm> outPage = new OutPage<FaceContrastAlarm>();
            AlarmmanagerDal dal = new AlarmmanagerDal();
            if (string.IsNullOrEmpty(tag))  //根据时间查询
            {
                var time1 = Convert.ToDateTime(sTime);
                var time2 = Convert.ToDateTime(eTime);
                outPage = dal.GetFaceContrastAlarmList(time1, time2, new InPage()
                {
                    Current = currentPage,
                    Row = PageSize
                });
            }
            else    //根据机器人标识和报警时间查询
            {
                var time1 = Convert.ToDateTime(sTime);
                var time2 = Convert.ToDateTime(eTime);
                outPage = dal.GetFaceContrastAlarmList(tag, time1, time2, new InPage()
                {
                    Current = currentPage,
                    Row = PageSize
                });
            }
            TotalPage = (outPage.TotalPageNumber + PageSize - 1) / PageSize;
            List<FaceContrastAlarm> alarms = new List<FaceContrastAlarm>();
            foreach (var item in outPage.Datas)
            {
                FaceContrastAlarm obj = new FaceContrastAlarm()
                {
                    Id = item.Id,
                    RobotTag = item.RobotTag,
                    TravellerName = item.TravellerName,
                    TravellerSex = item.TravellerSex,
                    TravellerAge = item.TravellerAge,
                    TravellerIDNumber = item.TravellerIDNumber,
                    TravellerGrade = item.TravellerGrade,
                    Similarity = item.Similarity,
                    CaptureImgUrl = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{item.CaptureImgUrl}",
                    IDImgUrl = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{item.IDImgUrl}",
                    EnteringTime = item.EnteringTime,
                    Time = item.Time
                };
                alarms.Add(obj);
            }
            FakeSource.Clear();
            FakeSource.AddRange(alarms);
        }
    }
}
