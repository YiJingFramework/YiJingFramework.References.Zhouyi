using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.References.Zhouyi.Exceptions
{
    /// <summary>
    /// 读取翻译失败。
    /// Cannot read the translations.
    /// </summary>
    [Serializable]
    public class CannotReadTranslationException : Exception
    {
        /// <summary>
        /// 创建一个新实例。
        /// Initialize a new instance.
        /// </summary>
        /// <param name="message">
        /// 消息。
        /// The message.
        /// </param>
        /// <param name="inner">
        /// 内部异常。
        /// The inner exception.
        /// </param>
        public CannotReadTranslationException(string? message = null, Exception? inner = null)
            : base(message, inner) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CannotReadTranslationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
