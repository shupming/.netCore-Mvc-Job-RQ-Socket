using HospitalReport.Models.DataBase;
using HospitalReport.Service.Interface.System;
using HospitalReport.SqlSugar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;

namespace HospitalReport.Test
{
    [TestClass]
    public class UnitTest1 : TestInitializeTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var services = new ServiceCollection().BuildServiceProvider();
        }
    }
}
