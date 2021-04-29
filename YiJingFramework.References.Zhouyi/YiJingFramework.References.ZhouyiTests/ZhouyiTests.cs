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
            _ = new Zhouyi("zhouyi/translation.json");
            using var fileStream = new FileStream("zhouyi/translation.json", FileMode.Open);
            _ = new Zhouyi(fileStream);
            _ = new Zhouyi();
        }

        private readonly Zhouyi yijing = new Zhouyi("zhouyi/translation.json");
        [TestMethod()]
        public void GetHexagramTest()
        {
            var hex = this.yijing.GetHexagram(new Core.Painting(
                 Core.YinYang.Yang, Core.YinYang.Yang, Core.YinYang.Yin,
                 Core.YinYang.Yang, Core.YinYang.Yang, Core.YinYang.Yin));
            Assert.AreEqual("兑", hex.Name);
            hex = this.yijing.GetHexagram(new Core.Painting(
                 Core.YinYang.Yang, Core.YinYang.Yin, Core.YinYang.Yang,
                 Core.YinYang.Yang, Core.YinYang.Yang, Core.YinYang.Yang));
            Assert.AreEqual("同人", hex.Name);
            hex = this.yijing.GetHexagram(1);
            Assert.AreEqual("乾", hex.Name);
            hex = this.yijing.GetHexagram(64);
            Assert.AreEqual("未济", hex.Name);
        }

        [TestMethod()]
        public void TryGetHexagramTest()
        {
            Assert.IsTrue(this.yijing.TryGetHexagram("同人", out var r));
            Assert.AreEqual("同人于野，亨。利涉大川。利君子贞。", r.Text);
        }

        [TestMethod()]
        public void GetTrigramTest()
        {
            var tri = this.yijing.GetTrigram(new Core.Painting(
                 Core.YinYang.Yin, Core.YinYang.Yin, Core.YinYang.Yin));
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