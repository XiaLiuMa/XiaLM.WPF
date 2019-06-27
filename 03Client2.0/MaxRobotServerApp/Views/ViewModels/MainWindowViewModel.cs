using MaxRobotServerApp.Extensions.Comm;
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
    public class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _uname;
        public string Uname
        {
            get
            {
                return _uname;
            }
            set
            {
                if (_uname != value)
                {
                    _uname = value;
                    OnPropertyChanged("Uname");
                }
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        private string _utype;
        public string Utype
        {
            get
            {
                return _utype;
            }
            set
            {
                if (_utype != value)
                {
                    _utype = value;
                    OnPropertyChanged("Utrpe");
                }
            }
        }

        public MainWindowViewModel()
        {
            _uname = UserManager.GetInstance().User.Name;
            _utype = UserManager.GetInstance().User.UserType.Equals(UserType.Admin)? "管理员":"普通用户";
        }
    }
}
