using Microsoft.Owin;
using Microsoft.Owin.FileSystems;

namespace XiaLM.Owin.FileSever
{
    public class FileServerOptions
    {
        private Microsoft.Owin.StaticFiles.FileServerOptions _options;

        public Microsoft.Owin.StaticFiles.FileServerOptions Options { get { return _options; } }
        public FileServerOptions(string pathString, string fileSystem, bool enableDirectoryBrowsing)
        {
            _options = new Microsoft.Owin.StaticFiles.FileServerOptions();
            _options.RequestPath = string.IsNullOrEmpty(pathString) ? PathString.Empty : new PathString(pathString);
            _options.FileSystem = new PhysicalFileSystem(fileSystem);
            _options.EnableDirectoryBrowsing = enableDirectoryBrowsing;
        }
    }
}
