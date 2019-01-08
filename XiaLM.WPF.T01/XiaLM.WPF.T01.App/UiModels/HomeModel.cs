﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using XiaLM.WPF.T01.App.Models;
using XiaLM.WPF.T01.App.UserControls;
using XiaLM.WPF.T01.Utility;

namespace XiaLM.WPF.T01.App.UiModels
{
    public class HomeModel : BaseViewModel
    {
        #region  声明

        public object ThisContorler;

        public ICommand AddNewDataCommand => new AnotherCommandImplementation(AddNewData);
        public ICommand DeleteDataGridItemCommand => new AnotherCommandImplementation(DeleteDataGridItem);
        public ICommand LoadedCommand => new AnotherCommandImplementation(Loaded);
        public ICommand DataGridMouseDoubleClickCommand => new AnotherCommandImplementation(DataGridMouseDoubleClick);
        public ICommand CleanDataCommand => new AnotherCommandImplementation(CleanData);
        public ICommand EditDataCommand => new AnotherCommandImplementation(EditData);

        private WorkTimeData _newData = new WorkTimeData();

        private ObservableCollection<WorkTimeViewData> _dataItems = new ObservableCollection<WorkTimeViewData>();

        private ObservableCollection<string> _statusCollection;
        private DateTime _workDateTime;
        private string _type;
        private string _title;
        private string _detail;
        private string _status;
        private ObservableCollection<string> _typeCollection;
        private DateTime _beginTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;
        private WorkTimeViewData _selecTimeDataViewData;
        private string _snackBarMessage;
        private Visibility _addVisibility = Visibility.Visible;
        private Visibility _editVisibility = Visibility.Collapsed;
        private WorkTimeViewData _editItemData;

        #endregion

        #region 属性设置

        public Visibility AddVisibility
        {
            get { return _addVisibility; }
            set
            {
                this.MutateVerbose(ref _addVisibility, value, RaisePropertyChanged());

            }
        }

        public Visibility EditVisibility
        {
            get { return _editVisibility; }
            set
            {
                this.MutateVerbose(ref _editVisibility, value, RaisePropertyChanged());

            }
        }

        public ObservableCollection<WorkTimeViewData> DataItems
        {
            get { return _dataItems; }
            set
            {
                this.MutateVerbose(ref _dataItems, value, RaisePropertyChanged());
                this.OnPropertyChanged("DataItems");
            }
        }

        public WorkTimeViewData SelecTimeDataViewData
        {
            get { return _selecTimeDataViewData; }
            set
            {
                this.MutateVerbose(ref _selecTimeDataViewData, value, RaisePropertyChanged());

            }
        }

        public string SnackBarMessage
        {
            get { return _snackBarMessage; }
            set
            {
                this.MutateVerbose(ref _snackBarMessage, value, RaisePropertyChanged());
            }
        }

        public ObservableCollection<string> StatusCollection
        {
            get { return _statusCollection; }
            set
            {
                this.MutateVerbose(ref _statusCollection, value, RaisePropertyChanged());
            }
        }

        public ObservableCollection<string> TypeCollection
        {
            get { return _typeCollection; }
            set
            {
                this.MutateVerbose(ref _typeCollection, value, RaisePropertyChanged());

            }
        }

        #endregion

        #region 新增数据

        public WorkTimeViewData EditItemData
        {
            get { return _editItemData; }
            set { _editItemData = value; }
        }

