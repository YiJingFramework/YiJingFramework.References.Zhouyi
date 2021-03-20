using System;
using System.Diagnostics;
using System.IO;
using YiJingFramework.References.Zhouyi;

namespace Examples
{
    class Program
    {
        static void Main()
        {
            // Here we read the translation from the resource.
            // You can also save the translation as a file and pass the it's path here.
            using var stream = new MemoryStream(Properties.Resources.translation);
            Zhouyi zhouyi = new Zhouyi(stream);

            _ = zhouyi.TryGetHexagram("比", out var bi);
            Debug.Assert(bi is not null);
            Console.WriteLine(bi);
            Console.WriteLine(bi.SecondLine.LineAttribute);
            Console.WriteLine(bi.UpperTrigram.Nature);
            Console.WriteLine();
            // Outputs: 第八卦 比 水地比 坎上坤下
            // 吉。原筮元永贞，无咎。不宁方来，后夫凶。
            // 初六：有孚比之，无咎。有孚盈缶，终来有他，吉。
            // 六二：比之自内，贞吉。
            // 六三：比之匪人。
            // 六四：外比之，贞吉。
            // 九五：显比，王用三驱，失前禽。邑人不诫，吉。
            // 上六：比之无首，凶。
            // 
            // Yin
            // 水

            var bi2 = zhouyi.GetHexagram(new YiJingFramework.Core.Painting(
                YiJingFramework.Core.LineAttribute.Yin,
                YiJingFramework.Core.LineAttribute.Yin,
                YiJingFramework.Core.LineAttribute.Yin,
                YiJingFramework.Core.LineAttribute.Yin,
                YiJingFramework.Core.LineAttribute.Yang,
                YiJingFramework.Core.LineAttribute.Yin
                ));
            Console.WriteLine(bi.Equals(bi2));
            Console.WriteLine();
            // Output: True
        }
    }
}
