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
    internal sealed class Numbers
    {
        private readonly string[] ordinal;
        private sealed record Translations(string[] Ordinal);
        internal string GetOrdinal(int number)
        {
            if (number is < 1 or > 64)
                throw new ArgumentOutOfRangeException(
                    nameof(number),
                    number,
                    $"{nameof(number)} should be between 1 and 64.");
            return this.ordinal[number - 1];
        }
        private static bool CheckLongerAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length >= length &&
                !array.Contains(null);
        }

        /// <exception cref="CannotReadTranslationException"></exception>
        internal Numbers(
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
                   CheckLongerAndNullValue(translations.Ordinal, 64))
                {
                    this.ordinal = new string[64];
                    Array.Copy(translations.Ordinal, this.ordinal, 64);
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
