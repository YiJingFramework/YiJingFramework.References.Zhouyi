using System;
using YiJingFramework.Core;
using YiJingFramework.FiveElements;

namespace YiJingFramework.References.Zhouyi.Maps
{
    internal sealed class TrigramsYinYangsAndFiveElementsMap
    {
#pragma warning disable CA1822 // 将成员标记为 static
        internal (YinYang, FiveElement) GetYinYangAndFiveElement(int trigram)
#pragma warning restore CA1822 // 将成员标记为 static
        {
            return trigram switch {
                0 => (YinYang.Yang, FiveElement.Metal),
                1 => (YinYang.Yin, FiveElement.Metal),
                2 => (YinYang.Yin, FiveElement.Fire),
                3 => (YinYang.Yang, FiveElement.Wood),
                4 => (YinYang.Yin, FiveElement.Wood),
                5 => (YinYang.Yang, FiveElement.Water),
                6 => (YinYang.Yang, FiveElement.Earth),
                7 => (YinYang.Yin, FiveElement.Earth),
                _ => throw new Exception("This line should never be executed.")
            };
        }
    }
}
