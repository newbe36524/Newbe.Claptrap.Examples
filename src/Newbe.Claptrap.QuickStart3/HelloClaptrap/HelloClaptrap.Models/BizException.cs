using System;

namespace HelloClaptrap.Models
{
    [Serializable]
    public class BizException : Exception
    {
        public BizException()
            : this("something error in business")
        {
        }

        public BizException(string message) : base(message)
        {
        }

        public BizException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}