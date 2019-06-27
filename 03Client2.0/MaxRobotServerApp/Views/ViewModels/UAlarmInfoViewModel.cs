using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class UAlarmInfoViewModel : ViewModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        private string _imgUrl;
        public string ImgUrl
        {
            get
            {
                return _imgUrl;
            }
            set
            {
                if (_imgUrl != value)
                {
                    _imgUrl = value;
                    OnPropertyChanged("ImgUrl");
                }
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        private BitmapImage _bitmapImg;
        public BitmapImage BitmapImg
        {
            get
            {
                return _bitmapImg;
            }
            set
            {
                if (_bitmapImg != value)
                {
                    _bitmapImg = value;
                    OnPropertyChanged("BitmapImg");
                }
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private string _time;
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public UAlarmInfoViewModel(AlarmPush obj)
        {
            _name = obj.Name;
            _time = obj.Time.ToString("yyyy-MM-dd HH:mm:ss");
            if (!obj.ImgUrl.Contains($"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}"))
            {
                obj.ImgUrl = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{obj.ImgUrl}";
            }
            _imgUrl = obj.ImgUrl;
            _bitmapImg = new BitmapImage();
            _bitmapImg.BeginInit();
            _bitmapImg.UriSource = new Uri(obj.ImgUrl, UriKind.RelativeOrAbsolute);
            _bitmapImg.EndInit();
        }
    }
}
