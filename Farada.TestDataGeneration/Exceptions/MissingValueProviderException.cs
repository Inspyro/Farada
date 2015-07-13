using System;
using System.Runtime.Serialization;

namespace Farada.TestDataGeneration.Exceptions
{
  /// <summary>
  /// This exception is thrown when a value provider is required but is not registered.
  /// </summary>
  [Serializable]
  public class MissingValueProviderException : Exception
  {
    public MissingValueProviderException (string message)
        : base (message)
    {
    }

    protected MissingValueProviderException (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
    }
  }
}