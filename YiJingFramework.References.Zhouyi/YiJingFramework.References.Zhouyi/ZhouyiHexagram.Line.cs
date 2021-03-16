using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi
{
    public sealed partial class ZhouyiHexagram
    {
        public sealed class Line : IEquatable<Line>, IComparable<Line>
        {
            internal Line(ZhouyiHexagram from, int index, Core.LineAttribute attribute, string text)
            {
                this.From = from;
                this.LineIndex = index;
                this.LineAttribute = attribute;
                this.LineText = text;
            }
            public ZhouyiHexagram From { get; }
            public int LineIndex { get; }
            public Core.LineAttribute LineAttribute { get; }
            public string LineText { get; }
            public override string ToString()
            {
                return this.From.Patterns.ApplyPattern(this);
            }

            public override bool Equals(object? obj)
            {
                if (obj is Line line)
                    return this.LineIndex.Equals(line?.LineIndex);
                return false;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(this.From, this.LineIndex);
            }

            public bool Equals(Line? other)
            {
                return this.LineIndex.Equals(other?.LineIndex)
                    && this.From.Equals(other?.From);
            }

            public int CompareTo(Line? other)
            {
                var v1 = this.From.CompareTo(other?.From);
                if (v1 != 0)
                    return v1;
                return this.LineIndex.CompareTo(other?.LineIndex);
            }
            public static bool operator ==(Line? left, Line? right)
            {
                if (left is null)
                    return right is null;
                else if (right is null)
                    return false;
                return left.LineIndex.Equals(right.LineIndex)
                    && left.From.Equals(right.From);
            }

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
