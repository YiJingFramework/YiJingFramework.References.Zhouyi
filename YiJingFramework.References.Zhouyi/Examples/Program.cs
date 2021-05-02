using System;
using System.Diagnostics;
using System.IO;
using YiJingFramework.References.Zhouyi;

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            // Here we read the translation from the resource.
            // You can also save the translation as a file and pass the it's path here.
            using var stream = new MemoryStream(Properties.Resources.translation);
            Zhouyi zhouyi = new Zhouyi(stream);

            _ = zhouyi.TryGetHexagram("同人", out var tongRen);
            Debug.Assert(tongRen is not null);
            Console.WriteLine(tongRen);
            Console.WriteLine(tongRen.SecondLine.YinYang);
            Console.WriteLine(tongRen.UpperTrigram.Nature);
            Console.WriteLine();
            // Outputs: 第十三卦 同人 天火同人 乾上离下
            // 同人于野，亨。利涉大川，利君子贞。
            // 初九：同人于门，无咎。
            // 六二：同人于宗，吝。
            // 九三：伏戎于莽，升其高陵，三岁不兴。
            // 九四：乘其墉，弗克攻，吉。
            // 九五：同人，先号啕而后笑。大师克相遇。
            // 上九：同人于郊，无悔。
            // 
            // Yin
            // 天

            var tongRen2 = zhouyi.GetHexagram(new YiJingFramework.Core.Painting(
                YiJingFramework.Core.YinYang.Yang,
                YiJingFramework.Core.YinYang.Yin,
                YiJingFramework.Core.YinYang.Yang,
                YiJingFramework.Core.YinYang.Yang,
                YiJingFramework.Core.YinYang.Yang,
                YiJingFramework.Core.YinYang.Yang
                ));
            Console.WriteLine(tongRen.Equals(tongRen2));
            Console.WriteLine();
            // Output: True
        }
    }
}
