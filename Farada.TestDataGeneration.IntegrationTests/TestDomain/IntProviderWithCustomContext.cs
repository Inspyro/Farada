using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class IntProviderWithCustomContext : ExtendedValueProvider<int, int>
  {
    public IntProviderWithCustomContext (int contextValue)
        : base (contextValue)
    {
    }

    protected override int CreateValue (ExtendedValueProviderContext<int, int> context)
    {
      return context.AdditionalData;
    }
  }
}