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
    public sealed class MiscTranslations
    {
        private readonly string[] cardinal;
        private readonly string[] ordinal;
        public string Yin { get; }
        public string Yang { get; }
        private sealed record Translations(string Yin, string Yang, string[] Cardinal,string[] Ordinal);
        public string GetCardinal(int number)
        {
            if (number is < 1 or > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(number), 
                    number, 
                    $"{nameof(number)} should be between 1 and 64.");
            return this.cardinal[number - 1];
        }
        internal string GetCardinalByIndex(int index)
        {
            return this.cardinal[index];
        }
        public string GetOrdinal(int number)
        {
            if (number is < 1 or > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(number),
                    number,
                    $"{nameof(number)} should be between 1 and 64.");
            return this.ordinal[number - 1];
        }
        internal string GetOrdinalByIndex(int index)
        {
            return this.ordinal[index];
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal MiscTranslations(
            FileInfo file, JsonSerializerOptions baseOptions)
        {
            try
            {
                using var stream = file.OpenText();
                var str = stream.ReadToEnd();
                var translations =
                    JsonSerializer.Deserialize<Translations>(
                        str, baseOptions);
                if (translations is null || 
                    translations.Cardinal is null || translations.Ordinal is null
                    || translations.Cardinal.Length < 64 || translations.Ordinal.Length < 64
                    || translations.Yin is null || translations.Yang is null)
                    throw new CannotReadTranslationException($"File content invalid: {file.FullName}");

                this.ordinal = new string[64];
                Array.Copy(translations.Ordinal, this.ordinal, 64);
                this.cardinal = new string[64];
                Array.Copy(translations.Cardinal, this.cardinal, 64);
                this.Yin = translations.Yin;
                this.Yang = translations.Yang;
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
