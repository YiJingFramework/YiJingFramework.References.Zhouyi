using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi.Exceptions;

namespace YiJingFramework.References.Zhouyi.Translations
{
    internal sealed class Grams
    {
        private Translations translations;
        private sealed record Translations(string[] Trigrams, string[] TrigramNatures, string[] Hexagrams);

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ZhouyiTrigram GetTrigram(int index)
        {
            return
                new(index, 
                this.translations.Trigrams[index - 1], 
                this.translations.TrigramNatures[index - 1]);
        }
        internal bool TryGetTrigramByName(string name, [MaybeNullWhen(false)] out ZhouyiTrigram trigram)
        {
            var index = Array.IndexOf(this.translations.Trigrams, name);
            if (index == -1)
            {
                trigram = null;
                return false;
            }
            trigram = new(index + 1, name, this.translations.TrigramNatures[index]);
            return true;
        }
        internal bool TryGetTrigramByNature(string nature, [MaybeNullWhen(false)] out ZhouyiTrigram trigram)
        {
            var index = Array.IndexOf(this.translations.TrigramNatures, nature);
            if (index == -1)
            {
                trigram = null;
                return false;
            }
            trigram = new(index + 1, this.translations.Trigrams[index], nature);
            return true;
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal string GetHexagramName(int index)
        {
            return this.translations.Hexagrams[index - 1];
        }

        internal int GetHexagramIndex(string name)
        {
            return Array.IndexOf(this.translations.TrigramNatures, name);
        }

        private static bool CheckLengthAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length == length &&
                !array.Contains(null);
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal Grams(
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
                    CheckLengthAndNullValue(translations.Trigrams, 8) &&
                    CheckLengthAndNullValue(translations.TrigramNatures, 8) &&
                    CheckLengthAndNullValue(translations.Hexagrams, 64))
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