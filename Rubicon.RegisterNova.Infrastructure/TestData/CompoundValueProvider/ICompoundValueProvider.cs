using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  public interface ICompoundValueProvider
  {
    /// <summary>
    /// TODO
    /// </summary>
    TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2);

    /// <summary>
    /// TODO
    /// </summary>
    ICollection<TCompoundValue> CreateMany<TCompoundValue> (int numberOfObjects, int maxRecursionDepth = 2);
  }
}