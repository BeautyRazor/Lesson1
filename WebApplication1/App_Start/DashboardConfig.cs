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
            //DashboardConfigurator.Default.DataLoading += Default_DataLoading;
            DashboardConfigurator.Default.ConfigureDataConnection += Default_ConfigureDataConnection;
        }

        private static void Default_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
        {
            e.ConnectionParameters = new ExcelDataSourceConnectionParameters(HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"));
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

            var excelDataSource1 = new ExcelDataSource
            {
                FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/GLOBCSES.Final20170714.csv"),
                SourceOptions = Config()
            };
            excelDataSource1.Fill();

            var excelDataSource2 = new ExcelDataSource
            {
                FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/meteorite-landings.csv"),
                SourceOptions = Config()
            };
            excelDataSource2.Fill();

            var dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());
            dataSourceStorage.RegisterDataSource("excelDataSource1", excelDataSource1.SaveToXml());
            dataSourceStorage.RegisterDataSource("excelDataSource2", excelDataSource2.SaveToXml());

            DashboardConfigurator.Default.SetDashboardStorage(new CrudDashboardStorage(HostingEnvironment.MapPath(@"~/App_Data/Dashboards/")));
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);

            //RestoreXmlPath();
        }
    }
}