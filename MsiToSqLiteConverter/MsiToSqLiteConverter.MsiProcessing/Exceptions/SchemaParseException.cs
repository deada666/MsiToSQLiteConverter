namespace MsiToSqLiteConverter.MsiProcessing.Exceptions
{
    using System;

    /// <summary>
    /// The schema parse exception.
    /// </summary>
    public class SchemaParseException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaParseException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public SchemaParseException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaParseException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public SchemaParseException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}