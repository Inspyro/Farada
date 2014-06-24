using System;
using Farada.Core.CompoundValueProvider.Keys;
using Farada.Core.ValueProvider;

namespace Farada.Core.CompoundValueProvider
{
  internal class ValueProviderLink
  {
    internal IValueProvider Value { get; private set; }
    internal IKey Key { get; private set; }
    internal Func<ValueProviderLink> Previous { get; private set; }

    internal ValueProviderLink (IValueProvider value, IKey key, Func<ValueProviderLink> previous)
    {
      Value = value;
      Key = key;
      Previous = previous;
    }
  }
}