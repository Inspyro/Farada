using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Fluent;
using JetBrains.Annotations;

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
    protected override IEnumerable<TMember> CreateManyValues (
        ValueProviderContext<TMember> context,
        [CanBeNull] IList<DependedPropertyCollection> dependedProperties, int itemCount)
    {
      var typeInfo = FastReflectionUtility.GetTypeInfo (context.Advanced.Key.Type);
      var ctorValuesCollections = InitializeCtorValues (itemCount, typeInfo);

      var ctorMembers = typeInfo.CtorArguments.Select (
          c => c.ToMember (context.Advanced.ParameterConversionService)).ToList();

      var sortedCtorMembers = ctorMembers.TopologicalSort (
          ctorMember => GetDependencies (context, ctorMember),
          throwOnCycle: true).ToList();

      for (var argumentIndex = 0; argumentIndex < sortedCtorMembers.Count; argumentIndex++)
      {
        var ctorMember = sortedCtorMembers[argumentIndex];

        var dependedArguments = dependedProperties == null
            ? null
            : ResolveDependendArguments (context, ctorMember, argumentIndex, typeInfo, ctorValuesCollections)?.ToList();

        //Note: Here we have a recursion to the compound value provider. e.g. other immutable types could be a ctor argument
        var ctorMemberValues = context.Advanced.AdvancedTestDataGenerator.CreateMany (
            context.Advanced.Key.CreateKey (ctorMember),
            dependedArguments,
            itemCount,
            2);

        for (var valueIndex = 0; valueIndex < ctorMemberValues.Count; valueIndex++)
        {
          ctorValuesCollections[valueIndex][argumentIndex] = ctorMemberValues[valueIndex];
        }
      }

      var typeFactoryWithArguments = FastActivator.GetFactory (context.Advanced.Key.Type, typeInfo.CtorArguments);
      return ctorValuesCollections.Select (ctorValues => typeFactoryWithArguments (ctorValues)).Cast<TMember>();
    }

    private static object[][] InitializeCtorValues (int itemCount, IFastTypeInfo typeInfo)
    {
      var ctorValuesCollections = new object[itemCount][];
      for (var i = 0; i < ctorValuesCollections.Length; i++)
      {
        ctorValuesCollections[i] = new object[typeInfo.CtorArguments.Count];
      }
      return ctorValuesCollections;
    }

    [CanBeNull]
    private IEnumerable<DependedPropertyCollection> ResolveDependendArguments (ValueProviderContext<TMember> context, IFastMemberWithValues ctorMember, int targetArgumentIndex, IFastTypeInfo typeInfo, object[][] ctorValuesCollections)
    {
      var ctorDependencies = GetDependencies (context, ctorMember).ToList();
      if (!ctorDependencies.Any())
        return null;

      var dependendPropertyList = new List<DependedPropertyCollection>();
      for (var valueIndex = 0; valueIndex < ctorValuesCollections.GetLength (0); valueIndex++)
      {
        var dependendProperties = new DependedPropertyCollection();

        for (var argumentIndex = 0; argumentIndex < targetArgumentIndex; argumentIndex++)
        {
          var otherCtorMember =
              context.Advanced.Key.CreateKey (typeInfo.CtorArguments[argumentIndex].ToMember (context.Advanced.ParameterConversionService));

          if(!ctorDependencies.Contains(otherCtorMember.Member))
            continue;

          dependendProperties.Add (otherCtorMember, ctorValuesCollections[valueIndex][argumentIndex]);
        }

        dependendPropertyList.Add (dependendProperties);
      }

      return dependendPropertyList;
    }

    protected override TMember CreateValue (ValueProviderContext<TMember> context)
    {
      //we implement it like this, to be able to make some performance optimizations in the create many method.
      return CreateManyValues (context, new[] { context.PropertyCollection }, 1).Single();
    }

    private IEnumerable<IFastMemberWithValues> GetDependencies (ValueProviderContext<TMember> context, IFastMemberWithValues ctorMember)
    {
      var memberKey = context.Advanced.Key.CreateKey (ctorMember);
      if (!context.Advanced.DependencyMapping.ContainsKey (memberKey))
        yield break;

      foreach (var dependency in context.Advanced.DependencyMapping[memberKey].Select (k => k.Member))
        yield return dependency;
    }
  }
}