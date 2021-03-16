using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.Core;
using YiJingFramework.References.Zhouyi.Translations;

namespace YiJingFramework.References.Zhouyi
{
    public sealed partial class ZhouyiHexagram : IEquatable<ZhouyiHexagram>, IComparable<ZhouyiHexagram>
    {
        private Patterns Patterns { get; }
        internal ZhouyiHexagram(Zhouyi zhouyi, int index, string name)
        {
            {
                this.Index = index;
                this.Patterns = zhouyi.Patterns;
                this.Name = name;
            }
            {
                var (upper, lower) = Maps.HexagramsToTrigrams.GetTrigrams(index);
                this.UpperTrigram = zhouyi.GramsTranslations.GetTrigram(upper);
                this.LowerTrigram = zhouyi.GramsTranslations.GetTrigram(lower);
            }
            var texts = zhouyi.TextTranslations.Get(index);
            this.Text = texts.Text;
            this.ApplyNinesOrApplySixes = zhouyi.TextTranslations.GetApplyNinesOrSixes(index);
            {
                var low = this.LowerTrigram.GetPainting();
                this.FirstLine = new Line(this, 1, low[0], texts.Lines[0]);
                this.SecondLine = new Line(this, 2, low[1], texts.Lines[1]);
                this.ThirdLine = new Line(this, 3, low[2], texts.Lines[2]);
            }
            {
                var up = this.UpperTrigram.GetPainting();
                this.FourthLine = new Line(this, 4, up[0], texts.Lines[3]);
                this.FifthLine = new Line(this, 5, up[1], texts.Lines[4]);
                this.SixthLine = new Line(this, 6, up[2], texts.Lines[5]);
            }
        }
        internal ZhouyiHexagram(Zhouyi zhouyi, Painting painting)
        {
            {
                this.LowerTrigram = zhouyi.GetTrigram(
                    new Painting(painting[0], painting[1], painting[2]));
                this.UpperTrigram = zhouyi.GetTrigram(
                    new Painting(painting[3], painting[4], painting[5]));
            }
            {
                this.Index = Maps.HexagramsToTrigrams.GetHexagram(
                    this.LowerTrigram.Index, this.UpperTrigram.Index);
                this.Patterns = zhouyi.Patterns;
                this.Name = zhouyi.GramsTranslations.GetHexagramName(this.Index);
            }
            var texts = zhouyi.TextTranslations.Get(this.Index);
            this.Text = texts.Text;
            this.ApplyNinesOrApplySixes = zhouyi.TextTranslations.GetApplyNinesOrSixes(this.Index);
            {
                this.FirstLine = new Line(this, 1, painting[0], texts.Lines[0]);
                this.SecondLine = new Line(this, 2, painting[1], texts.Lines[1]);
                this.ThirdLine = new Line(this, 3, painting[2], texts.Lines[2]);
                this.FourthLine = new Line(this, 4, painting[3], texts.Lines[3]);
                this.FifthLine = new Line(this, 5, painting[4], texts.Lines[4]);
                this.SixthLine = new Line(this, 6, painting[5], texts.Lines[5]);
            }
        }

        public string Text { get; }
        public string? ApplyNinesOrApplySixes { get; }

        public int Index { get; }
        public string Name { get; }

        public Line GetLine(int index)
        {
            return index switch {
                1 => this.FirstLine,
                2 => this.SecondLine,
                3 => this.ThirdLine,
                4 => this.FourthLine,
                5 => this.FifthLine,
                6 => this.SixthLine,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(index),
                    index,
                    $"{nameof(index)} should be between 1 and 6.")
            };
        }

        public Line FirstLine { get; }
        public Line SecondLine { get; }
        public Line ThirdLine { get; }
        public ZhouyiTrigram LowerTrigram { get; }

        public Line FourthLine { get; }
        public Line FifthLine { get; }
        public Line SixthLine { get; }
        public ZhouyiTrigram UpperTrigram { get; }

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
            return this.Patterns.ApplyPattern(this);
        }
        public static bool operator ==(ZhouyiHexagram? left, ZhouyiHexagram? right)
            => left?.Index == right?.Index;

        public static bool operator !=(ZhouyiHexagram? left, ZhouyiHexagram? right)
            => left?.Index != right?.Index;
    }
}