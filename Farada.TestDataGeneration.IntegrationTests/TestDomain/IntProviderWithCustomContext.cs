using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class IntProviderWithCustomContext : ExtendedValueProvider<int, int>
  {
    readonly int _contextValue;

    public IntProviderWithCustomContext (int contextValue)
    {
      _contextValue = contextValue;
    }

    protected override int CreateValue (ExtendedValueProviderContext<int, int> context)
    {
      return context.AdditionalData;
    }

    protected override int CreateData (ValueProviderObjectContext objectContext)
    {
      // Note: Here we would use some ctor data + the object context, where there is all the relevant member info etc.
      // So we could read attributes etc.
      return _contextValue;
    }
  }
}