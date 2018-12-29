using System;
using System.Runtime.Serialization;

namespace NClass.AssemblyImport
{
  [Serializable]
  public class UnsafeTypesPresentException : Exception
  {
    public UnsafeTypesPresentException() { }
    public UnsafeTypesPresentException(string message) : base(message) { }
    public UnsafeTypesPresentException(string message, Exception inner) : base(message, inner) { }

    protected UnsafeTypesPresentException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    { }
  }
}