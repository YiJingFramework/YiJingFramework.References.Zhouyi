using System;
using System.Collections.Generic;
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
        public MiscTranslations NumberTranslations { get; }
        public NamesTranslations NamesTranslations { get; }
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
                    Path.Join(translationsDirectory.FullName, "misc.json"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find file: {fileInfo.FullName}");
                this.NumberTranslations = new MiscTranslations(fileInfo, baseOptions);
            }
            {
                FileInfo fileInfo = new FileInfo(
                    Path.Join(translationsDirectory.FullName, "names.json"));
                if (!fileInfo.Exists)
                    throw new CannotReadTranslationException($"Cannot find file: {fileInfo.FullName}");
                this.NamesTranslations = new NamesTranslations(fileInfo, baseOptions);
            }

        }

        public Trigram GetTrigram(int index)
        {
            return new Trigram(index, this.NamesTranslations.GetTrigramName(index));
        }
    }
}
