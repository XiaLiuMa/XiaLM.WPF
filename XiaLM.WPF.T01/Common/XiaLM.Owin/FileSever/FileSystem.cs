using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Owin.FileSever
{
    public class FileSystem
    {
        private Microsoft.Owin.FileSystems.PhysicalFileSystem _physicalFileSystem;
        public Microsoft.Owin.FileSystems.PhysicalFileSystem PhysicalFileSystem
        {
            get
            {
                return _physicalFileSystem;
            }
        }
        public FileSystem(string root)
        {
            _physicalFileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem(root);
        }
    }
}
