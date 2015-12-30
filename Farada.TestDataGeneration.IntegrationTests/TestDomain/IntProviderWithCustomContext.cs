using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class IntProviderWithCustomContext : ValueProvider<int, ExtendedValueProviderContext<int, int>>
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
    
    protected override ExtendedValueProviderContext<int, int> CreateContext(ValueProviderObjectContext objectContext)
    {
      return new ExtendedValueProviderContext<int, int>(objectContext, _contextValue);
    }
  }
}