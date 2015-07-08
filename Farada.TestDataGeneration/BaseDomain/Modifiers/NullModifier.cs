using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.BaseDomain.Modifiers
{
  /// <summary>
  /// Modifies the instances so that some are null depending on a percentage given
  /// </summary>
  public class NullModifier : IInstanceModifier
  {
    readonly double _nullPercentage;

    /// <summary>
    /// Creates a new modifier that modifies exactly the percentage specified
    /// </summary>
    /// <param name="nullPercentage">This value ranges from 0.0 to 1.0 where 0.0 means 0% null and 1.0 means 100% null</param>
    public NullModifier (double nullPercentage)
    {
      _nullPercentage = nullPercentage;
    }

    public IList<object> Modify (ModificationContext context, IList<object> instances)
    {
      //TODO: Remove compound type check? - int? is filtered through "IsValueType"
      if (context.MemberType.IsCompoundType() || context.MemberType.IsValueType)
        return instances;

      if (context.Member != null
          && (context.Member.IsDefined(typeof (RequiredAttribute))))
      {
        return instances;
      }

      var instancesToNull = (int) (instances.Count * _nullPercentage);

      for(var i=0;i<instancesToNull;i++)
        instances[i] = null;

      instances.Randomize(context.Random);
      return instances;
    }
  }
}