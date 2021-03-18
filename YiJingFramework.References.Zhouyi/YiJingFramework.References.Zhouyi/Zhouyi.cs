﻿using System;
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
        public Zhouyi(string translationFilePath = "./zhouyi/translation.json")
        {
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
            var t = trigramsAndHexagrams.GetHexagram(index);
            return new ZhouyiHexagram(patternsAndNumbers,
                index,
                t.Name,
                t.Text,
                t.ApplyNinesOrSixes,
                l,
                u,
                t.Lines);
        }

        /// <summary>
        /// 通过卦名获取别卦。
        /// Get a hexagram by its name.
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
                result = new ZhouyiHexagram(patternsAndNumbers,
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
                d += Convert.ToInt32(painting[i] == Core.LineAttribute.Yin);
            }
            return this.trigramsAndHexagrams.GetTrigram(d + 1);
        }

        /// <summary>
        /// 通过卦名获取经卦。
        /// Get a trigram by its name.
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
