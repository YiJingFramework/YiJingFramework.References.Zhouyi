using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi.Exceptions;
using YiJingFramework.References.Zhouyi.Translations;

namespace YiJingFramework.References.Zhouyi
{
    /// <summary>
    /// 《周易》。
    /// Zhouyi.
    /// </summary>
    public sealed class Zhouyi
    {
        private readonly TrigramsAndHexagrams trigramsAndHexagrams;
        private readonly PatternsAndNumbers patternsAndNumbers;

        private static readonly JsonSerializerOptions jsonSerializerOptions
            = new JsonSerializerOptions() {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };

        /// <summary>
        /// 创建一个不具有翻译的新实例。
        /// 所有翻译将使用空字符串代替。
        /// Initialize a new instance without translations.
        /// Empty strings will be used instead of the translations.
        /// </summary>
        public Zhouyi()
        {
            TranslationFile translation = TranslationFile.Empty;
            Debug.Assert(translation.Check());
            this.patternsAndNumbers = new PatternsAndNumbers(
                translation.Patterns, translation.Numbers);
            this.trigramsAndHexagrams = new TrigramsAndHexagrams(
                translation.Hexagrams, translation.Trigrams);
        }

        /// <summary>
        /// 创建新实例。
        /// Initialize a new instance.
        /// </summary>
        /// <param name="translationFilePath">
        /// 翻译文件路径。
        /// Path of the translation file.
        /// </param>
        /// <exception cref="CannotReadTranslationException">
        /// 读取翻译失败。
        /// Cannot read the translation.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="translationFilePath"/> 为 <c>null</c> 。
        /// <paramref name="translationFilePath"/> is <c>null</c>.
        /// </exception>
        public Zhouyi(string translationFilePath)
        {
            if (translationFilePath is null)
                throw new ArgumentNullException(nameof(translationFilePath));
            TranslationFile? translation;
            try
            {
                using (var stream = new StreamReader(translationFilePath))
                    translation = JsonSerializer.Deserialize<TranslationFile>(
                        stream.ReadToEnd(), jsonSerializerOptions);
            }
            catch (IOException e)
            {
                throw new CannotReadTranslationException($"Cannot read translation file: {translationFilePath}", e);
            }
            catch (JsonException e)
            {
                throw new CannotReadTranslationException($"Invalid translation file: {translationFilePath}", e);
            }
            catch (ArgumentException e)
            {
                throw new CannotReadTranslationException($"Cannot read translation file: {translationFilePath}", e);
            }

            if (translation is null || !translation.Check())
                throw new CannotReadTranslationException($"Invalid translation file: {translationFilePath}");

            this.patternsAndNumbers = new PatternsAndNumbers(
                translation.Patterns, translation.Numbers);
            this.trigramsAndHexagrams = new TrigramsAndHexagrams(
                translation.Hexagrams, translation.Trigrams);
        }

        /// <summary>
        /// 创建新实例。
        /// Initialize a new instance.
        /// </summary>
        /// <param name="translationStream">
        /// 翻译流。
        /// The stream of the translation.
        /// </param>
        /// <param name="leaveOpen">
        /// 是否在读取后保存流开启。
        /// Whether to keep the stream open after reading.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="translationStream"/> 为 <c>null</c> 。
        /// <paramref name="translationStream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="CannotReadTranslationException">
        /// 读取翻译失败。
        /// Cannot read the translation.
        /// </exception>
        public Zhouyi(Stream translationStream, bool leaveOpen = false)
        {
            if (translationStream is null)
                throw new ArgumentNullException(nameof(translationStream));
            TranslationFile? translation;
            try
            {
                using (var stream = new StreamReader(translationStream, null, true, -1, leaveOpen))
                    translation = JsonSerializer.Deserialize<TranslationFile>(
                        stream.ReadToEnd(), jsonSerializerOptions);
            }
            catch (JsonException e)
            {
                throw new CannotReadTranslationException($"Invalid translation.", e);
            }

            if (translation is null || !translation.Check())
                throw new CannotReadTranslationException($"Invalid translation.");
            this.patternsAndNumbers = new PatternsAndNumbers(
                translation.Patterns, translation.Numbers);
            this.trigramsAndHexagrams = new TrigramsAndHexagrams(
                translation.Hexagrams, translation.Trigrams);
        }