        /// <summary>
        /// 工作日
        /// </summary>
        public DateTime WorkDateTime
        {
            get { return _workDateTime; }
            set { this.MutateVerbose(ref _workDateTime, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { this.MutateVerbose(ref _type, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { this.MutateVerbose(ref _title, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目明细
        /// </summary>
        public string Detail
        {
            get { return _detail; }
            set { this.MutateVerbose(ref _detail, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { this.MutateVerbose(ref _status, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 工作开始时间
        /// </summary>
        public DateTime Begin_time
        {
            get { return _beginTime; }
            set
            {
                this.MutateVerbose(ref _beginTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        public DateTime End_time
        {
            get { return _endTime; }
            set
            {
                this.MutateVerbose(ref _endTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        /// <summary>
        /// 项目花费时间【单位：分钟】
        /// </summary>
        private string Spend
        {
            get
            {
                return
                    (long.Parse(Common.GetTimeSecond(End_time)) - (long.Parse(Common.GetTimeSecond(Begin_time))))
                    .ToString();
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        public WorkTimeData NewData
        {
            get { return _newData; }
            set { _newData = value; }
        }

        #endregion

        #region 界面绑定方法

        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        public HomeModel()
        {

        }

        public void Loaded(Object o)
        {

            Inint(o);
        }


        /// <summary>
        /// 初始化，读取在线配置信息
        /// </summary>
        private void Inint(object o)
        {
            //var loadDialog = new LoadDialog();

            //var result = DialogHost.Show(loadDialog, "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            //{
            //    string access_token = MainStaticData.AccessToken;

            //    string get_data = "https://api.bobdong.cn/time_manager/data/select?access_token=" + access_token;

            //    var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

            //    this.ThisContorler = o;

            //    var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<WorkTimeData>>>(datas);

            //    ThreadStart start = delegate ()
            //    {

            //        Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
            //        {
            //            if (datasObject.code != 0)
            //            {

            //            }
            //            else
            //            {
            //                DataItems.Clear();
            //                foreach (var item in datasObject.data)
            //                {
            //                    DataItems.Add(new WorkTimeViewData(item));
            //                }
            //            }

            //            StatusCollection = MainStaticData.StstusCollection;

            //            TypeCollection = MainStaticData.TypeCollection;

            //            WorkDateTime = DateTime.Now;

            //            Status = StatusCollection.First();


            //            args.Session.Close(false);
            //        });

            //    };

            //    new Thread(start).Start(); // 启动线程

            //});

        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="o"></param>
        public void AddNewData(object o)
        {
            //var result = DialogHost.Show(new LoadingDialog(), "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            //{
            //    ThreadStart start = delegate ()
            //    {
            //        WorkTimeData postWorkTimeData = new WorkTimeData()
            //        {
            //            work_date = Common.GetTimeSecond(WorkDateTime),
            //            title = this.Title,
            //            detail = this.Detail,
            //            type = this.Type,
            //            state = this.Status,
            //            begin_time = Common.GetTimeSecond(this.Begin_time),
            //            end_time = Common.GetTimeSecond(this.End_time),
            //            spend = (long.Parse(Common.GetTimeSecond(this.End_time)) - (long.Parse(Common.GetTimeSecond(this.Begin_time)))).ToString()
            //        };

            //        DateTime Temp_end_time = End_time;

            //        string temp = NetHelper.GETProperties(postWorkTimeData);

            //        string addUrl = "https://api.bobdong.cn/time_manager/data/add?access_token=" + MainStaticData.AccessToken;

            //        var datas = NetHelper.HttpCall(addUrl, temp, HttpEnum.Post);

            //        var returnData = JsonHelper.Deserialize<ReturnData<WorkTimeData>>(datas);


            //        Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
            //        {
            //            args.Session.Close(false);
            //            if (returnData.code == 0)
            //            {
            //                DataItems.Add(new WorkTimeViewData(returnData.data));
            //                CleanData(null);
            //                this.Begin_time = Temp_end_time;
            //                MessageShow(o, "Add Success !");

            //            }
            //            else
            //            {

            //                MessageShow(o, returnData.message);
            //            }

            //        });
            //    };
            //    new Thread(start).Start(); // 启动线程
            //});
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="o"></param>
        public void DeleteDataGridItem(object o)
        {
            //Int64 id = SelecTimeDataViewData.GetID();
            //string deleteData = "https://api.bobdong.cn/time_manager/data/delete?access_token=" + MainStaticData.AccessToken + "&id=" + id;
            //var datas = NetHelper.HttpCall(deleteData, null, HttpEnum.Get);
            //var returnData = JsonHelper.Deserialize<ReturnData<object>>(datas);
            //if (returnData.code == 0)
            //{
            //    DataItems.Remove(SelecTimeDataViewData);
            //    MessageShow(o, "Delete Success!");
            //}

        }

        /// <summary>
        /// 双击选中数据事件
        /// </summary>
        /// <param name="o"></param>
        public void DataGridMouseDoubleClick(object o)
        {
            //EditItemData = SelecTimeDataViewData;

            //this.Title = SelecTimeDataViewData.title;
            //this.Detail = SelecTimeDataViewData.detail;
            //this.Begin_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().begin_time);
            //this.End_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().end_time);
            //this.Type = SelecTimeDataViewData.type;
            //this.WorkDateTime = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().work_date);
            //this.Status = SelecTimeDataViewData.state;


            //this.AddVisibility = Visibility.Collapsed;
            //this.EditVisibility = Visibility.Visible;
        }

        /// <summary>
        /// 清除输入框
        /// </summary>
        /// <param name="o"></param>
        public void CleanData(object o)
        {
            this.Title = "";
            this.Detail = "";

            this.AddVisibility = Visibility.Visible;
            this.EditVisibility = Visibility.Collapsed;

            this.WorkDateTime = DateTime.Now;
            this.Begin_time = DateTime.Now;
            this.End_time = DateTime.Now;

            this.Status = StatusCollection.First();
        }

        /// <summary>
        /// 编辑选择的数据
        /// </summary>
        /// <param name="o"></param>
        public void EditData(object o)
        {
            //// update  data 
            //EditItemData.title = Title;
            //EditItemData.detail = Detail;
            //EditItemData.work_date = (WorkDateTime);
            //EditItemData.type = this.Type;
            //EditItemData.state = this.Status;
            //EditItemData.begin_time = Common.GetTimeSecond(this.Begin_time);
            //EditItemData.end_time = Common.GetTimeSecond(this.End_time);
            //EditItemData.spend =
            //    (long.Parse(Common.GetTimeSecond(this.End_time)) - (long.Parse(Common.GetTimeSecond(this.Begin_time))))
            //    .ToString();

            //for (int i = 0; i < DataItems.Count(); i++)
            //{
            //    if (EditItemData.GetID() == DataItems[i].GetID())
            //    {
            //        DataItems[i] = EditItemData;
            //    }
            //}
            //var result = DialogHost.Show(new LoadingDialog(), "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            //{
            //    // 获取编辑数据
            //    WorkTimeData postWorkTimeData = new WorkTimeData()
            //    {
            //        __ID__ = EditItemData.GetID(),
            //        work_date = Common.GetTimeSecond(WorkDateTime),
            //        title = this.Title,
            //        detail = this.Detail,
            //        type = this.Type,
            //        state = this.Status,
            //        begin_time = Common.GetTimeSecond(this.Begin_time),
            //        end_time = Common.GetTimeSecond(this.End_time),
            //        spend = (long.Parse(Common.GetTimeSecond(this.End_time)) - (long.Parse(Common.GetTimeSecond(this.Begin_time)))).ToString()
            //    };

            //    string temp = NetHelper.GETProperties(postWorkTimeData);

            //    string addUrl = "https://api.bobdong.cn/time_manager/data/update?access_token=" + MainStaticData.AccessToken;

            //    var datas = NetHelper.HttpCall(addUrl, temp, HttpEnum.Post);

            //    var returnData = JsonHelper.Deserialize<ReturnData<WorkTimeData>>(datas);


            //    ThreadStart start = delegate ()
            //    {

            //        Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
            //        {
            //            args.Session.Close(false);
            //            if (returnData.code != 0)
            //            {
            //                MessageShow(o, returnData.message);
            //            }
            //            else
            //            {
            //                CleanData(null);
            //                MessageShow(o, "Edit Success !");
            //            }


            //        });

            //    };
            //    new Thread(start).Start(); // 启动线程
            //});
        }

        /// <summary>
        /// 
        /// </summary>
        private void MessageShow(object viewdata, string Message)
        {
            //Task.Factory.StartNew(() =>
            //{

            //}).ContinueWith(t =>
            //{
            //    ((UserControls.Home)ThisContorler).SnackbarOne.MessageQueue.Enqueue(Message);
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

    }
}
