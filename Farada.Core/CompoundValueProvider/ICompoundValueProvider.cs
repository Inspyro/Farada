using System;
using System.Collections.Generic;
using Farada.Core.FastReflection;

namespace Farada.Core.CompoundValueProvider
{
  public interface ICompoundValueProvider
  {
    /// <summary>
    /// TODO
    /// </summary>
    TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null);

    /// <summary>
    /// TODO
    /// </summary>
    IReadOnlyList<TCompoundValue> CreateMany<TCompoundValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null);
  }
}