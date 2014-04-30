using System;
using System.Linq;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ChainValueProviderBuilder
  {
    private readonly IChainValueProvider _chainValueProvider;

    public ChainValueProviderBuilder()
    {
      _chainValueProvider = new ChainValueProvider();
    }

    public void SetProvider<TProperty>(ValueProvider<TProperty> valueProvider)
    {
      _chainValueProvider.SetChainProvider(valueProvider, typeof (TProperty));
    }

    public void SetProvider<TProperty, TContainer>(ValueProvider<TProperty> valueProvider, Expression<Func<TContainer, TProperty>> chainExpression)
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

    public IChainValueProvider ToValueProvider()
    {
      return _chainValueProvider;
    }
  }
}