using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebApplication1.Models
{
    public class Preview
    {
        public List<DashboardInfo> Dashboards { get; set; } 

        public string DashboardImagePath { get; } = "/Content/img/";

        public string NewDashboardName { get; set; }
    }
}