        /// <summary>
        /// 通过卦画获取别卦。
        /// Get a hexagram by its painting.
        /// </summary>
        /// <param name="painting">
        /// 卦画。
        /// The painting.
        /// </param>
        /// <returns>
        /// 结果。
        /// The result.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="painting"/> 是 <c>null</c> 。
        /// <paramref name="painting"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="painting"/> 不表示一个别卦。
        /// <paramref name="painting"/> can't represent a hexagram.
        /// </exception>
        public ZhouyiHexagram GetHexagram(Core.Painting painting)
        {
            if (painting.Count != 6)
                throw new ArgumentException(
                    $"{nameof(painting)} should represent a hexagram.",
                    nameof(painting));
            var l = this.GetTrigram(new Core.Painting(painting[0], painting[1], painting[2]));
            var u = this.GetTrigram(new Core.Painting(painting[3], painting[4], painting[5]));
            var index = Maps.HexagramsToTrigrams.GetHexagram(l.Index, u.Index);
            var t = this.trigramsAndHexagrams.GetHexagram(index);
            return new ZhouyiHexagram(this.patternsAndNumbers,
                index,
                t.Name,
                t.Text,
                t.ApplyNinesOrSixes,
                l,
                u,
                t.Lines);
        }

        /// <summary>
        /// 通过序号获取别卦。
        /// Get a hexagram by its index.
        /// </summary>
        /// <param name="index">
        /// 序号。
        /// The index.
        /// </param>
        /// <returns>
        /// 结果。
        /// The result.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 不在 <c>1</c> （含）至 <c>64</c> （含）之间。
        /// <paramref name="index"/> is not between <c>1</c>(include) to <c>64</c>(include).
        /// </exception>
        public ZhouyiHexagram GetHexagram(int index)
        {
            if (index is < 1 or > 64)
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"{nameof(index)} should be between 1(include) and 64(include).");
            var t = this.trigramsAndHexagrams.GetHexagram(index);
            var tri = Maps.HexagramsToTrigrams.GetTrigrams(index);
            return new ZhouyiHexagram(this.patternsAndNumbers,
                index,
                t.Name,
                t.Text,
                t.ApplyNinesOrSixes,
                this.trigramsAndHexagrams.GetTrigram(tri.lower),
                this.trigramsAndHexagrams.GetTrigram(tri.upper),
                t.Lines);
        }
        /// <summary>
        /// 通过卦名获取别卦。
        /// 如果有卦名重复，不保证顺序。
        /// Get a hexagram by its name.
        /// If some hexagrams' names are duplicate, the order is not guaranteed.
        /// </summary>
        /// <param name="name">
        /// 卦名。
        /// The name.
        /// </param>
        /// <param name="result">
        /// 结果。
        /// The result.
        /// </param>
        /// <returns>
        /// 一个值，指示是否获取成功。
        /// A value indicates whether the hexagram has been found or not.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 是 <c>null</c> 。
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        public bool TryGetHexagram(string name, [MaybeNullWhen(false)] out ZhouyiHexagram result)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            var index = this.trigramsAndHexagrams.TryGetHexagram(name, out var t);
            if (index != -1)
            {
                Debug.Assert(t is not null);
                var tri = Maps.HexagramsToTrigrams.GetTrigrams(index);
                result = new ZhouyiHexagram(this.patternsAndNumbers,
                    index,
                    t.Name,
                    t.Text,
                    t.ApplyNinesOrSixes,
                    this.trigramsAndHexagrams.GetTrigram(tri.lower),
                    this.trigramsAndHexagrams.GetTrigram(tri.upper),
                    t.Lines);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// 通过卦画获取经卦。
        /// Get a trigram by its painting.
        /// </summary>
        /// <param name="painting">
        /// 卦画。
        /// The painting.
        /// </param>
        /// <returns>
        /// 结果。
        /// The result.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="painting"/> 是 <c>null</c> 。
        /// <paramref name="painting"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="painting"/> 不表示一个经卦。
        /// <paramref name="painting"/> can't represent a trigram.
        /// </exception>
        public ZhouyiTrigram GetTrigram(Core.Painting painting)
        {
            if (painting is null)
                throw new ArgumentNullException(nameof(painting));
            if (painting.Count != 3)
                throw new ArgumentException(
                    $"{nameof(painting)} should represent a trigram.",
                    nameof(painting));

            int d = 0;
            for (int i = 0; i < 3; i++)
            {
                d = d << 1;
                d += Convert.ToInt32(painting[i] == Core.YinYang.Yin);
            }
            return this.trigramsAndHexagrams.GetTrigram(d + 1);
        }

        /// <summary>
        /// 通过卦名获取经卦。
        /// 如果有卦名重复，不保证顺序。
        /// Get a trigram by its name.
        /// If some trigrams' names are duplicate, the order is not guaranteed.
        /// </summary>
        /// <param name="name">
        /// 卦名。
        /// The name.
        /// </param>
        /// <param name="result">
        /// 结果。
        /// The result.
        /// </param>
        /// <returns>
        /// 一个值，指示是否获取成功。
        /// A value indicates whether the trigram has been found or not.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 是 <c>null</c> 。
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        public bool TryGetTrigram(string name, [MaybeNullWhen(false)] out ZhouyiTrigram result)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            return this.trigramsAndHexagrams.TryGetTrigramByName(name, out result);
        }
    }
}
