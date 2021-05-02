using System;
using System.Collections.Generic;
using System.Linq;

namespace YiJingFramework.References.Zhouyi.Translations
{
    internal sealed class PatternsAndNumbers
    {
        private readonly TranslationFile.TranslationFileNumbers numbers;
        private readonly TranslationFile.TranslationFilePatterns patterns;

        public string ApplyPattern(ZhouyiHexagram hexagram)
        {
            IEnumerable<string> BuildPlaceholders()
            {
                yield return Environment.NewLine;

                yield return this.numbers.Ordinal[hexagram.Index - 1];
                yield return hexagram.Name;
                yield return hexagram.Text;

                yield return hexagram.LowerTrigram.Name;
                yield return hexagram.LowerTrigram.Nature;
                yield return hexagram.UpperTrigram.Name;
                yield return hexagram.UpperTrigram.Nature;

                yield return this.ApplyPattern(hexagram.FirstLine);
                yield return this.ApplyPattern(hexagram.SecondLine);
                yield return this.ApplyPattern(hexagram.ThirdLine);
                yield return this.ApplyPattern(hexagram.FourthLine);
                yield return this.ApplyPattern(hexagram.FifthLine);
                yield return this.ApplyPattern(hexagram.SixthLine);

                if (hexagram.ApplyNinesOrApplySixes is not null)
                    yield return this.ApplyPattern(hexagram.ApplyNinesOrApplySixes);
                else
                    yield return string.Empty;
            }

            return string.Format(
                hexagram.UpperTrigram.Equals(hexagram.LowerTrigram) ?
                this.patterns.PureHexagramsToString :
                this.patterns.HexagramsToString, BuildPlaceholders().ToArray());
        }
        public string ApplyPattern(ZhouyiHexagram.Line line)
        {
            if (line.LineIndex == 0)
            {
                return string.Format(line.YinYang == Core.YinYang.Yang ?
                    this.patterns.ApplyNines : this.patterns.ApplySixes,
                    Environment.NewLine, line.LineText);
            }
            var pattern = line.YinYang.IsYang ? this.patterns.YangLines : this.patterns.YinLines;
            return string.Format(pattern[line.LineIndex - 1], Environment.NewLine, line.LineText);
        }

        internal PatternsAndNumbers(TranslationFile.TranslationFilePatterns patterns,
            TranslationFile.TranslationFileNumbers numbers)
        {
            this.numbers = numbers;
            this.patterns = patterns;
        }
    }
}
