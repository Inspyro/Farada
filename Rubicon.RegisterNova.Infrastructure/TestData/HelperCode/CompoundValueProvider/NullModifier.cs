using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Internal;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;
using Rubicon.RegisterNova.Infrastructure.Validation;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider
{
  public class NullModifier : IInstanceModifier
  {
    readonly double _nullProbability;

    /// <summary>
    /// TODO: document, nullProbability from 0.0 to 1.0
    /// </summary>
    /// <param name="nullProbability"></param>
    public NullModifier (double nullProbability)
    {
      _nullProbability = nullProbability;
    }

    public IList<object> Modify (ModificationContext context, IList<object> instances)
    {
      if (context.PropertyType.IsCompoundType() || context.PropertyType.IsValueType)
        return instances;

      if (context.Property != null
          && (context.Property.IsDefined(typeof (RequiredAttribute)) || context.Property.IsDefined(typeof (NeverNullAttribute))))
      {
        return instances;
      }

      var instancesToNull = (int) (instances.Count * _nullProbability);

      for(var i=0;i<instancesToNull;i++)
        instances[i] = null;

      instances.Randomize(context.Random);
      return instances;
    }
  }
}