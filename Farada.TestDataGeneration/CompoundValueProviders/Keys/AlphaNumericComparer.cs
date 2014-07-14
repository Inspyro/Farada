using System;
using System.Collections.Generic;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
    internal class AlphaNumericComparer : IComparer<Type>
    {
        public int Compare (Type x, Type y)
        {
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}