using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Routing;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Excel;
using WebApplication5.Services;
using System.Web;

namespace WebApplication5
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
            //var pathToDataSources = HostingEnvironment.MapPath(@"~/App_Data/Resources");
            //var file_list = Directory.GetFiles(pathToDataSources, "*.csv");

            //foreach (var file in file_list)
            //{
                DashboardConfigurator.Default.ConfigureDataConnection += Default_ConfigureDataConnection;
            //}

            //DashboardConfigurator.Default.DataLoading += Default_DataLoading;
        }

        private static void Default_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
        {
            var name = (ExcelDataSourceConnectionParameters)e.ConnectionParameters;
            var file = name.FileName;

            var fileName = Path.GetFileName(file);
            var pathToDataSources = HttpContext.Current.Server.MapPath(@"~/App_Data/Resources/");
            e.ConnectionParameters =
                    new ExcelDataSourceConnectionParameters(pathToDataSources + fileName);
            
        }

        public static void RegisterService(RouteCollection routes)
        {
            routes.MapDashboardRoute("asd");

            var pathToDataSources = HttpContext.Current.Server.MapPath(@"~/App_Data/Resources/");
            var file_list = Directory.GetFiles(pathToDataSources, "*.csv");

            var dataSourceStorage = new DataSourceInMemoryStorage();

            foreach (var file in file_list)
            {
                var excelDataSource = new ExcelDataSource
                {
                    FileName = file,
                    SourceOptions = Config()
                };
                excelDataSource.Fill();

                dataSourceStorage.RegisterDataSource("excelDataSource" + file, excelDataSource.SaveToXml());

            }

            //var excelDataSource3 = new ExcelDataSource
            //{
            //    FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/sof16.csv"),
            //    SourceOptions = Config()
            //};
            //excelDataSource3.Fill();

            //var excelDataSource1 = new ExcelDataSource
            //{
            //    FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/GLOBCSES.Final20170714.csv"),
            //    SourceOptions = Config()
            //};
            //excelDataSource1.Fill();

            //var excelDataSource2 = new ExcelDataSource
            //{
            //    FileName = HostingEnvironment.MapPath(@"~/App_Data/Resources/meteorite-landings.csv"),
            //    SourceOptions = Config()
            //};
            //excelDataSource2.Fill();

            ////var dataSourceStorage = new DataSourceInMemoryStorage();
            //dataSourceStorage.RegisterDataSource("excelDataSource1", excelDataSource1.SaveToXml());
            //dataSourceStorage.RegisterDataSource("excelDataSource2", excelDataSource2.SaveToXml());

            DashboardConfigurator.Default.SetDashboardStorage(new CrudDashboardStorage(HostingEnvironment.MapPath(@"~/App_Data/Dashboards/")));
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);

            RestoreXmlPath();
        }
    }
}