using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ChainValueProvider:IChainValueProvider
  {
    private readonly Random _random;
    private IValueProvider _valueProvider;
    private readonly Dictionary<ChainKey, IChainValueProvider> _nextProviders;

    public ChainValueProvider(Random random, IValueProvider valueProvider=null)
    {
      _nextProviders = new Dictionary<ChainKey, IChainValueProvider>(new ChainKeyComparer());
      _random = random;
      _valueProvider = valueProvider;
    }

    public void SetProvider(IValueProvider valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public IChainValueProvider SetChainProvider(IValueProvider valueProvider, Type providerType, string nameFilter=null)
    {
      IChainValueProvider chainValueProvider;
      var key = GetKey(providerType, nameFilter);
      if(HasChainProvider(providerType, nameFilter))
      {
        chainValueProvider = GetChainProvider(providerType, nameFilter);
        chainValueProvider.SetProvider(valueProvider);
      }
      else
      {
        chainValueProvider = new ChainValueProvider(_random, valueProvider);
        _nextProviders.Add(key,chainValueProvider );
      }

      return chainValueProvider;
    }

    public bool HasChainProvider(Type providerType, string nameFilter=null)
    {
      return _nextProviders.ContainsKey(GetKey(providerType, nameFilter));
    }

    public IChainValueProvider GetChainProvider(Type providerType, string nameFilter)
    {
      return _nextProviders[GetKey(providerType, nameFilter)];
    }

    public Random Random
    {
      get { return _random; }
    }

    public bool HasValue()
    {
      return _valueProvider != null;
    }

    public object GetValue()
    {
      return !HasValue() ? null : _valueProvider.GetObjectValue(_random);
    }

    private static ChainKey GetKey (Type providerType, string nameFilter)
    {
      return new ChainKey(providerType, nameFilter);
    }
  }
}