using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    private readonly ValueProvider<TProperty> _nextProvider;
    protected RandomGenerator<TProperty> RandomGenerator { get; set; }

    protected ValueProvider (ValueProvider<TProperty> nextProvider)
    {
      _nextProvider = nextProvider;
    }

    public object GetObjectValue (RandomGeneratorProvider randomGeneratorProvider)
    {
      return GetConcreteValue(default(TProperty), randomGeneratorProvider);
    }

    protected virtual TProperty GetConcreteValue(TProperty currentValue, RandomGeneratorProvider randomGeneratorProvider)
    {
      RandomGenerator = randomGeneratorProvider.Get<TProperty>();
      return GetNextValue(GetValue(currentValue), randomGeneratorProvider);
    }

    protected abstract TProperty GetValue (TProperty currentValue);

    protected TProperty GetNextValue (TProperty currentValue, RandomGeneratorProvider randomGeneratorProvider)
    {
      return _nextProvider != null ? _nextProvider.GetConcreteValue(currentValue, randomGeneratorProvider) : currentValue;
    }
  }

  public abstract class ValueProvider<TProperty, TRandomGenerator> : ValueProvider<TProperty> where TRandomGenerator:RandomGenerator<TProperty>
  {
    protected ValueProvider (ValueProvider<TProperty> nextProvider)
        : base(nextProvider)
    {
    }

    protected override TProperty GetConcreteValue (TProperty currentValue, RandomGeneratorProvider randomGeneratorProvider)
    {
      RandomGenerator = randomGeneratorProvider.Get<TProperty>(typeof (TRandomGenerator));
      currentValue = GetValue(currentValue);
      return GetNextValue(currentValue, randomGeneratorProvider);
    }
  }
}
