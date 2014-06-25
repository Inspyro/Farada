using System;
using System.Collections.Generic;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastTypeInfo:IFastTypeInfo
  {
    public IList<IFastPropertyInfo> Properties { get; private set; }

    internal FastTypeInfo(IList<IFastPropertyInfo> properties )
    {
      Properties = properties;
    }
  }

  /// <summary>
  /// Provides a faster way to access a types properties than <see cref="Type.GetProperties()"/>
  /// </summary>
  public interface IFastTypeInfo
  {
    /// <summary>
    /// The properties of the type
    /// </summary>
    IList<IFastPropertyInfo> Properties { get; }
  }
}