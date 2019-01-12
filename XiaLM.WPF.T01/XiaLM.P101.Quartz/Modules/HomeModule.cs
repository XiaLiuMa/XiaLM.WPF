using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.P101.Quartz.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/api", args => "Hello World, it's Nancy on .NET Core");
            Get("/", p => View["chat.html"]);  //返回视图
        }
    }


    public class PageModule : NancyModule
    {
        public PageModule()
        {
            Get("/person/{name}", args => new Person() { Name = args.name });
        }
    }


    public class Person
    {
        public string Name { get; set; }
    }
}
