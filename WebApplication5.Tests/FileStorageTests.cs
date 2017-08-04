using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DevExpress.DashboardCommon;
using NUnit.Framework;
using WebApplication1.Services;

namespace WebApplication1.Tests
{
    [TestFixture]
    public class FileStorageTests
    {
        CrudDashboardStorage _storage;
        private readonly Dashboard _testDashboard = new Dashboard();

        [SetUp]
        public void SetUp()
        {     
            _storage = new CrudDashboardStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\EmptyStorage");


        }
        [TearDown]
        public void TearDown()
        {
            _storage = null;
            var dirInfo = new DirectoryInfo("C:/Users/Student17/Documents/Git/msmvc-devex/WebApplication1/App_Data/EmptyStorage");

            foreach (var file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
        [Test]
        public void AddDashboardTest()
        {
            var testDashboard = _testDashboard;
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual("test", _storage.AddDashboard(testXmlDashboard, "test"), "Add test");
            Assert.AreEqual(1, _storage.GetAvailableDashboardsInfo().Count(), "Dashboard wasn't added");
            Assert.AreEqual("dashboard_1", _storage.AddDashboard(testXmlDashboard, "dashboard_1///***??"), "Wrong chars aren't removed");
        }
        [Test]
        public void DashboardStorageFill()
        {
            var testDashboard = _testDashboard;
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual("test", _storage.AddDashboard(testXmlDashboard, "test"), "Add test");

            for (var i = 1; i <= 1024; i++)
            {
                Assert.AreEqual("test" + i, _storage.AddDashboard(testXmlDashboard, "test"), "Add test");
            }
        }


        [Test]
        public void EmptyBaseTest()
        {
            Assert.IsInstanceOf<ArgumentException>(
                Assert.Catch(() => _storage.LoadDashboard("")),
                "Null dashboardID Load test");

            Assert.IsInstanceOf<FileNotFoundException>(
                Assert.Catch(() => _storage.LoadDashboard("idThatNotExist")),
                "Wrong dashboardID Load test");

            Assert.AreEqual(0, _storage.GetAvailableDashboardsInfo().Count(), "GetAvaibleDashboards test");

            Assert.IsInstanceOf<FileNotFoundException>(
               Assert.Catch(() => _storage.CloneDashboard("idThatNotExist")),
               "Wrong dashboardID Clone test");

            Assert.IsInstanceOf<FileNotFoundException>(
               Assert.Catch(() => _storage.DeleteDashboard("idThatNotExist")),
               "Wrong dashboardID Delete test");

        }

        [Test]
        public void CloneTest()
        {
            var testDashboard = _testDashboard;
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual("test", _storage.AddDashboard(testXmlDashboard, "test"), "Add test");

            Assert.AreEqual("test1", _storage.CloneDashboard("test"), "Correct ID test");
            Assert.AreEqual("testName", _storage.CloneDashboard("test", "testName"), "Correct ID, correct Name test");

            Assert.IsInstanceOf<ArgumentException>(
                Assert.Catch( () => _storage.CloneDashboard("") ), 
                "dashboardID cant be null");

            Assert.IsInstanceOf<ArgumentException>(
                Assert.Catch(() => _storage.CloneDashboard("", "testName")),
                "dashboardID cant be null");

            Assert.IsInstanceOf<FileNotFoundException>(
               Assert.Catch(() => _storage.CloneDashboard("idThatNotExist")),
               "No dashboard with this id found");

            Assert.IsInstanceOf<FileNotFoundException>(
                Assert.Catch(() => _storage.CloneDashboard("idThatNotExist", "testName")),
                "No dashboard with this id found");
        }

    }
}
