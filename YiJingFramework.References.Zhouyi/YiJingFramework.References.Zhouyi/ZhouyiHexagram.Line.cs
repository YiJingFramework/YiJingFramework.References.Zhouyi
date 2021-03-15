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
                this.from = from;
                this.LineIndex = index;
                this.LineAttribute = attribute;
                this.LineText = text;
            }
            private readonly ZhouyiHexagram from;
            public int LineIndex { get; }
            public Core.LineAttribute LineAttribute { get; }
            public string LineText { get; }
            public override string ToString()
            {
                return this.from.patterns.ApplyPattern(this);
            }

            public override bool Equals(object? obj)
            {
                if (obj is Line line)
                    return this.LineIndex.Equals(line?.LineIndex);
                return false;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(this.from, this.LineIndex);
            }

            public bool Equals(Line? other)
            {
                return this.LineIndex.Equals(other?.LineIndex)
                    && this.from.Equals(other?.from);
            }

            public int CompareTo(Line? other)
            {
                var v1 = this.from.CompareTo(other?.from);
                if (v1 != 0)
                    return v1;
                return this.LineIndex.CompareTo(other?.LineIndex);
            }
        }
    }
}
