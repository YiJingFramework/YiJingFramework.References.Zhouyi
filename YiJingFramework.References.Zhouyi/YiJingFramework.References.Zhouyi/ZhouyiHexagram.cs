using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.Core;
using YiJingFramework.References.Zhouyi.Translations;

namespace YiJingFramework.References.Zhouyi
{
    public sealed partial class ZhouyiHexagram: IEquatable<ZhouyiHexagram>, IComparable<ZhouyiHexagram>
    {
        Patterns patterns;
        internal ZhouyiHexagram(ZhouyiJingSection zhouyi, int index, string name)
        {
            this.Index = index;
            this.Name = zhouyi.GramsTranslations.GetHexagramName(index);
            this.patterns = zhouyi.Patterns;
        }
        internal ZhouyiHexagram(ZhouyiJingSection zhouyi, Painting painting)
        {
        }

        public string Text { get; }

        public int Index { get; }
        public string Name { get; }

        public Line GetLine(int index)
        {
            return index switch {
                1 => FirstLine,
                2 => SecondLine,
                3 => ThirdLine,
                4 => FourthLine,
                5 => FifthLine,
                6 => SixthLine,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(index),
                    index,
                    $"{nameof(index)} should be between 1 and 6.")
            };
        }

        public Line FirstLine { get; }
        public Line SecondLine { get; }
        public Line ThirdLine { get; }
        public Core.Painting LowerTrigram
            => new Core.Painting(GetLowerTrigramAttributes());

        public Line FourthLine { get; }
        public Line FifthLine { get; }
        public Line SixthLine { get; }
        public Core.Painting UpperTrigram
            => new Core.Painting(GetUpperTrigramAttributes());

        private IEnumerable<Core.LineAttribute> GetLowerTrigramAttributes()
        {
            yield return this.FirstLine.LineAttribute;
            yield return this.SecondLine.LineAttribute;
            yield return this.ThirdLine.LineAttribute;
        }
        private IEnumerable<Core.LineAttribute> GetUpperTrigramAttributes()
        {
            yield return this.FourthLine.LineAttribute;
            yield return this.FifthLine.LineAttribute;
            yield return this.SixthLine.LineAttribute;
        }

        public Core.Painting GetPainting()
        {
            return new Core.Painting(this.GetLowerTrigramAttributes()
                .Concat(this.GetUpperTrigramAttributes()));
        }

        public bool Equals(ZhouyiHexagram? other)
        {
            return this.Index.Equals(other?.Index);
        }

        public override bool Equals(object? obj)
        {
            if (obj is ZhouyiHexagram hexagram)
                return this.Index.Equals(hexagram?.Index);
            return false;
        }

        public int CompareTo(ZhouyiHexagram? other)
        {
            return this.Index.CompareTo(other?.Index);
        }

        public override int GetHashCode()
        {
            return this.Index;
        }
        public override string ToString()
        {
            return patterns.ApplyPattern(this);
        }
    }
}