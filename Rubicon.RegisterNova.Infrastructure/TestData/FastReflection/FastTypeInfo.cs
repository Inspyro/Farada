using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
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