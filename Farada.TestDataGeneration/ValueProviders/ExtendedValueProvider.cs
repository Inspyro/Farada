namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Like <see cref="ValueProvider{TMember,TContext}"/> but with the default context which is <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  /// <typeparam name="TAdditionalData"></typeparam>
  public abstract class ExtendedValueProvider<TMember, TAdditionalData>
      : ValueProvider<TMember, ExtendedValueProviderContext<TMember, TAdditionalData>>
  {
    private readonly TAdditionalData _additionalData;

    public ExtendedValueProvider (TAdditionalData additionalData)
    {
      _additionalData = additionalData;
    }

    protected override ExtendedValueProviderContext<TMember, TAdditionalData> CreateContext (
        ValueProviderObjectContext objectContext)
    {
      return new ExtendedValueProviderContext<TMember, TAdditionalData> (objectContext, _additionalData);
    }
  }
}