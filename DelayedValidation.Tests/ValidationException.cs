namespace DelayedValidation
{
    using System;
    using System.Runtime.Serialization;

    public class ValidationException : Exception
    {
        #region Constructors and Destructors

        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}