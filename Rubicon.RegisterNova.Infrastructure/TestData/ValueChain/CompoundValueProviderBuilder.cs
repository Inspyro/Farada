using System;
using System.Linq;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class CompoundValueProviderBuilder
  {
    private readonly Random _random;
    private readonly IChainValueProvider _chainValueProvider;

    public CompoundValueProviderBuilder(Random random)
    {
      _random = random;
      _chainValueProvider = new ChainValueProvider(random);
    }

    public void SetProvider<TProperty>(ValueProvider<TProperty> valueProvider)
    {
      _chainValueProvider.SetChainProvider(valueProvider, typeof (TProperty));
    }

    public void SetProvider<TProperty, TContainer>(Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty> valueProvider)
    {
      var expressionChain = chainExpression.ToChain().ToList();
      var currentValueProvider = _chainValueProvider;

      for (var i = 0; i < expressionChain.Count-1; i++)
      {
        var chainKey = expressionChain[i];

        if (currentValueProvider.HasChainProvider(chainKey.Type, chainKey.Name))
        {
          currentValueProvider = currentValueProvider.GetChainProvider(chainKey.Type, chainKey.Name);
        }
        else
        {
          currentValueProvider = currentValueProvider.SetChainProvider(null, chainKey.Type, chainKey.Name);
        }
      }

      var finalExpression = expressionChain.Last();
      currentValueProvider.SetChainProvider(valueProvider, finalExpression.Type, finalExpression.Name);
    }

    internal CompoundValueProvider Build()
    {
      return new CompoundValueProvider(_chainValueProvider);
    }
  }
}