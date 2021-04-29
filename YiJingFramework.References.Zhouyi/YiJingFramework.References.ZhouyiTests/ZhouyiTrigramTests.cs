using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi;

namespace YiJingFramework.References.Zhouyi.Tests
{
    [TestClass()]
    public class ZhouyiTrigramTests
    {
        private readonly Zhouyi yijing = new Zhouyi("zhouyi/translation.json");

        public ZhouyiTrigramTests()
        {
            this.qian = this.yijing.GetTrigram(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yang,
                Core.YinYang.Yang
            ));
            this.li = this.yijing.GetTrigram(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yin,
                Core.YinYang.Yang
            ));
            _ = this.yijing.TryGetTrigram("坤", out this.kun);
        }
        private readonly ZhouyiTrigram qian;
        private readonly ZhouyiTrigram li;
        private readonly ZhouyiTrigram kun;
        [TestMethod()]
        public void GetPaintingTest()
        {
            Assert.AreEqual(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yang,
                Core.YinYang.Yang
            ), this.qian.GetPainting());
            Assert.AreEqual(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yin,
                Core.YinYang.Yang
            ), this.li.GetPainting());
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsFalse(this.qian.Equals(this.li));
            Assert.IsFalse(this.qian.Equals(null));
            Assert.IsFalse(this.qian.Equals(new object()));
            Assert.IsTrue(this.li.Equals(new Zhouyi("zhouyi/translation.json").GetTrigram(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yin,
                Core.YinYang.Yang
            ))));
            Assert.IsTrue(this.li.Equals((object)new Zhouyi("zhouyi/translation.json").GetTrigram(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yin,
                Core.YinYang.Yang
            ))));
            Assert.IsTrue(this.qian.Equals((object)this.qian));
            Assert.IsTrue(this.qian == new Zhouyi("zhouyi/translation.json").GetTrigram(new Core.Painting(
                Core.YinYang.Yang,
                Core.YinYang.Yang,
                Core.YinYang.Yang
            )));
            Assert.IsFalse(this.qian == this.li);
            Assert.IsTrue(this.qian != this.li);
            Assert.IsFalse(this.qian == null);
            Assert.IsTrue(this.qian != null);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("乾", this.qian.ToString());
            Assert.AreEqual("离", this.li.ToString());
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(1, this.qian.GetHashCode());
            Assert.AreEqual(3, this.li.GetHashCode());
        }

        [TestMethod()]
        public void CompareToTest()
        {
            Assert.IsTrue(this.qian.CompareTo(this.li) < 0);
            Assert.IsTrue(this.qian.CompareTo(null) > 0);
            Assert.IsTrue(this.li.CompareTo(this.qian) > 0);
            Assert.IsTrue(this.qian.CompareTo(this.qian) == 0);
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            Assert.AreEqual("乾", this.qian.Name);
            Assert.AreEqual(1, this.qian.Index);
            Assert.AreEqual("天", this.qian.Nature);
            Assert.AreEqual("离", this.li.Name);
            Assert.AreEqual(3, this.li.Index);
            Assert.AreEqual("火", this.li.Nature);
            Assert.AreEqual("坤", this.kun.Name);
            Assert.AreEqual(8, this.kun.Index);
            Assert.AreEqual("地", this.kun.Nature);
        }
    }
}