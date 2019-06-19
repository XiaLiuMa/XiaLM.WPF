using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.Mods;

namespace WpfApp1.ViewModels
{
    #region 圆饼图标
    public class 环形图表1VM : INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }

        public 环形图表1VM()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Chrome",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 环形图表2VM : INotifyPropertyChanged
    {
        public Func<ChartPoint, string> PointLabel { get; set; }

        public 环形图表2VM()
        {
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 环形图表3VM : INotifyPropertyChanged
    {
        private SeriesCollection _series;
        public SeriesCollection Series
        {
            get { return _series; }
            set
            {
                _series = value;
                NotifyPropertyChanged("Series");
            }
        }
        public DropDownCommand SliceClickCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public Func<double, string> LabFormatter { get; set; }

        public 环形图表3VM()
        {
            var navigation = new List<SeriesCollection>();
            var initialValues = DataProvider.Values.ToArray();

            Series = GroupSeriesByTheshold(content: initialValues, threshold: initialValues.Max() * .7);

            SliceClickCommand = new DropDownCommand(dropDownPoint =>
            {
                //if the point has no content to display...
                if (dropDownPoint.Content.Length == 1) return;

                navigation.Add(Series.Select(x => new PieSeries
                {
                    Values = x.Values,
                    Title = x.Title,
                    Fill = ((Series)x).Fill
                }).AsSeriesCollection());

                Series = GroupSeriesByTheshold(content: dropDownPoint.Content, threshold: dropDownPoint.Content.Max() * .7);
            });

            GoBackCommand = new RelayCommand(() =>
            {
                if (!navigation.Any()) return;
                var previous = navigation.Last();
                if (previous == null) return;
                Series = previous;
                navigation.Remove(previous);
            });

            LabFormatter = x => x.ToString("N1");
        }

        private static SeriesCollection GroupSeriesByTheshold(double[] content, double threshold)
        {
            var series = content
                .Where(x => x > threshold)
                .Select((value, index) => new PieSeries
                {
                    Values = new ChartValues<DropDownPoint>
                    {
                        new DropDownPoint {Content = new[] {value}}
                    },
                    Title = "Series " + index
                }).AsSeriesCollection();

            var thresholdSeries = new PieSeries
            {
                Values = new ChartValues<DropDownPoint>
                {
                    new DropDownPoint
                    {
                        Content = content.Where(x => x < threshold).ToArray()
                    }
                },
                Title = "DropDown Slice",
                Fill = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                PushOut = 5
            };
            series.Add(thresholdSeries);

            return series;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    #region 环形图表3VM扩展
    public static class DataProvider
    {
        public static IEnumerable<double> Values
        {
            get
            {
                var r = new Random();

                for (var i = 0; i < 30; i++)
                {
                    yield return r.NextDouble() * 100;
                }
            }
        }
    }

    public class DropDownCommand : ICommand
    {
        private Action<DropDownPoint> _action;

        public DropDownCommand(Action<DropDownPoint> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            var chartPoint = (ChartPoint)parameter;
            var dropDownPoint = (DropDownPoint)chartPoint.Instance;

            _action(dropDownPoint);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

    public class DropDownPoint
    {
        static DropDownPoint()
        {
            //In this case we are plotting our own point to have 
            //more control over the current plot
            //configuring a custom type is quite simple

            //first we define a mapper
            var mapper = Mappers.Pie<DropDownPoint>()
                .Value(x => x.Value);//use the value property in the plot

            //then we save the mapper globally, there are many ways
            //so configure a series, for more info see:
            //https://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration
            Charting.For<DropDownPoint>(mapper);
        }

        /// <summary>
        /// Gets or sets the value to plot
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value
        {
            get { return Content.Sum(); }
        }

        /// <summary>
        /// Gets or sets the content, all the values that represent this point
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public double[] Content { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public double Title { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private Action _action;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
    #endregion

    public class 环形图表4VM : INotifyPropertyChanged
    {
        private double _angularGaugeValue;
        public double AngularGaugeValue
        {
            get { return _angularGaugeValue; }
            set
            {
                _angularGaugeValue = value;
                NotifyPropertyChanged("AngularGaugeValue");
            }
        }

        public 环形图表4VM()
        {
            AngularGaugeValue = 50;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 环形图表5VM : INotifyPropertyChanged
    {
        public Func<double, string> LabFormatter { get; set; }

        public 环形图表5VM()
        {
            LabFormatter = x => x.ToString("P");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    #endregion

    #region 线形图表
    public class 线形图表1VM : INotifyPropertyChanged
    {
        public Func<ChartPoint, string> LabelPoint { get; set; }

        public 线形图表1VM()
        {
            LabelPoint = x => "A long label so it overflows " + x;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 线形图表2VM : INotifyPropertyChanged
    {
        private double _xSection;

        public double XSection
        {
            get { return _xSection; }
            set
            {
                _xSection = value;
                NotifyPropertyChanged("XSection");
            }
        }

        private double _ySection;
        public double YSection
        {
            get { return _ySection; }
            set
            {
                _ySection = value;
                NotifyPropertyChanged("YSection");
            }
        }

        public 线形图表2VM()
        {
            XSection = 5;
            YSection = 5;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 线形图表3VM : INotifyPropertyChanged
    {
        public ChartValues<float> Values { get; set; }

        public 线形图表3VM()
        {
            Values = Values = new ChartValues<float>
            {
                3,
                4,
                6,
                3,
                2,
                6
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 线形图表4VM : INotifyPropertyChanged
    {
        public ChartValues<double> Values { get; set; }

        public 线形图表4VM()
        {
            Values = new ChartValues<double> { 150, 375, 420, 500, 160, 140 };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    #endregion

    #region 方形图表
    public class 方形图表1VM : INotifyPropertyChanged
    {
        private double _xPointer;
        public double XPointer
        {
            get { return _xPointer; }
            set
            {
                _xPointer = value;
                NotifyPropertyChanged("XPointer");
            }
        }

        private double _yPointer;
        public double YPointer
        {
            get { return _yPointer; }
            set
            {
                _yPointer = value;
                NotifyPropertyChanged("YPointer");
            }
        }
        public Func<double, string> Formatter { get; set; }

        public 方形图表1VM()
        {
            //lets initialize in an invisible location
            XPointer = -5;
            YPointer = -5;

            //the formatter or labels property is shared 
            Formatter = x => x.ToString("N2");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 方形图表2VM : INotifyPropertyChanged
    {
        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }

        public 方形图表2VM()
        {
            Values1 = new ChartValues<double> { 3, 5, 2, 6, 2, 7, 1 };
            Values2 = new ChartValues<double> { 6, 2, 6, 3, 2, 7, 2 };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 方形图表3VM : INotifyPropertyChanged
    {
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public 方形图表3VM()
        {
            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new RowSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    #endregion

    #region 其它图表
    public class 其它图表1VM : INotifyPropertyChanged
    {
        public ChartValues<ObservablePoint> ValuesA { get; set; }
        public ChartValues<ObservablePoint> ValuesB { get; set; }
        public ChartValues<ObservablePoint> ValuesC { get; set; }

        public 其它图表1VM()
        {
            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC = new ChartValues<ObservablePoint>();

            for (var i = 0; i < 20; i++)
            {
                ValuesA.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesB.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesC.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 其它图表2VM : INotifyPropertyChanged
    {
        public string[] Days { get; set; }
        public string[] SalesMan { get; set; }
        public ChartValues<HeatPoint> Values { get; set; }

        public 其它图表2VM()
        {
            Days = new[]
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };

            SalesMan = new[]
            {
                "Jeremy Swanson",
                "Lorena Hoffman",
                "Robyn Williamson",
                "Carole Haynes",
                "Essie Nelson"
            };

            var r = new Random();

            Values = new ChartValues<HeatPoint>
            {
                //X means sales man
                //Y is the day

                //"Jeremy Swanson"
                new HeatPoint(0, 0, r.Next(0, 10)),
                new HeatPoint(0, 1, r.Next(0, 10)),
                new HeatPoint(0, 2, r.Next(0, 10)),
                new HeatPoint(0, 3, r.Next(0, 10)),
                new HeatPoint(0, 4, r.Next(0, 10)),
                new HeatPoint(0, 5, r.Next(0, 10)),
                new HeatPoint(0, 6, r.Next(0, 10)),

                //"Lorena Hoffman"
                new HeatPoint(1, 0, r.Next(0, 10)),
                new HeatPoint(1, 1, r.Next(0, 10)),
                new HeatPoint(1, 2, r.Next(0, 10)),
                new HeatPoint(1, 3, r.Next(0, 10)),
                new HeatPoint(1, 4, r.Next(0, 10)),
                new HeatPoint(1, 5, r.Next(0, 10)),
                new HeatPoint(1, 6, r.Next(0, 10)),

                //"Robyn Williamson"
                new HeatPoint(2, 0, r.Next(0, 10)),
                new HeatPoint(2, 1, r.Next(0, 10)),
                new HeatPoint(2, 2, r.Next(0, 10)),
                new HeatPoint(2, 3, r.Next(0, 10)),
                new HeatPoint(2, 4, r.Next(0, 10)),
                new HeatPoint(2, 5, r.Next(0, 10)),
                new HeatPoint(2, 6, r.Next(0, 10)),

                //"Carole Haynes"
                new HeatPoint(3, 0, r.Next(0, 10)),
                new HeatPoint(3, 1, r.Next(0, 10)),
                new HeatPoint(3, 2, r.Next(0, 10)),
                new HeatPoint(3, 3, r.Next(0, 10)),
                new HeatPoint(3, 4, r.Next(0, 10)),
                new HeatPoint(3, 5, r.Next(0, 10)),
                new HeatPoint(3, 6, r.Next(0, 10)),

                //"Essie Nelson"
                new HeatPoint(4, 0, r.Next(0, 10)),
                new HeatPoint(4, 1, r.Next(0, 10)),
                new HeatPoint(4, 2, r.Next(0, 10)),
                new HeatPoint(4, 3, r.Next(0, 10)),
                new HeatPoint(4, 4, r.Next(0, 10)),
                new HeatPoint(4, 5, r.Next(0, 10)),
                new HeatPoint(4, 6, r.Next(0, 10))
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class 其它图表3VM : INotifyPropertyChanged
    {
        private DateTime _initialDateTime;
        public DateTime InitialDateTime
        {
            get { return _initialDateTime; }
            set
            {
                _initialDateTime = value;
                NotifyPropertyChanged("InitialDateTime");
            }
        }

        private PeriodUnits _period = PeriodUnits.Days;
        public PeriodUnits Period
        {
            get { return _period; }
            set
            {
                _period = value;
                NotifyPropertyChanged("Period");
            }
        }

        private IAxisWindow _selectedWindow;
        public IAxisWindow SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                _selectedWindow = value;
                NotifyPropertyChanged("SelectedWindow");
            }
        }

        public SeriesCollection SeriesCollection { get; set; }

        public 其它图表3VM()
        {
            var now = DateTime.UtcNow;
            InitialDateTime = new DateTime(now.Year, now.Month, now.Day);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6, 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6  },
                },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    #endregion
}
