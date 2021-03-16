using System;
using System.Collections.Generic;
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
        internal Numbers NumberTranslations { get; }
        internal Grams GramsTranslations { get; }
        internal Patterns Patterns { get; }
        internal Texts TextTranslations { get; }

        /// <summary>
        /// 创建新实例。
        /// 默认使用的翻译目录为 <c>./zhouyi/translations</c> 。
        /// Initialize a new instance.
        /// The default translations directory is <c>./zhouyi/translations</c>.
        /// </summary>
        /// <exception cref="CannotReadTranslationException">
        /// 读取翻译失败。
        /// Cannot read the translations.
        /// </exception>
        public Zhouyi()
            : this(new DirectoryInfo(Path.GetFullPath("zhouyi/translations", AppContext.BaseDirectory))) { }

        /// <summary>
        /// 创建新实例。
        /// Initialize a new instance.
        /// </summary>
        /// <param name="translationsDirectory">
        /// 翻译目录。
        /// The translations directory.
        /// </param>
        /// <exception cref="CannotReadTranslationException">
        /// 读取翻译失败。
        /// Cannot read the translations.
        /// </exception>
        public Zhouyi(DirectoryInfo translationsDirectory)
        {
            if (!translationsDirectory.Exists)
                throw new CannotReadTranslationException($"Cannot find directory: {translationsDirectory.FullName}");

            JsonSerializerOptions baseOptions = new JsonSerializerOptions() {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            {
                FileInfo fileInfo = new FileInfo(
                    Path.Join(translationsDirectory.FullName, "numbers.json"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find file: {fileInfo.FullName}");
                this.NumberTranslations = new Numbers(fileInfo, baseOptions);
            }
            {
                FileInfo fileInfo = new FileInfo(
                    Path.Join(translationsDirectory.FullName, "grams.json"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find file: {fileInfo.FullName}");
                this.GramsTranslations = new Grams(fileInfo, baseOptions);
            }
            {
                FileInfo fileInfo = new FileInfo(
                    Path.Join(translationsDirectory.FullName, "patterns.json"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find file: {fileInfo.FullName}");
                this.Patterns = new Patterns(fileInfo, baseOptions, this.NumberTranslations);
            }
            {
                DirectoryInfo fileInfo = new DirectoryInfo(
                    Path.Join(translationsDirectory.FullName, "texts"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find directory: {fileInfo.FullName}");
                this.TextTranslations = new Texts(fileInfo, baseOptions);
            }
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

            return new ZhouyiHexagram(this, painting);
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
            if (this.GramsTranslations.TryGetHexagramIndex(name, out var t))
            {
                result = new ZhouyiHexagram(this, t, name);
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
            return this.GramsTranslations.GetTrigram(d + 1);
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
            return this.GramsTranslations.TryGetTrigramByName(name, out result);
        }
    }
}
