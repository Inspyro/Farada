using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Fluent;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A general value provider that can provide an object to nearly every object. 
  /// -> That has either a default ctor (public empty)
  /// -> Or a ctor with parameters that can be automatically mapped to properties (immutable DTO). 
  /// --> This mapping happens with the <see cref="IDomainConfigurator.UseParameterToPropertyConversion"/> func. 
  /// </summary>
  public class DefaultInstanceValueProvider<TMember> : SubTypeValueProvider<TMember>
  {
    protected override IEnumerable<TMember> CreateManyValues (ValueProviderContext<TMember> context, int numberOfObjects)
    {
      var typeInfo = FastReflectionUtility.GetTypeInfo (context.Advanced.Key.Type);

      var ctorValuesCollections = new object[numberOfObjects][];
      for (var i = 0; i < ctorValuesCollections.Length; i++)
      {
        ctorValuesCollections[i] = new object[typeInfo.CtorArguments.Count];
      }

      for (var argumentIndex = 0; argumentIndex < typeInfo.CtorArguments.Count; argumentIndex++)
      {
        var ctorArgument = typeInfo.CtorArguments[argumentIndex];

        //Note: Here we have a recursion to the compound value provider. e.g. other immutable types could be a ctor argument
        var ctorArgumentValues = context.Advanced.AdvancedTestDataGenerator.CreateMany (
            context.Advanced.Key.CreateKey (ctorArgument.ToMember (context.Advanced.ParameterConversionService)),
            numberOfObjects,
            2);

        for (var valueIndex = 0; valueIndex < ctorArgumentValues.Count; valueIndex++)
        {
          ctorValuesCollections[valueIndex][argumentIndex] = ctorArgumentValues[valueIndex];
        }
      }

      var typeFactoryWithArguments = FastActivator.GetFactory (context.Advanced.Key.Type, typeInfo.CtorArguments);
      return ctorValuesCollections.Select (ctorValues => typeFactoryWithArguments (ctorValues)).Cast<TMember>();
    }

    protected override TMember CreateValue (ValueProviderContext<TMember> context)
    {
      //we implement it like this, to be able to make some performance optimizations in the create many method.
      return CreateManyValues (context, 1).Single();
    }
  }
}