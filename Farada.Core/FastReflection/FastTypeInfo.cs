using System;
using System.Collections.Generic;

namespace Farada.Core.FastReflection
{
  public class FastTypeInfo:IFastTypeInfo
  {
    public IList<IFastPropertyInfo> Properties { get; private set; }

    internal FastTypeInfo(IList<IFastPropertyInfo> properties )
    {
      Properties = properties;
    }
  }

  public interface IFastTypeInfo
  {
    IList<IFastPropertyInfo> Properties { get; }
  }
}