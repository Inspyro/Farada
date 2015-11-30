namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A value provider that has additional data of type <see cref="TAdditionalData"/> in its context
  /// which is constructed based on the corresponding <see cref="ValueProviderObjectContext"/>
  /// (containing information about target property etc.)
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  /// <typeparam name="TAdditionalData"></typeparam>
  public abstract class ExtendedValueProvider<TMember, TAdditionalData>
      : ValueProvider<TMember, ExtendedValueProviderContext<TMember, TAdditionalData>>
  {
    protected sealed override ExtendedValueProviderContext<TMember, TAdditionalData> CreateContext(
        ValueProviderObjectContext objectContext)
    {
      return new ExtendedValueProviderContext<TMember, TAdditionalData> (objectContext, CreateData (objectContext));
    }

    protected abstract TAdditionalData CreateData(ValueProviderObjectContext objectContext);
  }
}