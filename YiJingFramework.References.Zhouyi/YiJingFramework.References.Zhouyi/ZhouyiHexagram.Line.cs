using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi
{
    public sealed partial class ZhouyiHexagram
    {
        /// <summary>
        /// 别卦的一根爻。
        /// A line in a hexagram.
        /// </summary>
        public sealed class Line : IEquatable<Line>, IComparable<Line>
        {
            internal Line(ZhouyiHexagram from, int index, Core.LineAttribute attribute, string text)
            {
                this.From = from;
                this.LineIndex = index;
                this.LineAttribute = attribute;
                this.LineText = text;
            }

            /// <summary>
            /// 所属的别卦。
            /// The hexagram that the line belongs to.
            /// </summary>
            public ZhouyiHexagram From { get; }

            /// <summary>
            /// 从 1 开始的序号。
            /// 为 0 表示用辞。
            /// 1-based index of the line.
            /// If its value is 0, this line represents the apply nines or apply sixes.
            /// </summary>
            public int LineIndex { get; }

            /// <summary>
            /// 爻性质。
            /// Attribute of the line.
            /// </summary>
            public Core.LineAttribute LineAttribute { get; }

            /// <summary>
            /// 爻辞。
            /// Text of the line.
            /// </summary>
            public string LineText { get; }

            /// <summary>
            /// 根据模板获取一个字符串表示。
            /// Get a string representation of the line according to the patterns.
            /// </summary>
            /// <returns>
            /// 字符串表示。
            /// The string representation.
            /// </returns>
            public override string ToString()
            {
                return this.From.Patterns.ApplyPattern(this);
            }

            /// <summary>
            /// 爻所属的卦相同才会视为相等，
            /// 但来自不同的 <see cref="Zhouyi"/> 实例的爻也可能被视为相等。
            /// Only when the two lines are from the same hexagram will be regarded same,
            /// but lines from different <see cref="Zhouyi"/> instances may also be.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public override bool Equals(object? other)
            {
                if (other is Line line)
                    return this.LineIndex.Equals(line?.LineIndex);
                return false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCode.Combine(this.From, this.LineIndex);
            }

            /// <summary>
            /// 爻所属的卦相同才会视为相等，
            /// 但来自不同的 <see cref="Zhouyi"/> 实例的爻也可能被视为相等。
            /// Only when the two lines are from the same hexagram will be regarded same,
            /// but lines from different <see cref="Zhouyi"/> instances may also be.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(Line? other)
            {
                return this.LineIndex.Equals(other?.LineIndex)
                    && this.From.Equals(other?.From);
            }

            /// <summary>
            /// 爻所属的卦相同才会视为相等，
            /// 但来自不同的 <see cref="Zhouyi"/> 实例的爻也可能被视为相等。
            /// Only when the two lines are from the same hexagram will be regarded same,
            /// but lines from different <see cref="Zhouyi"/> instances may also be.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Line? other)
            {
                var v1 = this.From.CompareTo(other?.From);
                if (v1 != 0)
                    return v1;
                return this.LineIndex.CompareTo(other?.LineIndex);
            }

            /// <summary>
            /// 爻所属的卦相同才会视为相等，
            /// 但来自不同的 <see cref="Zhouyi"/> 实例的爻也可能被视为相等。
            /// Only when the two lines are from the same hexagram will be regarded same,
            /// but lines from different <see cref="Zhouyi"/> instances may also be.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator ==(Line? left, Line? right)
            {
                if (left is null)
                    return right is null;
                else if (right is null)
                    return false;
                return left.LineIndex.Equals(right.LineIndex)
                    && left.From.Equals(right.From);
            }

            /// <summary>
            /// 爻所属的卦相同才会视为相等，
            /// 但来自不同的 <see cref="Zhouyi"/> 实例的爻也可能被视为相等。
            /// Only when the two lines are from the same hexagram will be regarded same,
            /// but lines from different <see cref="Zhouyi"/> instances may also be.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator !=(Line? left, Line? right)
            {
                if (left is null)
                    return right is not null;
                else if (right is null)
                    return true;
                return !left.LineIndex.Equals(right.LineIndex)
                    || !left.From.Equals(right.From);
            }
        }
    }
}
