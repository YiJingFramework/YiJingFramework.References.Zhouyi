using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi.Exceptions;

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

                yield return hexagram.Index switch {
                    1 => string.Format(this.patterns.ApplyNines
                    , Environment.NewLine, hexagram.ApplyNinesOrApplySixes),
                    2 => string.Format(this.patterns.ApplySixes
                    , Environment.NewLine, hexagram.ApplyNinesOrApplySixes),
                    _ => string.Empty
                };
            }

            return string.Format(
                hexagram.UpperTrigram.Equals(hexagram.LowerTrigram) ?
                this.patterns.PureHexagramsToString :
                this.patterns.HexagramsToString, BuildPlaceholders().ToArray());
        }
        public string ApplyPattern(ZhouyiHexagram.Line line)
        {
            var pattern = line.LineAttribute switch {
                Core.LineAttribute.Yang => this.patterns.YangLines,
                _ => this.patterns.YinLines
            };
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
