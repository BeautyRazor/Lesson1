using System.Linq;
using System.Web.Hosting;
using System.Web.Routing;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Excel;
using WebApplication1.Services;

namespace WebApplication1
{
    public class DashboardConfig
    {
        private static CsvSourceOptions Config()
        {
            return new CsvSourceOptions
            {
                NewlineType = CsvNewlineType.LF,
                UseFirstRowAsHeader = true,
                ValueSeparator = ','
            };
        }

        private static void RestoreXmlPath()
        {
            var storage = DashboardConfigurator.Default.DashboardStorage;
            DashboardConfigurator.Default.DataLoading += Default_DataLoading;
            DashboardConfigurator.Default.ConfigureDataConnection += Default_ConfigureDataConnection;

            //for (var i = 0; i < storage.GetAvailableDashboardsInfo().Count(); i++)
            //{
            //    var dashboardId = storage.GetAvailableDashboardsInfo().ToArray()[i].ID;
            //    var xmlDashboard = storage.LoadDashboard(dashboardId);

            //    var dashboard = new Dashboard();
            //    dashboard.LoadFromXDocument(xmlDashboard);

            //    dashboard.DataSources[0] = new DashboardExcelDataSource
            //    {
            //        FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"),
            //        SourceOptions = Config()
            //    };

            //    storage.SaveDashboard(dashboardId, dashboard.SaveToXDocument());
            //}
        }

        private static void Default_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)

        {
            e.ConnectionParameters = new ExcelDataSourceConnectionParameters(HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"));
           
        }

        private static void Default_DataLoading(object sender, DataLoadingWebEventArgs e)
        {
        }

        public static void RegisterService(RouteCollection routes)
        {
            routes.MapDashboardRoute("asd");

            var excelDataSource = new ExcelDataSource
            {
                FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"),
                SourceOptions = Config()
            };
            excelDataSource.Fill();


            var dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());

            DashboardConfigurator.Default.SetDashboardStorage(new CrudDashboardStorage(HostingEnvironment.MapPath(@"~/App_Data/Dashboards/")));
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);

            RestoreXmlPath();
        }
    }
}