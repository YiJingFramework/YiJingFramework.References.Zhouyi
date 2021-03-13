using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi
{
    public sealed class ZhouyiTrigram : IEquatable<ZhouyiTrigram>, IComparable<ZhouyiTrigram>
    {
        public int Index { get; }
        public string Name { get; }
        public string Nature { get; }
        internal ZhouyiTrigram(int index, string name, string nature)
        {
            this.Index = index;
            this.Name = name;
            this.Nature = nature;
        }
        public Core.Painting GetPainting()
        {
            var index = this.Index - 1;
            var first = index >> 2;
            var second = index >> 1 - first * 2;
            var third = index - first * 4 - second * 2;
            return new Core.Painting(
                first == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin,
                second == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin,
                third == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin);
        }

        public bool Equals(ZhouyiTrigram? other)
        {
            return this.Index.Equals(other?.Index);
        }

        public override bool Equals(object? obj)
        {
            if (obj is ZhouyiTrigram trigram)
                return this.Index.Equals(trigram?.Index);
            return false;
        }
        
        public override string ToString()
        {
            return this.Name;
        }

        public override int GetHashCode()
        {
            return this.Index;
        }

        public int CompareTo(ZhouyiTrigram? other)
        {
            return this.Index.CompareTo(other?.Index);
        }

        public static bool operator ==(ZhouyiTrigram? left, ZhouyiTrigram? right)
        {
            return left?.Index == right?.Index;
        }

        public static bool operator !=(ZhouyiTrigram? left, ZhouyiTrigram? right)
        {
            return left?.Index != right?.Index;
        }
    }
}
