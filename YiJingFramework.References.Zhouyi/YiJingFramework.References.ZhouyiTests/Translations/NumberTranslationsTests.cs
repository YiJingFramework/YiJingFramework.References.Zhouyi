using Microsoft.VisualStudio.TestTools.UnitTesting;
using YiJingFramework.References.Zhouyi.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi.Translations.Tests
{
    [TestClass()]
    public class NumberTranslationsTests
    {
        [TestMethod()]
        public void GetCardinalTest()
        {
            var v = new ZhouyiJingSection().NumberTranslations;
            Assert.AreEqual("二十六", v.GetCardinal(26));
        }

        [TestMethod()]
        public void GetOrdinalTest()
        {
            var v = new ZhouyiJingSection().NumberTranslations;
            Assert.AreEqual("二十六", v.GetCardinal(26));
        }
    }
}