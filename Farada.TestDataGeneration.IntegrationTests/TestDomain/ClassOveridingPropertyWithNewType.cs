using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
    class ClassOveridingPropertyWithNewType:BaseClassWithProtectedProperty
    {
        public new int OverrideMe { get; set; }

    }

    class BaseClassWithProtectedProperty
    {
        public string OverrideMe { get; set; }
    }
}
