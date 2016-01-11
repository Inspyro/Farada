using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A general value provider that can provide an object to nearly every object. 
  /// -> That has either a default ctor (public empty)
  /// -> Or a ctor with parameters that can be automatically mapped to properties (immutable DTO). 
  /// --> This mapping happens with the <see cref="IDomainConfigurator.UseParameterToPropertyConversion"/> func. 
  /// </summary>
  public class DefaultInstanceValueProvider<TMember> : SubTypeValueProvider<TMember, ValueProviderContext<TMember>>
  {
    protected override IEnumerable<TMember> CreateManyValues (
        ValueProviderContext<TMember> context,
        [CanBeNull] IList<object> metadatas, int itemCount)
    {
      var typeInfo = context.Advanced.FastReflection.GetTypeInfo (context.Advanced.Key.Type);
      var ctorValuesCollections = InitializeCtorValues (itemCount, typeInfo);

      var ctorMembers = typeInfo.CtorArguments.Select (
          c =>c.ToMember (context.Advanced.ParameterConversionService)).ToList();

      var sortedCtorMembers = context.Advanced.MemberSorter.Sort (ctorMembers, context.Advanced.Key).ToList();

      //Note: We use a dictionary because when we have many ctor members, we gain some performance.
      var toUnsortedDictionary = ctorMembers.Select ((value, index) => new { value, index }).ToDictionary (x => x.value, x => x.index);
      var sortedToUnsorted = sortedCtorMembers.Select (member => toUnsortedDictionary[member]).ToList();

      List<object> resolvedMetadatas = null;
      for (var argumentIndex = 0; argumentIndex < sortedCtorMembers.Count; argumentIndex++)
      {
        var ctorMember = sortedCtorMembers[argumentIndex];
        var unsortedArgumentIndex = sortedToUnsorted[argumentIndex];

        var memberKey = context.Advanced.Key.CreateKey (ctorMember);

        //we only resolve once per parent (not for each member), so the data is shared between all members.
        if (resolvedMetadatas == null && context.Advanced.MetadataResolver.NeedsMetadata (memberKey))
        {
          var dependendArguments = sortedCtorMembers.Take (argumentIndex).ToList();

          var metadataContexts = GetMetadataContexts (
              context.Advanced.Key,
              dependendArguments,
              ctorValuesCollections,
              sortedToUnsorted,
              context.TestDataGenerator).ToList();

          resolvedMetadatas = context.Advanced.MetadataResolver.Resolve (memberKey, metadataContexts).ToList();
        }

        //Note: Here we have a recursion to the compound value provider. e.g. other immutable types could be a ctor argument
        var ctorMemberValues = context.Advanced.AdvancedTestDataGenerator.CreateMany (memberKey, resolvedMetadatas, itemCount, maxRecursionDepth: 2);
        for (var valueIndex = 0; valueIndex < ctorMemberValues.Count; valueIndex++)
        {
          ctorValuesCollections[valueIndex][unsortedArgumentIndex] = ctorMemberValues[valueIndex];
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

    private IEnumerable<MetadataObjectContext> GetMetadataContexts (
        IKey baseKey,
        IList<IFastMemberWithValues> dependendArguments,
        object[][] ctorValuesCollections,
        IReadOnlyList<int> sortedToUnsorted,
        ITestDataGenerator testDataGenerator)
    {
      var dependendArgumentMapping = new List<Tuple<int, IKey>>();
      for (var i = 0; i < dependendArguments.Count; i++)
      {
        var argument = dependendArguments[i];
        var argumentKey = baseKey.CreateKey (argument);
        var argumentIndex = sortedToUnsorted[i];

        dependendArgumentMapping.Add (new Tuple<int, IKey> (argumentIndex, argumentKey));
      }

      foreach (var valueCollection in ctorValuesCollections)
      {
        var context = new MetadataObjectContext(testDataGenerator);
        foreach (var dependendArgument in dependendArgumentMapping)
        {
          context.Add (dependendArgument.Item2, valueCollection[dependendArgument.Item1]);
        }

        yield return context;
      }
    }

    protected override ValueProviderContext<TMember> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TMember> (objectContext);
    }

    protected override TMember CreateValue (ValueProviderContext<TMember> context)
    {
      //we implement it like this, to be able to make some performance optimizations in the create many method.
      return CreateManyValues (context, context.InternalMetadata == null ? null : new[] { context.InternalMetadata }, 1).Single();
    }

    //we indicate that we don't fill any types, as this provider just creates blank instances by default.
    //however, immutable types are filled by default (because there is no other ctor). 
    //Autofill will however skip those ctor params.
    public override ValueFillMode FillMode
    {
      get { return ValueFillMode.FillNone; }
    }
  }
}