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
        private sealed record Translations(
            string[] YinLines, 
            string[] YangLines, 
            string HexagramsToString,
            string PureHexagramsToString);

        public string ApplyPattern(ZhouyiHexagram hexagram)
        {
            IEnumerable<string> BuildPlaceholders()
            {
                yield return Environment.NewLine;
                yield return ApplyPattern(hexagram.FirstLine);
                yield return ApplyPattern(hexagram.SecondLine);
                yield return ApplyPattern(hexagram.ThirdLine);
                yield return ApplyPattern(hexagram.FourthLine);
                yield return ApplyPattern(hexagram.FifthLine);
                yield return ApplyPattern(hexagram.SixthLine);
            }
            return string.Format(
                hexagram.UpperTrigram.Equals(hexagram.LowerTrigram) ?
                translations.PureHexagramsToString :
                translations.HexagramsToString, BuildPlaceholders());
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
            FileInfo file, JsonSerializerOptions baseOptions)
        {
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
