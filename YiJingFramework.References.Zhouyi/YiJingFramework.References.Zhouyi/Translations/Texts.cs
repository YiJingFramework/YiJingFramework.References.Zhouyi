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
    internal sealed class Texts
    {
        private readonly TextTranslations[] translations;
        private readonly string applyNines;
        private readonly string applySixes;
        internal sealed record ApplyNinesAndSixes(
            string Nines,
            string Sixes);
        internal sealed record TextTranslations(
            string Text,
            string[] Lines);

        internal string? GetApplyNinesOrSixes(int index)
        {
            return index switch {
                1 => this.applyNines,
                2 => this.applySixes,
                _ => default
            };
        }
        /// <summary>
        /// Do not modify the results' inner values.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal TextTranslations Get(int index)
        {
            return this.translations[index - 1];
        }

        private static bool CheckLengthAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length == length &&
                !array.Contains(null);
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal Texts(
            DirectoryInfo directory, JsonSerializerOptions baseOptions)
        {
            {
                var file = new FileInfo(
                    Path.Join(directory.FullName, $"ApplyNinesAndSixes.json"));
                try
                {
                    using var stream = file.OpenText();
                    var str = stream.ReadToEnd();
                    var translations =
                        JsonSerializer.Deserialize<ApplyNinesAndSixes>(
                            str, baseOptions);
                    if (translations is not null &&
                         translations.Nines is not null &&
                         translations.Sixes is not null)
                    {
                        this.applyNines = translations.Nines;
                        this.applySixes = translations.Sixes;
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
            this.translations = new TextTranslations[64];
            for (int i = 1; i <= 64; i++)
            {
                var file = new FileInfo(
                    Path.Join(directory.FullName, $"H{i}.json"));
                try
                {
                    using var stream = file.OpenText();
                    var str = stream.ReadToEnd();
                    var translations =
                        JsonSerializer.Deserialize<TextTranslations>(
                            str, baseOptions);
                    if (translations is not null &&
                         translations.Text is not null &&
                        CheckLengthAndNullValue(translations.Lines, 6))
                    {
                        this.translations[i - 1] = translations;
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
}
