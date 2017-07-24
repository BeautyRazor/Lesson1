using System.Web.Hosting;
using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
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
        }
    }
}