using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
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
    ICollection<TCompoundValue> CreateMany<TCompoundValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null);
  }
}