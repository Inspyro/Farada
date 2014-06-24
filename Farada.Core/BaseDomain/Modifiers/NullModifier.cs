using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Farada.Core.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.BaseDomain.Modifiers
{
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
      if (context.PropertyType.IsCompoundType() || context.PropertyType.IsValueType)
        return instances;

      if (context.Property != null
          && (context.Property.IsDefined(typeof (RequiredAttribute))))
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