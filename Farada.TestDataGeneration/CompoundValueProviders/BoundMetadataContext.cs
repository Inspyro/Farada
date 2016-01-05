using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders
{
  public class BoundMetadataContext<TContainer>
  {
    private readonly MetadataObjectContext _objectContext;
     

    public IRandom Random { get; }
    public ITestDataGenerator TestDataGenerator { get; }
    public IFastReflectionUtility FastReflection { get; }

    public BoundMetadataContext(MetadataObjectContext objectContext, IFastReflectionUtility fastReflectionUtility, IRandom random)
    {
      _objectContext = objectContext;

      FastReflection = fastReflectionUtility;
      Random = random;
      TestDataGenerator = objectContext.TestDataGenerator;
    }

    public TDependendMember Get<TDependendMember>(Expression<Func<TContainer, TDependendMember>> memberExpression)
    {
      var key = ChainedKey.FromExpression(memberExpression, FastReflection);
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