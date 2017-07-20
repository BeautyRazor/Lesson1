using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Excel;
using System.Web.Hosting;
using System.Linq;
using System.IO;
using DevExpress.DashboardCommon;
using System.Drawing;
using WebApplication1.Services;

namespace WebApplication1
{
    public class DashboardConfig
    {
        private static CsvSourceOptions Config()
        {
            return new CsvSourceOptions()
            {
                NewlineType = CsvNewlineType.LF,
                UseFirstRowAsHeader = true,
                ValueSeparator = ','
            };
        }

        public static void RegisterService(RouteCollection routes)
        {
            routes.MapDashboardRoute("asd");

            ExcelDataSource excelDataSource = new ExcelDataSource()
            {
                FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"),
                SourceOptions = Config()
            };
            excelDataSource.Fill();


            var dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());

            DashboardConfigurator.Default.SetDashboardStorage(new CRUDDashboardStorage(HostingEnvironment.MapPath(@"~/App_Data/Dashboards/")));
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
        }
    }
}