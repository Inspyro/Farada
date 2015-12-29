using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IContainerConfigurator<TContainer>
      : IContainerChainConfigurator<TContainer>, IContainerValueProviderConfigurator<TContainer>
  {
  }

  public interface IMemberConfigurator<TContainer, TMember>
      : IMemberChainConfigurator<TContainer>, IMemberValueProviderConfigurator<TContainer, TMember>
  {
  }

  public interface IContainerWithMetadataConfigurator<TContainer, TMetadata>
      : IContainerWithMetadataChainConfigurator<TContainer, TMetadata>
  {
  }

  public interface IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata>
      : IMemberWithMetadataChainConfigurator<TContainer, TMetadata>,
          IMemberWithMetadataValueProviderConfigurator<TContainer, TMember, TMetadata>
  {
  }
}