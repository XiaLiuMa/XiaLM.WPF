using System.Windows;

namespace XiaLM.WPF.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.loadingGif.StartAnimate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.loadingGif.StopAnimate();
        }

        
    }
}
