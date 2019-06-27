using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class MapInfoList
    {
        public List<MapInfo> maps { get; set; }
    }
    public class MapInfo
    {
        public string mapid { get; set; }
        public string name { get; set; }

        public string enname { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int minzoom { get; set; }
        public int zoom { get; set; }
        public int maxzoom { get; set; }
        public float az { get; set; }
    }
    public class MapPositionList
    {

        public List<MapPosition> positions { get; set; }
    }
    public class MapPosition
    {
        public string posid { get; set; }
        public string name { get; set; }
        public string mapid { get; set; }
        public int type { get; set; }
        public int zoom { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float angle { get; set; }
    }
    public class MapLineList
    {
        public List<MapLine> paths { get; set; }
    }

    public class MapLine
    {
        public string lineid { get; set; }
        public string name { get; set; }
        public string task { get; set; }

        public string type { get; set; }

        public string mapid { get; set; }
        public string points { get; set; }
        public string linedata { get; set; }
        public string taskdata { get; set; }
    }
    public class RobotPointInfoMpd
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public double X { get; set; }

        public double Y { get; set; }
        public double Angle { get; set; }

        public string Mapid { get; set; }
    }
}
