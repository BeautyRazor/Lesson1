using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Services;
using DevExpress.DashboardWeb;
using DevExpress.DashboardCommon;
using System.IO;

namespace WebApplication1.Tests
{
    [TestFixture]
    public class IdGeneratorTests
    {
        CRUDDashboardStorage storage = null;

           [SetUp]
        public void SetUp() {
            
            storage = new CRUDDashboardStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\EmptyStorage");
        }
        [TearDown]
        public void TearDown()
        {
            storage = null;
            DirectoryInfo dirInfo = new DirectoryInfo("C:/Users/Student17/Documents/Git/msmvc-devex/WebApplication1/App_Data/EmptyStorage");

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
        [Test]
        public void AddDashboardTest()
        {
            var testDashboard = new Dashboard();
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual("test", storage.AddDashboard(testXmlDashboard, "test"), "Add test");
            Assert.AreEqual(1, storage.GetAvailableDashboardsInfo().Count(), "Dashboard wasn't added");
            Assert.AreEqual("dashboard_1", storage.AddDashboard(testXmlDashboard, "dashboard_1///***??"), "Wrong chars aren't removed");

            for(int i = 1; i <= 1024; i++)
            {
                Assert.AreEqual("test" + i.ToString(), storage.AddDashboard(testXmlDashboard, "test"), "Add test");
            }
        }

        [Test]
        public void EmptyBaseTest()
        {
            var testDashboard = new Dashboard();
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual(null, storage.LoadDashboard(null), "Pass null should return null");
            Assert.AreEqual(null, storage.LoadDashboard("test"), "Load test");
            Assert.AreEqual(0, storage.GetAvailableDashboardsInfo().Count(), "GetAvaibleDashboards test");
            Assert.AreEqual(null, storage.CloneDashboard("test"), "Clone test");
            Assert.AreEqual(null, storage.DeleteDashboard("test"), "Delete test");

        }

        [Test]
        public void CloneTest()
        {
            var testDashboard = new Dashboard();
            var testXmlDashboard = testDashboard.SaveToXDocument();

            Assert.AreEqual("test", storage.AddDashboard(testXmlDashboard, "test"), "Add test");

            Assert.AreEqual(null, storage.CloneDashboard(""), "Null ID test");
            Assert.AreEqual(null, storage.CloneDashboard("", "test"), "Null ID, correct Name test");
            Assert.AreEqual(null, storage.CloneDashboard("test", ""), "Correct ID, null Name test");
            Assert.AreEqual(null, storage.CloneDashboard("",""), "Null ID, null Name test");

            Assert.AreEqual("test1", storage.CloneDashboard("test"), "Correct ID test");
            Assert.AreEqual("test2", storage.CloneDashboard("test", "testName"), "Correct ID, correct Name test");

        }


    }
}
