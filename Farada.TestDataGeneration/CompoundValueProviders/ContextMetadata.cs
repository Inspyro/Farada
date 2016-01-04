using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  using System.Collections.Generic;

  namespace Farada.TestDataGeneration.CompoundValueProviders
  {
    public class MetadataObjectContext
    {
      private readonly Dictionary<IKey, object> _valueMapping;

      public MetadataObjectContext()
      {
        _valueMapping = new Dictionary<IKey, object>();
      }

      internal void Add (IKey dependencyKey, object value)
      {
        _valueMapping.Add (dependencyKey, value);
      }

      public bool ContainsKey (IKey key)
      {
        return _valueMapping.ContainsKey (key);
      }

      public object this [IKey key] => _valueMapping[key];
    }

    public class BoundMetadataContext<TContainer>
    {
      private readonly MetadataObjectContext _objectContext;
      private readonly IFastReflectionUtility _fastReflectionUtility;

      public BoundMetadataContext(MetadataObjectContext objectContext, IFastReflectionUtility fastReflectionUtility)
      {
        _objectContext = objectContext;
        _fastReflectionUtility = fastReflectionUtility;
      }

      public TDependendMember Get<TDependendMember>(Expression<Func<TContainer, TDependendMember>> memberExpression)
      {
        var key = ChainedKey.FromExpression(memberExpression, _fastReflectionUtility);
        if (!_objectContext.ContainsKey (key))
        {
          throw new ArgumentException (
              "Could not find key:'" + key +
              "' in metadata context. Have you registered the dependency before the metadata provider?");
        }

        return (TDependendMember) _objectContext[key];
      }
    }
  }
}