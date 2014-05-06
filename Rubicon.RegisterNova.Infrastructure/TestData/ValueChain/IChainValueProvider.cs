using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public interface IChainValueProvider
  {
    bool HasValue ();
    object GetValue();

    void SetProvider (IValueProvider valueProvider);
    IChainValueProvider SetChainProvider(IValueProvider valueProvider, Type providerType, string nameFilter=null);
    bool HasChainProvider(Type providerType, string nameFilter=null);
    IChainValueProvider GetChainProvider(Type providerType, string nameFilter);
    Random Random { get; }
  }
}