using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.WPF.T01.App.UserControls;

namespace XiaLM.WPF.T01.App.UiModels
{
    public class MainViewModel
    {
        public ContorllerItem[] Items { get; }

        public MainViewModel()
        {
            Items = new[]
            {
                new ContorllerItem("Home", new Home(){ DataContext = new HomeModel() }),
                new ContorllerItem("AddItem", new AddItem(){ DataContext = new AddItemModel()}),
            };
        }
    }
}
