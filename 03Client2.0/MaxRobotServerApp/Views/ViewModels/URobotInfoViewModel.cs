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
    public class URobotInfoViewModel : ViewModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _robotName;
        public string RobotName
        {
            get
            {
                return _robotName;
            }
            set
            {
                if (_robotName != value)
                {
                    _robotName = value;
                    OnPropertyChanged("RobotName");
                }
            }
        }

        /// <summary>
        /// 标识
        /// </summary>
        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (_tag != value)
                {
                    _tag = value;
                    OnPropertyChanged("Tag");
                }
            }
        }

        /// <summary>
        /// 任务名
        /// </summary>
        private string _rwName;
        public string RwName
        {
            get
            {
                return _rwName;
            }
            set
            {
                if (_rwName != value)
                {
                    _rwName = value;
                    OnPropertyChanged("RwName");
                }
            }
        }

        /// <summary>
        /// 电量
        /// </summary>
        private string _power;
        public string Power
        {
            get
            {
                return _power;
            }
            set
            {
                if (_power != value)
                {
                    _power = value;
                    OnPropertyChanged("Power");
                }
            }
        }

        /// <summary>
        /// (状态)无
        /// </summary>
        private string _noState;
        public string NoState
        {
            get
            {
                return _noState;
            }
            set
            {
                if (_noState != value)
                {
                    _noState = value;
                    OnPropertyChanged("NoState");
                }
            }
        }

        /// <summary>
        /// (状态)正常
        /// </summary>
        private string _normalState;
        public string NormalState
        {
            get
            {
                return _normalState;
            }
            set
            {
                if (_normalState != value)
                {
                    _normalState = value;
                    OnPropertyChanged("NormalState");
                }
            }
        }

        /// <summary>
        /// (状态)故障
        /// </summary>
        private string _errorState;
        public string ErrorState
        {
            get
            {
                return _errorState;
            }
            set
            {
                if (_errorState != value)
                {
                    _errorState = value;
                    OnPropertyChanged("ErrorState");
                }
            }
        }

        /// <summary>
        /// 故障列表
        /// </summary>
        private List<string> _errorList;
        public List<string> ErrorList
        {
            get
            {
                return _errorList;
            }
            set
            {
                if (_errorList != value)
                {
                    _errorList = value;
                    OnPropertyChanged("ErrorList");
                }
            }
        }

        /// <summary>
        /// 在线
        /// </summary>
        private string _online;
        public string Online
        {
            get
            {
                return _online;
            }
            set
            {
                if (_online != value)
                {
                    _online = value;
                    OnPropertyChanged("Online");
                }
            }
        }

        /// <summary>
        /// 不在线
        /// </summary>
        private string _offline;
        public string Offline
        {
            get
            {
                return _offline;
            }
            set
            {
                if (_offline != value)
                {
                    _offline = value;
                    OnPropertyChanged("Offline");
                }
            }
        }

        public URobotInfoViewModel(RobotInfo obj)
        {
            _robotName = obj.Name;
            _tag = obj.Tag;
            _online = obj.IsOnline ? "Visible" : "Hidden";
            _offline = obj.IsOnline ? "Hidden" : "Visible";
            _noState = "Visible";   //默认无状态
            _normalState = "Hidden";
            _errorState = "Hidden";
            _errorList = new List<string>();
        }


        /// <summary>
        /// 刷新机器人数据
        /// </summary>
        public void RefreshRobotInfo(RobotOnlineInfo obj)
        {
            Online = obj.IsOnline ? "Visible" : "Hidden";
            Offline = obj.IsOnline ? "Hidden" : "Visible";
            string tasks = "";
            if (obj.TaskIds != null && obj.TaskIds.Count > 0)
            {
                foreach (var item in obj.TaskIds)
                {
                    tasks += $"{item},";
                }
            }
            RwName = tasks.TrimEnd(',');
            Power = (obj.Power/100).ToString("P");    //转百分比字符串
            NoState = "Hidden";

            NormalState = (obj.DeviceStateIds != null && obj.DeviceStateIds.Count > 0) ? "Hidden" : "Visible";
            ErrorState = (obj.DeviceStateIds != null && obj.DeviceStateIds.Count > 0) ? "Visible" : "Hidden";
            ErrorList = (obj.DeviceStateIds != null && obj.DeviceStateIds.Count > 0)?obj.DeviceStateIds:new List<string>();
        }
    }
}
