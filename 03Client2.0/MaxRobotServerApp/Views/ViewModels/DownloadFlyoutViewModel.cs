using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels.Ext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class DownloadFlyoutViewModel : ViewModel
    {
        #region 属性
        /// <summary>
        /// 正在下载的列表
        /// </summary>
        private List<DownloadInfo> dlist1 = new List<DownloadInfo>();
        private ObservableCollection<DownloadInfo> _fakeSoruce1;

        public ObservableCollection<DownloadInfo> FakeSource1
        {
            get
            {
                return _fakeSoruce1;
            }
            set
            {
                if (_fakeSoruce1 != value)
                {
                    _fakeSoruce1 = value;
                    OnPropertyChanged("FakeSource1");
                }
            }
        }

        /// <summary>
        /// 已下载完成的列表
        /// </summary>
        private List<DownloadInfo> dlist2 = new List<DownloadInfo>();
        private ObservableCollection<DownloadInfo> _fakeSoruce2;

        public ObservableCollection<DownloadInfo> FakeSource2
        {
            get
            {
                return _fakeSoruce2;
            }
            set
            {
                if (_fakeSoruce2 != value)
                {
                    _fakeSoruce2 = value;
                    OnPropertyChanged("FakeSource2");
                }
            }
        }
        #endregion

        public DownloadFlyoutViewModel()
        {
            _fakeSoruce1 = new ObservableCollection<DownloadInfo>();
            _fakeSoruce2 = new ObservableCollection<DownloadInfo>();
            List1RefreshDataGrid();
            List2RefreshDataGrid();
        }

        #region 正在下载列表
        /// <summary>
        /// 新增Item
        /// </summary>
        /// <param name="item"></param>
        public void List1AddItem(DownloadInfo item)
        {
            dlist1.Add(item);
            List1RefreshDataGrid();
        }

        /// <summary>
        /// 修改Item的进度值
        /// </summary>
        /// <param name="item"></param>
        public void ProgressSizeChanged(string id, double progresssize)
        {
            dlist1.Where(p => p.Id.Equals(id)).FirstOrDefault().ProgressSize = progresssize;
            dlist1.Where(p => p.Id.Equals(id)).FirstOrDefault().Percentage = (progresssize / 100.0).ToString("P2");
            List1RefreshDataGrid();
        }

        /// <summary>
        /// 修改Item的下载状态
        /// </summary>
        /// <param name="item"></param>
        public void DownloadStateChanged(string id, string state)
        {
            dlist1.Where(p => p.Id.Equals(id)).FirstOrDefault().State = state;
            if (state.Equals("下载成功"))
            {
                var obj = dlist1.Where(p => p.Id.Equals(id)).FirstOrDefault();
                dlist1.Remove(obj);

                //移至完成列表
                List2AddItem(obj);
                List2RefreshDataGrid();
            }
            List1RefreshDataGrid();
        }


        /// <summary>
        /// 删除Items
        /// </summary>
        /// <param name="item"></param>
        public void List1DeleteItem(string[] ids)
        {
            foreach (var id in ids)
            {
                dlist1.Remove(dlist1.Where(p => p.Id.Equals(id)).FirstOrDefault());
            }
            List1RefreshDataGrid();
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void List1RefreshDataGrid()
        {
            FakeSource1.Clear();
            FakeSource1.AddRange(dlist1);
        }
        #endregion


        #region 下载完成列表
        /// <summary>
        /// 新增Item
        /// </summary>
        /// <param name="item"></param>
        public void List2AddItem(DownloadInfo item)
        {
            dlist2.Add(item);
            List2RefreshDataGrid();
        }

        /// <summary>
        /// 删除Items
        /// </summary>
        /// <param name="item"></param>
        public void List2DeleteItem(string[] ids)
        {
            foreach (var id in ids)
            {
                dlist2.Remove(dlist2.Where(p => p.Id.Equals(id)).FirstOrDefault());
            }
            List2RefreshDataGrid();
        }

        /// <summary>
        /// 清空Items
        /// </summary>
        /// <param name="item"></param>
        public void List2ClearItem()
        {
            dlist2.Clear();
            List2RefreshDataGrid();
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void List2RefreshDataGrid()
        {
            FakeSource2.Clear();
            FakeSource2.AddRange(dlist2);
        } 
        #endregion
    }
}
