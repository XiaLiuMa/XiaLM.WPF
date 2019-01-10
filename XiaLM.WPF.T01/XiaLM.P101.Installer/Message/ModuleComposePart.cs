using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Installer.Message
{
    public class ModuleComposePart
    {
        private CompositionContainer container;
        private static ModuleComposePart moduleComposePart;
        private readonly static object lockObj = new object();
        public static ModuleComposePart GetInstance()
        {
            if (moduleComposePart == null)
            {
                lock (lockObj)
                {
                    if (moduleComposePart == null)
                    {
                        moduleComposePart = new ModuleComposePart();
                    }
                }
            }
            return moduleComposePart;
        }

        public void Init()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            container = new CompositionContainer(catalog);
        }
        public void ComposePart(object obj)
        {
            try
            {
                container.ComposeParts(obj);
            }
            catch (ChangeRejectedException ex)
            {
                Trace.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
