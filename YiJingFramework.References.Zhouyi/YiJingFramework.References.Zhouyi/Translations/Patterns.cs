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
    internal sealed class Patterns
    {
        private readonly Translations translations;
        private readonly Numbers numbers;
        private sealed record Translations(
            string[] YinLines,
            string[] YangLines,
            string HexagramsToString,
            string PureHexagramsToString,
            string ApplyNines,
            string ApplySixes);

        public string ApplyPattern(ZhouyiHexagram hexagram)
        {
            IEnumerable<string> BuildPlaceholders()
            {
                yield return Environment.NewLine;

                yield return this.numbers.GetOrdinal(hexagram.Index);
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
                    1 => string.Format(this.translations.ApplyNines
                    , Environment.NewLine, hexagram.ApplyNinesOrApplySixes),
                    2 => string.Format(this.translations.ApplySixes
                    , Environment.NewLine, hexagram.ApplyNinesOrApplySixes),
                    _ => string.Empty
                };
            }

            return string.Format(
                hexagram.UpperTrigram.Equals(hexagram.LowerTrigram) ?
                this.translations.PureHexagramsToString :
                this.translations.HexagramsToString, BuildPlaceholders().ToArray());
        }
        public string ApplyPattern(ZhouyiHexagram.Line line)
        {
            var pattern = line.LineAttribute switch {
                Core.LineAttribute.Yang => this.translations.YangLines,
                _ => this.translations.YinLines
            };
            return string.Format(pattern[line.LineIndex - 1], Environment.NewLine, line.LineText);
        }

        private static bool CheckLengthAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length == length &&
                !array.Contains(null);
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal Patterns(
            FileInfo file, JsonSerializerOptions baseOptions, Numbers numbers)
        {
            this.numbers = numbers;
            try
            {
                using var stream = file.OpenText();
                var str = stream.ReadToEnd();
                var translations =
                    JsonSerializer.Deserialize<Translations>(
                        str, baseOptions);
                if (translations is not null &&
                     translations.HexagramsToString is not null &&
                     translations.PureHexagramsToString is not null &&
                    CheckLengthAndNullValue(translations.YinLines, 6) &&
                    CheckLengthAndNullValue(translations.YangLines, 6))
                {
                    this.translations = translations;
                }
                else
                    throw new CannotReadTranslationException($"File content invalid: {file.FullName}");
            }
            catch (IOException e)
            {
                throw new CannotReadTranslationException($"File cannot read: {file.FullName}", e);
            }
            catch (JsonException e)
            {
                throw new CannotReadTranslationException($"File content invalid: {file.FullName}", e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new CannotReadTranslationException($"File cannot read: {file.FullName}", e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new CannotReadTranslationException($"File cannot read: {file.FullName}", e);
            }
        }
    }
}
