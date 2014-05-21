using System;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  public class ModificationContext
  {
    public Type PropertyType { get; private set; }
    public IFastPropertyInfo Property { get; private set; }
    public Random Random { get; private set; }

    public ModificationContext (Type propertyType, IFastPropertyInfo property, Random random)
    {
      PropertyType = propertyType;
      Property = property;
      Random = random;
    }
  }
}