using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Excel;
using System.Web.Hosting;
using System.Linq;
using System.IO;
using DevExpress.DashboardCommon;
using System.Drawing;

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


            string dashboardsPath = @"~\App_Data";
            string thumbnailsPath = @"~\Content\img";

            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);
            var dashboards = (storage as IDashboardStorage).GetAvailableDashboardsInfo().ToList();
            ASPxDashboardExporter exporter = new ASPxDashboardExporter(DashboardConfigurator.Default);

            string path = HostingEnvironment.MapPath(thumbnailsPath);

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            for (int i = 0; i < dashboards.Count; i++)
            {
                string fullPath = string.Format(@"{0}\{1}.png", path, dashboards[i].ID);
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                //using (MemoryStream fs = new MemoryStream())
                    exporter.ExportToImage(dashboards[i].ID, fs, new Size(500, 400), null, new DashboardImageExportOptions()
                    {
                        Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Png
                    });
            }

        }
    }
}
/*
  public partial class Default : System.Web.UI.Page {
        string dashboardsPath = @"~/App_Data/";
        string thumbnailsPath = @"~/Content/img";


        protected void Page_Load(object sender, EventArgs e) {
            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);
            ASPxDashboard1.SetDashboardStorage(storage);
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource(DataSourceGenerator.CreateNWindDataSource().SaveToXml());
            ASPxDashboard1.SetDataSourceStorage(dataSourceStorage);
        }

        protected void ImageSlider_ItemDataBound(object source, ImageSliderItemEventArgs e) {
            e.Item.Name = System.IO.Path.GetFileNameWithoutExtension(e.Item.ImageUrl);
        }

        protected void ASPxDashboard1_DataLoading(object sender, DataLoadingWebEventArgs e) {
            DashboardMainDemo.DataLoader.LoadData(e);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e) {
            DashboardFileStorage storage = new DashboardFileStorage(dashboardsPath);
            var dashboards = (storage as IDashboardStorage).GetAvailableDashboardsInfo().ToList();

            ASPxDashboardExporter exporter = new ASPxDashboardExporter(ASPxDashboard1);
            string path = Server.MapPath(thumbnailsPath);

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles()) {
                file.Delete();
            }

            for (int i = 0; i < dashboards.Count; i++) {
                string fullPath = string.Format(@"{0}\{1}.png", path, dashboards[i].ID);
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
                    exporter.ExportToImage(dashboards[i].ID, fs, new Size(1000, 1000), null, new DashboardImageExportOptions() {
                        Format = DevExpress.DashboardCommon.DashboardExportImageFormat.Png
                    });
            }

            ImageSlider.ImageSourceFolder = thumbnailsPath;
        }
    }
 */
