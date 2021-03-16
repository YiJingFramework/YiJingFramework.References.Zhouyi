using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi.Tests
{
    [TestClass()]
    public class ZhouyiHexagramLineTests
    {
        private readonly Zhouyi yijing = new Zhouyi();

        public ZhouyiHexagramLineTests()
        {
            this.qian = this.yijing.GetHexagram(new Core.Painting(
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang
            ));
            this.weiJi = this.yijing.GetHexagram(new Core.Painting(
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang
            ));
            _ = this.yijing.TryGetHexagram("师", out this.shi);
        }
        private readonly ZhouyiHexagram qian;
        private readonly ZhouyiHexagram shi;
        private readonly ZhouyiHexagram weiJi;

        [TestMethod()]
        public void PropertiesTest()
        {
            var line = this.qian.GetLine(1);
            Assert.AreEqual(this.qian, line.From);
            Assert.AreEqual(Core.LineAttribute.Yang, line.LineAttribute);
            Assert.AreEqual(1, line.LineIndex);
            Assert.AreEqual("潜龙，勿用。", line.LineText);

            line = this.weiJi.FifthLine;
            Assert.AreEqual(this.yijing.GetHexagram(new Core.Painting(
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang
            )), line.From);
            Assert.AreEqual(Core.LineAttribute.Yin, line.LineAttribute);
            Assert.AreEqual(5, line.LineIndex);
            Assert.AreEqual("贞吉，无悔，君子之光，有孚，吉。", line.LineText);

            Assert.AreEqual("师出以律，否臧凶。", this.shi.FirstLine.LineText);
            Assert.AreEqual("在师中，吉无咎，王三锡命。", this.shi.SecondLine.LineText);
            Assert.AreEqual("师或舆尸，凶。", this.shi.ThirdLine.LineText);
            Assert.AreEqual("师左次，无咎。", this.shi.FourthLine.LineText);
            Assert.AreEqual("田有禽，利执言，无咎。长子帅师，弟子舆尸，贞凶。", this.shi.FifthLine.LineText);
            Assert.AreEqual("大君有命，开国承家，小人勿用。", this.shi.SixthLine.LineText);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("六五：田有禽，利执言，无咎。长子帅师，弟子舆尸，贞凶。",
                this.shi.FifthLine.ToString());
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsFalse(this.qian.GetLine(1).Equals(this.weiJi.GetLine(1)));
            Assert.IsFalse(this.qian.GetLine(1).Equals(null));
            Assert.IsFalse(this.qian.GetLine(1).Equals(new object()));
            Assert.IsTrue(this.shi.GetLine(1).Equals(new Zhouyi().GetHexagram(new Core.Painting(
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin
            )).GetLine(1)));
            Assert.IsTrue(this.shi.GetLine(1).Equals((object)new Zhouyi().GetHexagram(new Core.Painting(
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin,
                Core.LineAttribute.Yin
            )).GetLine(1)));
            Assert.IsTrue(this.qian.GetLine(2).Equals((object)this.qian.GetLine(2)));
            Assert.IsTrue(this.qian.GetLine(1) == new Zhouyi().GetHexagram(new Core.Painting(
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang,
                Core.LineAttribute.Yang
            )).GetLine(1));
            Assert.IsFalse(this.qian.GetLine(2) == this.shi.GetLine(2));
            Assert.IsTrue(this.qian.GetLine(2) != this.shi.GetLine(2));
            Assert.IsFalse(this.qian.GetLine(2) == null);
            Assert.IsTrue(this.qian.GetLine(2) != null);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Random r = new Random();
            for (int i = 0; i < 1000000;)
            {
                Core.Painting painting1 = new Core.Painting(
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2));
                Core.Painting painting2 = new Core.Painting(
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2),
                    (Core.LineAttribute)r.Next(0, 2));
                if (r.Next(0, 2) == 0)
                {
                    var line1 = this.yijing.GetHexagram(painting1).GetLine(r.Next(1, 7));
                    var line2 = this.yijing.GetHexagram(painting2).GetLine(r.Next(1, 7));
                    if (line1.GetHashCode() != line2.GetHashCode())
                    {
                        i++;
                        Assert.IsFalse(line1.Equals(line2));
                    }
                    else
                    {
                        Assert.IsTrue(line1.Equals(line2));
                    }
                }
                else
                {
                    var line1 = this.yijing.GetHexagram(painting1).GetLine(r.Next(1, 7));
                    var line2 = this.yijing.GetHexagram(painting1).GetLine(r.Next(1, 7));
                    if (line1.GetHashCode() != line2.GetHashCode())
                    {
                        i++;
                        Assert.AreNotEqual(line1, line2);
                    }
                    else
                    {
                        Assert.AreEqual(line1, line2);
                    }
                }
            }
        }

        [TestMethod()]
        public void CompareToTest()
        {
            Assert.IsTrue(this.qian.GetLine(1).CompareTo(this.shi.GetLine(1)) < 0);
            Assert.IsTrue(this.qian.GetLine(1).CompareTo(null) > 0);
            Assert.IsTrue(this.weiJi.GetLine(1).CompareTo(this.qian.GetLine(1)) > 0);
            Assert.IsTrue(this.qian.GetLine(1).CompareTo(this.qian.GetLine(1)) == 0);
            Assert.IsTrue(this.qian.GetLine(1).CompareTo(this.qian.GetLine(2)) < 0);
        }
    }
}