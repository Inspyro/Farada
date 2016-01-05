using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders
{
  public class MetadataObjectContext
  {
    internal ITestDataGenerator TestDataGenerator { get; private set; }
    private readonly Dictionary<IKey, object> _valueMapping;

    public MetadataObjectContext(ITestDataGenerator testDataGenerator)
    {
      TestDataGenerator = testDataGenerator;
      _valueMapping = new Dictionary<IKey, object>();
    }

    internal void Add (IKey dependencyKey, object value)
    {
      _valueMapping.Add (dependencyKey, value);
    }

    internal bool ContainsKey (IKey key)
    {
      return _valueMapping.ContainsKey (key);
    }

    internal object this [IKey key] => _valueMapping[key];
  }
}