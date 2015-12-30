using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IReflectiveChainConfigurator : IRootChainConfigurator
  {
  }

  public interface IContainerChainConfigurator<TContainer> : IRootChainConfigurator
  {
    IContainerWithMetadataChainConfigurator<TContainer, TMetadata> WithMetadata<TMetadata> (Func<BoundMetadataContext<TContainer>, TMetadata> metadataProvider);

    IMemberConfigurator<TContainer, TMember> Select<TMember> (Expression<Func<TContainer, TMember>> memberExpression);
  }

  public interface IMemberChainConfigurator<TContainer> : IRootChainConfigurator
  {
    IMemberConfigurator<TContainer, TNewMember> Select<TNewMember> (Expression<Func<TContainer, TNewMember>> memberExpression);
  }

  public interface IContainerWithMetadataChainConfigurator<TContainer, TMetadata> : IRootChainConfigurator
  {
    IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata> Select<TMember> (Expression<Func<TContainer, TMember>> memberExpression);

  }

  public interface IMemberWithMetadataChainConfigurator<TContainer, TMetadata> : IRootChainConfigurator
  {
    IMemberWithMetadataConfigurator<TContainer, TNewMember, TMetadata> Select<TNewMember> (
        Expression<Func<TContainer, TNewMember>> memberExpression);
  }


  public interface IRootChainConfigurator
  {
    IContainerConfigurator<TContainer> For<TContainer> ();
    IReflectiveConfigurator For (Type containerType);
  }
}