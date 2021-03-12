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
    public sealed class NamesTranslations
    {
        private readonly string[] trigrams;
        private readonly string[] hexagrams;
        private sealed record Translations(string[] Trigrams,string[] Hexagrams);
        public string GetTrigramName(int index)
        {
            if (index is < 1 or > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    index, 
                    $"{nameof(index)} should be between 1 and 64.");
            return this.trigrams[index - 1];
        }
        internal string GetTrigramNameByIndex(int index)
        {
            return this.trigrams[index];
        }
        public string GetHexagramName(int index)
        {
            if (index is < 1 or > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    index,
                    $"{nameof(index)} should be between 1 and 64.");
            return this.hexagrams[index - 1];
        }
        internal string GetHexagramNameByIndex(int index)
        {
            return this.hexagrams[index];
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal NamesTranslations(
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
                    translations.Trigrams is null || translations.Hexagrams is null
                    || translations.Trigrams.Length is not 8 || translations.Hexagrams.Length is not 64)
                    throw new CannotReadTranslationException($"File content invalid: {file.FullName}");

                this.hexagrams = new string[64];
                Array.Copy(translations.Hexagrams, this.hexagrams, 64);
                this.trigrams = new string[8];
                Array.Copy(translations.Trigrams, this.trigrams, 8);
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
