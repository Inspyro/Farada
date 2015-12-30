using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProvider<TMember> : ValueProvider<TMember>
  {
    private readonly Func<ValueProviderContext<TMember>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public FuncProvider (Func<ValueProviderContext<TMember>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateValue (ValueProviderContext<TMember> context)
    {
      return _valueFunc (context);
    }
  }

  public class FuncValueProviderWithMetadata<TMember, TMetadata> : MetadataValueProvider<TMember, TMetadata>
  {
    private readonly Func<MetatadaValueProviderContext<TMember, TMetadata>, TMember> _valueFunc;

    public FuncValueProviderWithMetadata(Func<MetatadaValueProviderContext<TMember, TMetadata>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateValue (MetatadaValueProviderContext<TMember, TMetadata> context)
    {
      return _valueFunc (context);
    }
  }


  /// <summary>
    /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
    /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
    /// </summary>
  public class AttributeFuncProvider<TMember, TAttribute> : AttributeBasedValueProvider<TMember, TAttribute>
      where TAttribute : Attribute
  {
    private readonly Func<AttributeValueProviderContext<TMember, TAttribute>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public AttributeFuncProvider(Func<AttributeValueProviderContext<TMember, TAttribute>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateAttributeBasedValue (AttributeValueProviderContext<TMember, TAttribute> context)
    {
      return _valueFunc (context);
    }
  }
}