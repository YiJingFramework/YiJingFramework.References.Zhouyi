using System;
using System.Collections.Generic;
using YiJingFramework.References.Zhouyi.Translations;

namespace YiJingFramework.References.Zhouyi
{
    /// <summary>
    /// 《周易》别卦。
    /// A hexagram in Zhouyi.
    /// </summary>
    public sealed partial class ZhouyiHexagram : IEquatable<ZhouyiHexagram>, IComparable<ZhouyiHexagram>
    {
        internal ZhouyiHexagram(PatternsAndNumbers patterns,
            int index, string name,
            string text, string? applyNinesOrApplySixes,
            ZhouyiTrigram lowerTrigram,
            ZhouyiTrigram upperTrigram,
            string[] lines)
        {
            this.Patterns = patterns;
            this.Text = text;
            this.Index = index;
            this.Name = name;
            var trigram = lowerTrigram.GetPainting();
            this.FirstLine = new Line(this, 1, trigram[0], lines[0]);
            this.SecondLine = new Line(this, 2, trigram[1], lines[1]);
            this.ThirdLine = new Line(this, 3, trigram[2], lines[2]);
            trigram = upperTrigram.GetPainting();
            this.FourthLine = new Line(this, 4, trigram[0], lines[3]);
            this.FifthLine = new Line(this, 5, trigram[1], lines[4]);
            this.SixthLine = new Line(this, 6, trigram[2], lines[5]);
            this.LowerTrigram = lowerTrigram;
            this.UpperTrigram = upperTrigram;

            if (applyNinesOrApplySixes is not null)
                this.ApplyNinesOrApplySixes = new Line(this, 0, trigram[0], applyNinesOrApplySixes);
        }

        private PatternsAndNumbers Patterns { get; }

        /// <summary>
        /// 卦辞。
        /// Text of the hexagram.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 用九或用六。
        /// Apply nines or apply sixes.
        /// </summary>
        public Line? ApplyNinesOrApplySixes { get; }

        /// <summary>
        /// 一个从 1 开始的序号，反映其在《周易》中的位置。
        /// A 1-based index of the hexagram represents its position in Zhouyi.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 卦名。
        /// Name of the hexagram.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 根据爻序号获取一行。
        /// Get a line by the line index.
        /// </summary>
        /// <param name="index">
        /// 一个从 1 开始的爻序号。
        /// A 1-based line index.
        /// </param>
        /// <returns>
        /// 爻。
        /// The line.
        /// </returns>
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

        /// <summary>
        /// 初爻。
        /// The first line.
        /// </summary>
        public Line FirstLine { get; }

        /// <summary>
        /// 第二爻。
        /// The second line.
        /// </summary>
        public Line SecondLine { get; }

        /// <summary>
        /// 第三爻。
        /// The third line.
        /// </summary>
        public Line ThirdLine { get; }

        /// <summary>
        /// 主卦。
        /// The lower trigram.
        /// </summary>
        public ZhouyiTrigram LowerTrigram { get; }

        /// <summary>
        /// 第四爻。
        /// The fourth line.
        /// </summary>
        public Line FourthLine { get; }

        /// <summary>
        /// 第五爻。
        /// The fifth line.
        /// </summary>
        public Line FifthLine { get; }

        /// <summary>
        /// 上爻。
        /// The sixth line.
        /// </summary>
        public Line SixthLine { get; }

        /// <summary>
        /// 客卦。
        /// The upper trigram.
        /// </summary>
        public ZhouyiTrigram UpperTrigram { get; }

        private IEnumerable<Core.YinYang> GetAttributes()
        {
            yield return this.FirstLine.YinYang;
            yield return this.SecondLine.YinYang;
            yield return this.ThirdLine.YinYang;
            yield return this.FourthLine.YinYang;
            yield return this.FifthLine.YinYang;
            yield return this.SixthLine.YinYang;
        }

        /// <summary>
        /// 获取卦画。
        /// Get the painting of the hexagram.
        /// </summary>
        /// <returns>
        /// 卦画。
        /// The painting.
        /// </returns>
        public Core.Painting GetPainting()
        {
            return new Core.Painting(this.GetAttributes());
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Hexagrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ZhouyiHexagram? other)
        {
            return this.Index.Equals(other?.Index);
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Hexagrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is ZhouyiHexagram hexagram)
                return this.Index.Equals(hexagram?.Index);
            return false;
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Hexagrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ZhouyiHexagram? other)
        {
            return this.Index.CompareTo(other?.Index);
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
        /// 根据模板获取一个字符串表示。
        /// Get a string representation of the hexagram according to the patterns.
        /// </summary>
        /// <returns>
        /// 字符串表示。
        /// The string representation.
        /// </returns>
        public override string ToString()
        {
            return this.Patterns.ApplyPattern(this);
        }

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Hexagrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ZhouyiHexagram? left, ZhouyiHexagram? right)
            => left?.Index == right?.Index;

        /// <summary>
        /// 来自不同的 <see cref="Zhouyi"/> 实例的卦也可能被视为相等。
        /// Hexagrams from different <see cref="Zhouyi"/> instances may also be regarded same.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ZhouyiHexagram? left, ZhouyiHexagram? right)
            => left?.Index != right?.Index;
    }
}