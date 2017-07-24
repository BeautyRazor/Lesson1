using System.Collections.Generic;
using DevExpress.DashboardWeb;

namespace WebApplication1.Models
{
    public class Preview
    {
        public List<DashboardInfo> Dashboards { get; set; } 

        public string DashboardImagePath { get; } = "/Content/img/";

        public string NewDashboardName { get; set; }
    }
}