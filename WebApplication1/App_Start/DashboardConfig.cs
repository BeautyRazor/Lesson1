using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Excel;
using System.Web.Hosting;

namespace WebApplication1
{
    public class DashboardConfig
    {
        public static void RegisterService(RouteCollection routes)
        {
            routes.MapDashboardRoute();

            // Uncomment this line to save dashboards to the App_Data folder.
            //DashboardConfigurator.Default.SetDashboardStorage(new DashboardFileStorage(@"~/App_Data/"));

            // Uncomment these lines to create an in-memory storage of dashboard data sources. Use the DataSourceInMemoryStorage.RegisterDataSource
            // method to register the existing data source in the created storage.
            //var dataSourceStorage = new DataSourceInMemoryStorage();
            //DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);

            ExcelDataSource excelDataSource = new ExcelDataSource();
            excelDataSource.FileName = HostingEnvironment.MapPath(@"~/App_Data/Dashboards/sof16.csv");
            //ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings("SalesPerson", "A1:L2000");
            var options = new CsvSourceOptions();
            options.NewlineType = CsvNewlineType.LF;
            options.UseFirstRowAsHeader = true;
            options.ValueSeparator = ',';
            excelDataSource.SourceOptions = options;
            excelDataSource.Fill();


            var dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());

            //var aspx = new ASPxDashboard();
            //aspx.SetDataSourceStorage(dataSourceStorage);
            DashboardConfigurator.Default.SetDashboardStorage(new DashboardFileStorage(@"~/App_Data/"));
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
        }
    }
}