using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YiJingFramework.References.Zhouyi.Translations.TranslationFile;
using static YiJingFramework.References.Zhouyi.Translations.TranslationFile.TranslationFileHexagrams;

namespace YiJingFramework.References.Zhouyi.Translations
{
    internal sealed record TranslationFile(
        TranslationFileTrigrams Trigrams,
        TranslationFileNumbers Numbers,
        TranslationFilePatterns Patterns,
        TranslationFileHexagrams Hexagrams)
    {
        internal static TranslationFile Empty =>
            new TranslationFile(
                TranslationFileTrigrams.Empty,
                TranslationFileNumbers.Empty,
                TranslationFilePatterns.Empty,
                TranslationFileHexagrams.Empty);
        internal bool Check()
        {
            return this.Trigrams.Check() &&
                this.Numbers.Check() &&
                this.Patterns.Check() &&
                this.Hexagrams.Check();
        }
        private static bool CheckLongerAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length >= length &&
                !array.Contains(null);
        }
        private static bool CheckLengthAndNullValue<T>(T?[] array, int length) where T : class
        {
            return array is not null &&
                array.Length == length &&
                !array.Contains(null);
        }
        internal sealed record TranslationFileTrigrams(string[] Names, string[] Natures)
        {
            internal static TranslationFileTrigrams Empty
            {
                get
                {
                    var eightEmptyStrings = new string[] {
                        string.Empty, string.Empty, string.Empty, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty
                    };
                    return new TranslationFileTrigrams(eightEmptyStrings, eightEmptyStrings);
                }
            }
            internal bool Check()
            {
                return CheckLengthAndNullValue(this.Names, 8) && CheckLengthAndNullValue(this.Natures, 8);
            }
        }
        internal sealed record TranslationFileNumbers(string[] Ordinal)
        {
            internal static TranslationFileNumbers Empty
            {
                get
                {
                    var sixtyFourEmptyStrings = new string[64];
                    Array.Fill(sixtyFourEmptyStrings, string.Empty);
                    return new TranslationFileNumbers(sixtyFourEmptyStrings);
                }
            }
            internal bool Check()
            {
                return CheckLongerAndNullValue(this.Ordinal, 64);
            }
        }
        internal sealed record TranslationFilePatterns(
            string[] YinLines,
            string[] YangLines,
            string HexagramsToString,
            string PureHexagramsToString,
            string ApplyNines,
            string ApplySixes)
        {
            internal static TranslationFilePatterns Empty
            {
                get
                {
                    var sixEmptyStrings = new string[] {
                        string.Empty, string.Empty, string.Empty,
                        string.Empty, string.Empty, string.Empty
                    };
                    return new TranslationFilePatterns(sixEmptyStrings, sixEmptyStrings,
                        string.Empty, string.Empty, string.Empty, string.Empty);
                }
            }
            internal bool Check()
            {
                return this.HexagramsToString is not null &&
                    this.PureHexagramsToString is not null &&
                    this.ApplyNines is not null &&
                    this.ApplySixes is not null &&
                    CheckLengthAndNullValue(this.YinLines, 6) && CheckLengthAndNullValue(this.YangLines, 6);
            }
        }
        internal sealed record TranslationFileHexagrams(
            TranslationFileHexagramNameAndTexts[] NamesAndTexts,
            string ApplyNines,
            string ApplySixes)
        {
            internal static TranslationFileHexagrams Empty
            {
                get
                {
                    var sixtyFourEmptyNamesAndTexts = new TranslationFileHexagramNameAndTexts[64];
                    Array.Fill(sixtyFourEmptyNamesAndTexts, TranslationFileHexagramNameAndTexts.Empty);
                    return new TranslationFileHexagrams(
                        sixtyFourEmptyNamesAndTexts, string.Empty, string.Empty);
                }
            }
            internal sealed record TranslationFileHexagramNameAndTexts(
                string[] Lines,
                string Name,
                string Text)
            {
                internal static TranslationFileHexagramNameAndTexts Empty
                {
                    get
                    {
                        return new TranslationFileHexagramNameAndTexts(
                            new string[] {
                                string.Empty, string.Empty, string.Empty,
                                string.Empty, string.Empty, string.Empty
                            }, string.Empty, string.Empty);
                    }
                }
                internal bool Check()
                {
                    return this.Name is not null &&
                        this.Text is not null &&
                        CheckLengthAndNullValue(this.Lines, 6);
                }
            }
            internal bool Check()
            {
                if (this.ApplyNines is not null &&
                    this.ApplySixes is not null &&
                    CheckLengthAndNullValue(this.NamesAndTexts, 64))
                {
                    foreach (var nat in this.NamesAndTexts)
                    {
                        if (!nat.Check())
                            return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
