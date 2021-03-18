using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi;

namespace YiJingFramework.References.Zhouyi.Tests
{
    [TestClass()]
    public class ZhouyiTests
    {
        [TestMethod()]
        public void ZhouyiTest()
        {
            _ = new Zhouyi();
            _ = new Zhouyi("abc.json");
            using var fileStream = new FileStream("abc.json", FileMode.Open);
            _ = new Zhouyi(fileStream);
        }

        private readonly Zhouyi yijing = new Zhouyi();
        [TestMethod()]
        public void GetHexagramTest()
        {
            var hex = this.yijing.GetHexagram(new Core.Painting(
                 Core.LineAttribute.Yang, Core.LineAttribute.Yang, Core.LineAttribute.Yin,
                 Core.LineAttribute.Yang, Core.LineAttribute.Yang, Core.LineAttribute.Yin));
            Assert.AreEqual("兑", hex.Name);
            hex = this.yijing.GetHexagram(new Core.Painting(
                 Core.LineAttribute.Yang, Core.LineAttribute.Yin, Core.LineAttribute.Yang,
                 Core.LineAttribute.Yang, Core.LineAttribute.Yang, Core.LineAttribute.Yang));
            Assert.AreEqual("同人", hex.Name);
        }

        [TestMethod()]
        public void TryGetHexagramTest()
        {
            Assert.IsTrue(this.yijing.TryGetHexagram("同人", out var r));
            Assert.AreEqual("同人于野，亨。利涉大川，利君子贞。", r.Text);
        }

        [TestMethod()]
        public void GetTrigramTest()
        {
            var tri = this.yijing.GetTrigram(new Core.Painting(
                 Core.LineAttribute.Yin, Core.LineAttribute.Yin, Core.LineAttribute.Yin));
            Assert.AreEqual("坤", tri.Name);
        }

        [TestMethod()]
        public void TryGetTrigramTest()
        {
            Assert.IsTrue(this.yijing.TryGetTrigram("乾", out var r));
            Assert.AreEqual("天", r.Nature);
        }
    }
}