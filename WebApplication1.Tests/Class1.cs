using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Services;
using DevExpress.DashboardWeb;

namespace WebApplication1.Tests
{
    [TestFixture]
    public class IdGeneratorTests
    {
        [Test]
        public void EmptyBaseTest()
        {
            MyDashboardFileStorage storage = new MyDashboardFileStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\Dashboards");
            Assert.AreEqual("dashboard1_1", storage.GenerateID("dashboard1"), "Fist test");
        }

        [Test]
        public void WrongName()
        {
            var storage = new MyDashboardFileStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\Dashboards");
            Assert.AreEqual("dashboard-1", storage.ReplaceWrong("dashboard_1"), "Wrong chars");
        }

        [Test]
        public void Add()
        {
            //var storage = new MyDashboardFileStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\Dashboards");
           
            var ds = new MyDashboardFileStorage(@"C:\Users\Student17\Documents\Git\msmvc-devex\WebApplication1\App_Data\Dashboards");
            Assert.AreEqual("", ds.AddDashboard(), "Wrong chars");
        }
    }
}
