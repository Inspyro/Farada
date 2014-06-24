using System;
using System.Collections.Generic;

namespace Farada.Core.Modifiers
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