using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiJingFramework.References.Zhouyi.Exceptions;

namespace YiJingFramework.References.Zhouyi.Translations
{
    internal sealed class TrigramsAndHexagrams
    {
        internal sealed record ZhouyiHexagramValues(
            string Name, string Text,
            int Index, string[] Lines,
            string? ApplyNinesOrSixes);

        private readonly ZhouyiHexagramValues[] hexagrams;
        private readonly ZhouyiTrigram[] trigrams;

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ZhouyiTrigram GetTrigram(int index)
        {
            return this.trigrams[index - 1];
        }
        internal bool TryGetTrigramByName(string name, [MaybeNullWhen(false)] out ZhouyiTrigram trigram)
        {
            foreach (var tri in this.trigrams)
            {
                if (tri.Name == name)
                {
                    trigram = tri;
                    return true;
                }
            }
            trigram = null;
            return false;
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ZhouyiHexagramValues GetHexagram(int index)
        {
            return this.hexagrams[index - 1];
        }
        internal int TryGetHexagram
            (string name, out ZhouyiHexagramValues? translation)
        {
            for (int i = 0; i < 64; i++)
            {
                var d = this.hexagrams[i];
                if (d.Name == name)
                {
                    translation = d;
                    return i + 1;
                }
            }
            translation = null;
            return -1;
        }

        internal TrigramsAndHexagrams(
            TranslationFile.TranslationFileHexagrams hexagrams,
            TranslationFile.TranslationFileTrigrams trigrams)
        {
            this.trigrams = new ZhouyiTrigram[8];
            for (int i = 0; i < 8; i++)
            {
                this.trigrams[i] = new ZhouyiTrigram(i + 1, trigrams.Names[i], trigrams.Natures[i]);
            }

            this.hexagrams = new ZhouyiHexagramValues[64];
            for (int i = 0; i < 64; i++)
            {
                var nat = hexagrams.NamesAndTexts[i];
                this.hexagrams[i] = new ZhouyiHexagramValues(
                     nat.Name, nat.Text, i + 1, nat.Lines, i switch {
                         0 => hexagrams.ApplyNines,
                         1 => hexagrams.ApplySixes,
                         _ => null
                     });
            }
        }
    }
}