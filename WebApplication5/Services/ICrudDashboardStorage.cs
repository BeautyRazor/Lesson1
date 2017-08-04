using DevExpress.DashboardWeb;

namespace WebApplication5.Services
{
    interface ICrudDashboardStorage : IEditableDashboardStorage
    {
        string DeleteDashboard(string dashboardId);
        string CloneDashboard(string dashboardId);
        string CloneDashboard(string dashboardId, string newDashboardName);
    }
}
