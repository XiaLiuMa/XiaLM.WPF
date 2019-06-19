using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// HeatingArea1Page.xaml 的交互逻辑
    /// </summary>
    public partial class HeatingArea1Page : Page
    {
        public Dictionary<string, double> Values { get; set; }
        public Dictionary<string, string> LanguagePack { get; set; }

        public HeatingArea1Page()
        {
            InitializeComponent();

            var r = new Random();
            Values = new Dictionary<string, double>();
            Values["MX"] = r.Next(0, 100);
            Values["CA"] = r.Next(0, 100);
            Values["US"] = r.Next(0, 100);
            Values["IN"] = r.Next(0, 100);
            Values["CN"] = r.Next(0, 100);
            Values["JP"] = r.Next(0, 100);
            Values["BR"] = r.Next(0, 100);
            Values["DE"] = r.Next(0, 100);
            Values["FR"] = r.Next(0, 100);
            Values["GB"] = r.Next(0, 100);
            LanguagePack = new Dictionary<string, string>();
            LanguagePack["MX"] = "México"; // change the language if necessary
            DataContext = this;
        }
    }
}
