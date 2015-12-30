using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ChainConfigurator : IRootChainConfigurator
  {
    protected Func<CompoundValueProviderBuilder> _lazyValueProviderBuilder;

    internal ChainConfigurator ([CanBeNull] Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
    {
      _lazyValueProviderBuilder = lazyValueProviderBuilder;
    }

    public IContainerConfigurator<TContainer> For<TContainer> ()
    {
      return new ContainerChainConfigurator<TContainer> (_lazyValueProviderBuilder);
    }


    internal ITestDataGenerator Build ()
    {
      return _lazyValueProviderBuilder().Build();
    }
  }

  internal class ContainerChainConfigurator<TContainer> : ChainConfigurator, IContainerConfigurator<TContainer>
  {
    public ContainerChainConfigurator ([CanBeNull] Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base (lazyValueProviderBuilder)
    {
    }

    public IContainerWithMetadataChainConfigurator<TContainer, TMetadata> WithMetadata<TMetadata> (Func<BoundMetadataContext<TContainer>, TMetadata> metadataProvider)
    {
      return new ContainerWithMetadataConfigurator<TContainer, TMetadata> (_lazyValueProviderBuilder, metadataProvider);
    }

    public IMemberConfigurator<TContainer, TMember> Select<TMember> (Expression<Func<TContainer, TMember>> memberExpression)
    {
      return new MemberConfigurator<TContainer, TMember> (_lazyValueProviderBuilder, memberExpression);
    }

    public IContainerConfigurator<TContainer> AddProvider (ValueProvider<TContainer> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider (valueProvider);
      return this;
    }

    public IContainerConfigurator<TContainer> AddProvider<TContext> (ValueProvider<TContainer, TContext> valueProvider)
        where TContext : ValueProviderContext<TContainer>
    {
      _lazyValueProviderBuilder().AddProvider (valueProvider);
      return this;
    }

    public IContainerConfigurator<TContainer> EnableAutoFill()
    {
      _lazyValueProviderBuilder().EnableAutoFill<TContainer>();
      return this;
    }
  }

  internal class MemberConfigurator<TContainer, TMember>
      : ChainConfigurator, IMemberConfigurator<TContainer, TMember>
  {
    private readonly Expression<Func<TContainer, TMember>> _memberExpression;

    public MemberConfigurator (
        Func<CompoundValueProviderBuilder> lazyValueProviderBuilder,
        Expression<Func<TContainer, TMember>> memberExpression)
        : base (lazyValueProviderBuilder)
    {
      _memberExpression = memberExpression;
    }

    public IMemberConfigurator<TContainer, TNewMember> Select<TNewMember> (Expression<Func<TContainer, TNewMember>> memberExpression)
    {
      return new MemberConfigurator<TContainer, TNewMember> (_lazyValueProviderBuilder, memberExpression);
    }

    public IMemberConfigurator<TContainer, TMember> AddProvider (ValueProvider<TMember> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider (_memberExpression, valueProvider);
      return this;
    }

    public IMemberConfigurator<TContainer, TMember> AddProvider<TContext> (ValueProvider<TMember, TContext> valueProvider)
        where TContext : ValueProviderContext<TMember>
    {
      _lazyValueProviderBuilder().AddProvider (_memberExpression, valueProvider);
      return this;
    }

    public IMemberConfigurator<TContainer, TMember> EnableAutoFill()
    {
      _lazyValueProviderBuilder().EnableAutoFill(_memberExpression);
      return this;
    }
  }

  internal class ContainerWithMetadataConfigurator<TContainer, TMetadata>
      : ChainConfigurator, IContainerWithMetadataConfigurator<TContainer, TMetadata>
  {
    private readonly Func<BoundMetadataContext<TContainer>, TMetadata> _metadataProvider;

    public ContainerWithMetadataConfigurator (
        Func<CompoundValueProviderBuilder> lazyValueProviderBuilder,
        Func<BoundMetadataContext<TContainer>, TMetadata> metadataProvider)
        : base (lazyValueProviderBuilder)
    {
      _metadataProvider = metadataProvider;
    }

    public IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata> Select<TMember> (Expression<Func<TContainer, TMember>> memberExpression)
    {
      return new MemberWithMetadataConfigurator<TContainer, TMember, TMetadata> (_lazyValueProviderBuilder, _metadataProvider, memberExpression);
    }
  }

  internal class MemberWithMetadataConfigurator<TContainer, TMember, TMetadata>
      : ChainConfigurator, IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata>
  {
    private readonly Func<BoundMetadataContext<TContainer>, TMetadata> _metadataProvider;
    private readonly Expression<Func<TContainer, TMember>> _memberExpression;

    public MemberWithMetadataConfigurator (
        Func<CompoundValueProviderBuilder> lazyValueProviderBuilder,
        Func<BoundMetadataContext<TContainer>, TMetadata> metadataProvider,
        Expression<Func<TContainer, TMember>> memberExpression)
        : base (lazyValueProviderBuilder)
    {
      _metadataProvider = metadataProvider;
      _memberExpression = memberExpression;
    }
    public IMemberWithMetadataConfigurator<TContainer, TNewMember, TMetadata> Select<TNewMember>(
        Expression<Func<TContainer, TNewMember>> newMemberExpression)
    {
      return new MemberWithMetadataConfigurator<TContainer, TNewMember, TMetadata>(_lazyValueProviderBuilder, _metadataProvider, newMemberExpression);
    }

    public IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata> AddProvider (MetadataValueProvider<TMember, TMetadata> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider (_memberExpression, _metadataProvider, valueProvider);
      return this;
    }

    public IMemberWithMetadataConfigurator<TContainer, TMember, TMetadata> EnableAutoFill()
    {
      _lazyValueProviderBuilder().EnableAutoFill(_memberExpression);
      return this;
    }
  }
}