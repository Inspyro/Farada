using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
    class ClassAddingAttributes:BaseClassWithAttribute
    {
        [SubClassString1]
        [SubClassString2]
        public override string SomeAttributedProperty { get { return base.SomeAttributedProperty; } set { base.SomeAttributedProperty = value; } }
    }

    class BaseClassWithAttribute
    {
        [BaseClassString2]
        public virtual string SomeAttributedProperty { get; set; }
    }

    class SubClassString1Attribute : Attribute
    {
        public string Content = "Subclass1";
    }

    class SubClassString2Attribute : Attribute
    {
        public string Content = "Subclass2";
    }

    class BaseClassString1Attribute : Attribute
    {
        public string Content = "Base1";
    }

    class BaseClassString2Attribute : Attribute
    {
        public string Content = "Base2";
    }
}
