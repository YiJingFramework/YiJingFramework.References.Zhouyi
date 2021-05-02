using System;
using System.Linq;
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
        internal bool CheckAndWrite()
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
                if (!CheckLengthAndNullValue(this.Names, 8))
                {
                    Console.WriteLine("There should be the translation for 8 trigrams' names.");
                    return false;
                }
                if (!CheckLengthAndNullValue(this.Natures, 8))
                {
                    Console.WriteLine("There should be the translation for 8 trigrams' natures.");
                    return false;
                }
                return true;
            }
        }
        internal sealed record TranslationFileNumbers(string[] Ordinal)
        {
            internal bool Check()
            {
                if (!CheckLongerAndNullValue(this.Ordinal, 64))
                {
                    Console.WriteLine("There should be the translation for 64 ordinal numbers.");
                    return false;
                }
                return true;
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
                if (this.HexagramsToString is null)
                {
                    Console.WriteLine("There should be the HexagramsToString pattern.");
                    return false;
                }
                if (this.PureHexagramsToString is null)
                {
                    Console.WriteLine("There should be the PureHexagramsToString pattern.");
                    return false;
                }
                if (this.ApplyNines is null)
                {
                    Console.WriteLine("There should be the ApplyNines pattern.");
                    return false;
                }
                if (this.ApplySixes is null)
                {
                    Console.WriteLine("There should be the ApplySixes pattern.");
                    return false;
                }
                if (!CheckLengthAndNullValue(this.YinLines, 6))
                {
                    Console.WriteLine("There should be patterns for the 6 yin lines.");
                    return false;
                }
                if (!CheckLengthAndNullValue(this.YangLines, 6))
                {
                    Console.WriteLine("There should be patterns for the 6 yang lines.");
                    return false;
                }
                return true;
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
                internal bool Check(int n)
                {
                    if (this.Name is null)
                    {
                        Console.WriteLine($"There should be the translation for the name of the hexagram {n + 1}.");
                        return false;
                    }
                    if (this.Text is null)
                    {
                        Console.WriteLine($"There should be the translation for the text of the hexagram {n + 1}.");
                        return false;
                    }
                    if (!CheckLengthAndNullValue(this.Lines, 6))
                    {
                        Console.WriteLine($"There should be the translation for the 6 lines of the hexagram {n + 1}.");
                        return false;
                    }
                    return true;
                }
            }
            internal bool Check()
            {
                if (this.ApplyNines is null)
                {
                    Console.WriteLine("There should be the translation for the apply nines.");
                    return false;
                }
                if (this.ApplySixes is null)
                {
                    Console.WriteLine("There should be the translation for the apply nines.");
                    return false;
                }
                if (!CheckLengthAndNullValue(this.NamesAndTexts, 64))
                {
                    Console.WriteLine("There should be the translation for the 64 hexagrams.");
                    return false;
                }
                for (int i = 0; i < 64; i++)
                {
                    var nat = this.NamesAndTexts[i];
                    if (!nat.Check(i))
                        return false;
                }
                return true;
            }
        }
    }
}
