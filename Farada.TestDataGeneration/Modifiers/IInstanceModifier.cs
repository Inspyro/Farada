using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Modifiers
{
  /// <summary>
  /// Provides functionality to modifiy instances that where previously filled by an <see cref="ITestDataGenerator"/>
  /// </summary>
  public interface IInstanceModifier
  {
    /// <summary>
    /// This method is called after the given instances where filled, in order for the InstanceModifier to modify the instances
    /// </summary>
    /// <param name="context">The <see cref="ModificationContext"/> under which the instances should be modified</param>
    /// <param name="instances">The instances to modify</param>
    /// <returns>The modified instances</returns>
    IList<object> Modify (ModificationContext context, IList<object> instances);
  }
}