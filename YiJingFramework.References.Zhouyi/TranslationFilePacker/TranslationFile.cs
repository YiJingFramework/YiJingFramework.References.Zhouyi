using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TranslationFilePacker.TranslationFile;
using static TranslationFilePacker.TranslationFile.TranslationFileHexagrams;

namespace TranslationFilePacker
{
    internal sealed record TranslationFile(
        TranslationFileTrigrams Trigrams,
        TranslationFileNumbers Numbers,
        TranslationFilePatterns Patterns,
        TranslationFileHexagrams Hexagrams)
    {
        internal bool Check()
        {
            return this.Trigrams.Check() &&
                this.Numbers.Check() &&
                this.Patterns.Check() &&
                this.Hexagrams.Check();
        }
        private static bool CheckLongerAndNullValue<T>(T[] array, int length) where T : class
        {
            return array is not null &&
                array.Length >= length &&
                !array.Contains(null);
        }
        private static bool CheckLengthAndNullValue<T>(T[] array, int length) where T : class
        {
            return array is not null &&
                array.Length == length &&
                !array.Contains(null);
        }
        internal sealed record TranslationFileTrigrams(string[] Names, string[] Natures)
        {
            internal bool Check()
            {
                return CheckLengthAndNullValue(this.Names, 8) && CheckLengthAndNullValue(this.Natures, 8);
            }
        }
        internal sealed record TranslationFileNumbers(string[] Ordinal)
        {
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
            internal sealed record TranslationFileHexagramNameAndTexts(
                string[] Lines,
                string Name,
                string Text)
            {
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
