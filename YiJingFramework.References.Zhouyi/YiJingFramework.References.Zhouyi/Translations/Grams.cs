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
        private readonly string[] hexagrams;
        private sealed record Translations(string[] Trigrams, string[] TrigramNatures, string[] Hexagrams);

        private readonly ZhouyiTrigram[] trigrams;

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ZhouyiTrigram GetTrigram(int index)
        {
            return this.trigrams[index - 1];
        }
        internal bool TryGetTrigramByName(string name, [MaybeNullWhen(false)] out ZhouyiTrigram trigram)
        {
            foreach (var tri in this.trigrams)
            {
                if (tri.Name == name)
                {
                    trigram = tri;
                    return true;
                }
            }
            trigram = null;
            return false;
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal string GetHexagramName(int index)
        {
            return this.hexagrams[index - 1];
        }
        internal bool TryGetHexagramIndex(string name, out int index)
        {
            index = Array.IndexOf(this.hexagrams, name) + 1;
            return index != 0;
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
                    this.hexagrams = translations.Hexagrams;
                    List<ZhouyiTrigram> trigrams = new List<ZhouyiTrigram>(8);
                    for (int i = 0; i < 8; i++)
                    {
                        trigrams.Add(
                            new ZhouyiTrigram(i + 1,
                            translations.Trigrams[i],
                            translations.TrigramNatures[i]));
                    }
                    this.trigrams = trigrams.ToArray();
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