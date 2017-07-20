using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    interface ICRUDDashboardStorage : IEditableDashboardStorage
    {
        string DeleteDashboard(string dashboardID);
        string CloneDashboard(string dashboardID);
        string CloneDashboard(string dashboardID, string newDashboardName);
    }
}
