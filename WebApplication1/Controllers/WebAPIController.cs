using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using WebApplication1.Services;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace WebApplication1.Controllers
{
    public class WebAPIController : ApiController
    {

        public class Item
        {
            public string ID { get; set; }
            public string title { get; set; }
        }

        [System.Web.Http.HttpPost]
        public string Post(string id)
        {
            var data = new Item()
            {
                ID = Dashboard.Add(id),
                title = id
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public string Delete(string id)
        {
            var data = new Item()
            {
                ID = Dashboard.Delete(id)
            };

            return new JavaScriptSerializer().Serialize(data);
        }

        [System.Web.Http.HttpPost]
        public string Clone(string id, string name = "default")
        {
            var data = new Item(); 

            if (name == "default")
            {
                data.ID = Dashboard.Clone(id);
            }
            else
            {
                data.ID = Dashboard.Clone(id, name);
                data.title = name;
            }
            
            return new JavaScriptSerializer().Serialize(data);
        }
    }
}
