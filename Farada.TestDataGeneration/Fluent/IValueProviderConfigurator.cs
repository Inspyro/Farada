using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderConfigurator<out TReturn, TType>
  {
    TReturn AddProvider (ValueProvider<TType> valueProvider);

    TReturn AddProvider<TContext> (ValueProvider<TType, TContext> valueProvider)
        where TContext : ValueProviderContext<TType>;

    TReturn EnableAutoFill();
  }

  public interface IValueProviderWithMetadataConfigurator<out TReturn, TMember, TMetadata>
  {
    TReturn AddProvider (MetadataValueProvider<TMember, TMetadata> valueProvider);

    TReturn EnableAutoFill();
  }

  public interface IReflectiveValueProviderConfigurator: IValueProviderConfigurator<IReflectiveConfigurator, object>
  {
  }

  public interface IContainerValueProviderConfigurator<TContainer>
      : IValueProviderConfigurator<IContainerConfigurator<TContainer>, TContainer>
  {
  }

  public interface IMemberValueProviderConfigurator<TContainer, TMember>
      : IValueProviderConfigurator<IMemberConfigurator<TContainer, TMember>, TMember>
  {
  }

  public interface IMemberWithMetadataValueProviderConfigurator<TContainer, TMember, TMetadata>
      : IValueProviderWithMetadataConfigurator<IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata>, TMember, TMetadata>
  {
  }
}