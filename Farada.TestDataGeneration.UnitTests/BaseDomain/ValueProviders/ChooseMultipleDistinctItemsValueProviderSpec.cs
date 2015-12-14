using System;
using System.Collections.Generic;
using FakeItEasy;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.FakeItEasy;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject (typeof (ChooseMultipleDistinctItemsValueProvider<,>), "Create")]
  public class ChooseMultipleDistinctItemsValueProviderSpec : Spec
  {
    static TestDataDomainConfiguration TestDataDomainConfiguration;
    static ITestDataGenerator TestDataGenerator;

    //TODO: Use strict mock - possible with TestFx?
    [Faked] IRandom Random;
    static List<int> InputList;
    static int MinItems;
    static int MaxItems;

    public ChooseMultipleDistinctItemsValueProviderSpec ()
    {
      Specify (x => TestDataGenerator.Create<IList<string>> ())
          .Case ("when creating", _ => _
              .Given (BaseContext ())
              .Given ("Fake Random", c => SetupFakeRandom ())
              .It ("returns {2, 1, 3, 0}", x => x.Result.Should ().BeEquivalentTo ("2", "1", "3", "0")));

      Specify (x => new ChooseMultipleDistinctItemsValueProvider<int, int> (InputList, MinItems, MaxItems, i => i))
          .Case ("when instantiating with MinItems>MaxItems", _ => _
              .Given (x => InputList = new List<int> ())
              .Given (x => MinItems = 4)
              .Given (x => MaxItems = 2)
              .ItThrows (typeof (ArgumentException),
                  $"minItemCount must be lower or equal to maxItemCount.{Environment.NewLine}Parameter name: minItemCount"))
          .Case ("when instantiating with Items.Count<MaxItems", _ => _
              .Given (x => InputList = new List<int> ())
              .Given (x => MinItems = 0)
              .Given (x => MaxItems = 1)
              .ItThrows (typeof (ArgumentException),
                  $"items list must have at least maxItemCount items.{Environment.NewLine}Parameter name: items"));
    }

    void SetupFakeRandom ()
    {
      A.CallTo (() => Random.Next (MinItems, MaxItems + 1)).Returns (MaxItems);
      A.CallTo (() => Random.Next (int.MinValue, int.MaxValue)).ReturnsNextFromSequence (
          // Ordering index used for first result item
          3,
          // Ordering index used for second result item
          1,
          // Ordering index used for third result item
          0,
          // Ordering index used for forth result item
          2);
    }

    Context BaseContext ()
    {
      return
          c =>
              c
                  .Given ("{0,1,2,3}", x => InputList = new List<int> { 0, 1, 2, 3 })
                  .Given ("min: 2", x => MinItems = 2)
                  .Given ("max: 4", x => MaxItems = 4)
                  .Given ("Empty domain with value provider",
                      x =>
                          TestDataDomainConfiguration =
                              (context =>
                                  context.UseDefaults (false)
                                      .UseRandom (Random)
                                      .For<IList<string>> ()
                                      .AddProvider (new ChooseMultipleDistinctItemsValueProvider<int, string> (InputList, MinItems, MaxItems,
                                          conversionFunc: item => item.ToString ())).DisableAutoFill ()))
                                      //TODO: Invert logic - disable autofill by default.
                  .Given ("TestDataGenerator", x => TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration));
    }
  }
}

//For<Cat>().AddProvider(c => new SpecialCat() {Name = ""}).DisableAutofill();
//For<Cat>().AddProvider(c => new SpecialCat("nnn", c.TestDataGenerator.Create<string>()) {Name = ""}).EnableAutofill();
//For<Cat>().AddProvider(c => new SpecialCat("nnn", c.TestDataGenerator.Create<string>((Dog d)=>d.Name))
//For<AbstractCat>().AddProvider(c=> new Cat()).FillAll();
//For<object>.AddProvider(ReflectionBasedIntstatinat())..-.