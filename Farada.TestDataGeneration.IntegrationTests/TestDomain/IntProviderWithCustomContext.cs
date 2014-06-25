using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class IntProviderWithCustomContext:ValueProvider<int, IntProviderWithCustomContext.CustomIntContext>
  {
    readonly int _contextValue;

    public IntProviderWithCustomContext(int contextValue)
    {
      _contextValue = contextValue;
    }

    internal class CustomIntContext : ValueProviderContext<int>
    {
      public int ContextValue { get; private set; }

      public CustomIntContext (ValueProviderObjectContext objectContext, int contextValue)
          : base (objectContext)
      {
        ContextValue = contextValue;
      }
    }

    protected override CustomIntContext CreateContext (ValueProviderObjectContext objectContext)
    {
      return new CustomIntContext(objectContext, _contextValue);
    }

    protected override int CreateValue (CustomIntContext context)
    {
      return context.ContextValue;
    }
  }
}