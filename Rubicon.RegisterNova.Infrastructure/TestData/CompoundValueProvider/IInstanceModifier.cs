using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO: document
  /// </summary>
  public interface IInstanceModifier
  {
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    /// <param name="instances"></param>
    /// <returns></returns>
    IList<object> Modify (ModificationContext context, IList<object> instances);
  }
}