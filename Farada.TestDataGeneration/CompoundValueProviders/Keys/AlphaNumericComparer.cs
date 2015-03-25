using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
    internal class AlphaNumericComparer : IComparer<Type>
    {
        public int Compare ([NotNull] Type x, [NotNull] Type y)
        {
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}