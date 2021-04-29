using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi
{
    /// <summary>
    /// 《周易》经卦。
    /// A trigram in Zhouyi.
    /// </summary>
    public sealed class ZhouyiTrigram : IEquatable<ZhouyiTrigram>, IComparable<ZhouyiTrigram>
    {
        /// <summary>
        /// 先天八卦数，
        /// 可以作为从 1 开始的序号。
        /// The innate number of the trigram,
        /// which can work as a 1-based index.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// 卦名。
        /// Name of the trigram.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 卦对应的自然事物。
        /// Nature of the trigram.
        /// </summary>
        public string Nature { get; }
        internal ZhouyiTrigram(int index, string name, string nature)
        {
            this.Index = index;
            this.Name = name;
            this.Nature = nature;
        }

        /// <summary>
        /// 获取卦画。
        /// Get the painting of the trigram.
        /// </summary>
        /// <returns>
        /// 卦画。
        /// The painting.
        /// </returns>
        public Core.Painting GetPainting()
        {
            var index = this.Index - 1;
            var first = index >> 2;
            var second = (index >> 1) - first * 2;
            var third = index - first * 4 - second * 2;
            return new Core.Painting(
                first == 0 ? Core.YinYang.Yang : Core.YinYang.Yin,
                second == 0 ? Core.YinYang.Yang : Core.YinYang.Yin,
                third == 0 ? Core.YinYang.Yang : Core.YinYang.Yin);
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Trigrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ZhouyiTrigram? other)
        {
            return this.Index.Equals(other?.Index);
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Trigrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object? other)
        {
            if (other is ZhouyiTrigram trigram)
                return this.Index.Equals(trigram?.Index);
            return false;
        }

        /// <summary>
        /// 返回卦名。
        /// Returns the name.
        /// </summary>
        /// <returns>
        /// 卦名。
        /// Name of the trigram.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Index;
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Trigrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ZhouyiTrigram? other)
        {
            return this.Index.CompareTo(other?.Index);
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Trigrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ZhouyiTrigram? left, ZhouyiTrigram? right)
            => left?.Index == right?.Index;

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Trigrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ZhouyiTrigram? left, ZhouyiTrigram? right)
            => left?.Index != right?.Index;
    }
}
