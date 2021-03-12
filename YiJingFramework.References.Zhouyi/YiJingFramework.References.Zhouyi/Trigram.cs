using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi
{
    public sealed class Trigram
    {
        public int Index { get; }
        public string Name { get; }
        internal Trigram(int index, string name)
        {
            this.Index = index;
            this.Name = name;
        }

        public Core.Painting GetPainting()
        {
            var index = this.Index - 1;
            var first = index >> 2;
            var second = index >> 1 - first * 2;
            var third = index - first * 4 - second * 2;
            return new Core.Painting(
                first == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin,
                second == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin,
                third == 0 ? Core.LineAttribute.Yang : Core.LineAttribute.Yin);
        }
    }
}
