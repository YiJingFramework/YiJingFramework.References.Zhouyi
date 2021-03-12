using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi.Exceptions
{

    [Serializable]
    public class CannotReadTranslationException : Exception
    {
        public CannotReadTranslationException(string? message = null, Exception? inner = null)
            : base(message, inner) { }
        protected CannotReadTranslationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
