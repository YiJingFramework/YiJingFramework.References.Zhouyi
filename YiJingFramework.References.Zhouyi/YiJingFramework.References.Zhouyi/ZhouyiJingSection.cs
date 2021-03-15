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
    /// 《周易》经部。
    /// The jing section of Zhouyi, Zhouyi's main part.
    /// </summary>
    public sealed class ZhouyiJingSection
    {
        public Numbers NumberTranslations { get; }
        internal Grams GramsTranslations { get; }
        internal Patterns Patterns { get; }
        internal Texts TextTranslations { get; }

        public ZhouyiJingSection()
            : this(new DirectoryInfo(Path.GetFullPath("zhouyi/translations", AppContext.BaseDirectory))) { }
        public ZhouyiJingSection(DirectoryInfo translationsDirectory)
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
                this.Patterns = new Patterns(fileInfo, baseOptions);
            }
            {
                DirectoryInfo fileInfo = new DirectoryInfo(
                    Path.Join(translationsDirectory.FullName, "texts"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find directory: {fileInfo.FullName}");
                this.TextTranslations = new Texts(fileInfo, baseOptions);
            }
        }

        public ZhouyiHexagram GetHexagram(Core.Painting painting)
        {
            if (painting.Count != 6)
                throw new ArgumentException(
                    $"{nameof(painting)} should represent a hexagram.",
                    nameof(painting));

            return new ZhouyiHexagram(this, painting);
        }
        public bool TryGetHexagram(string name, [MaybeNullWhen(false)] out ZhouyiHexagram result)
        {
            if (this.GramsTranslations.TryGetHexagramIndex(name, out var t))
            {
                result = new ZhouyiHexagram(this, t, name);
                return true;
            }
            result = null;
            return false;
        }
        public ZhouyiTrigram GetTrigram(Core.Painting painting)
        {
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

        public bool TryGetTrigram(string name, [MaybeNullWhen(false)] out ZhouyiTrigram result)
        {
            return this.GramsTranslations.TryGetTrigramByName(name, out result);
        }
    }
}
