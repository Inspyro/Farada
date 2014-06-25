using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Farada.TestDataGeneration.BaseDomain.Modifiers;
using Farada.TestDataGeneration.CompoundValueProvider;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProvider;
using FluentAssertions;
using SpecK;
using SpecK.Extensibility;
using SpecK.Specifications;
using SpecK.Specifications.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator))]
  public class CompoundValueProviderSpeck : Specs //TODO: split in multiple files
  {
    DomainConfiguration Domain;
    ITestDataGenerator ValueProvider;
    int MaxRecursionDepth;

    Context ValueProviderContext (int recursionDepth = 2)
    {
      return
          c =>
              c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth)
                  .Given ("create compound value provider",
                      x => ValueProvider = TestDataGenerator.Create (Domain));
    }

    Context BaseDomainContext (bool useDefaults = true, int? seed = null)
    {
      return c => c
          .Given ("empty base domain with seed " + (!seed.HasValue ? "any" : seed.ToString ()),
              x => Domain = new DomainConfiguration { UseDefaults = useDefaults, Random = seed.HasValue ? new Random (seed.Value) : new Random () })
          .Given (ValueProviderContext ());
    }

    [Group]
    void ValueProviderWithDefaultDomain ()
    {
        GenericCase<byte> ("simple byte case", _ => _
          .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random byte", x => x.Result.Should ().Be(244)));

      GenericCase<char> ("simple char case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random char", x => x.Result.Should ().Be(';')));

      GenericCase<decimal> ("simple decimal case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random decimal", x => x.Result.Should ().Be (-12805660511301768796771975167m)));

      GenericCase<double> ("simple double case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random double", x => x.Result.Should ().Be (-2.9056142737628133E+307)));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a valid Enum", x => x.Result.Should ().Be(SomeEnum.SomeMember3)));

      GenericCase<float> ("simple float case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random float", x => x.Result.Should ().Be(-5.499989E+37f)));

       GenericCase<int> ("simple int case", _ => _
         .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random int", x => x.Result.Should ().Be(726643699)));

      GenericCase<long> ("simple long case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random long", x => x.Result.Should ().Be(-1490775089629665279L)));

       GenericCase<sbyte> ("simple sbyte case", _ => _
         .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random sbyte", x => x.Result.Should ().Be(-42)));

      GenericCase<short> ("simple short case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random short", x => x.Result.Should ().Be (-5297)));

      GenericCase<uint> ("simple uint case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random uint", x => x.Result.Should ().Be(726643700)));

      GenericCase<ulong> ("simple ulong case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random ulong", x => x.Result.Should ().Be(3120910928797722625)));

      GenericCase<ushort> ("simple ushort case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random ushort", x => x.Result.Should ().Be(11087)));

      GenericCase<string> ("simple string case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should equal test", x => x.Result.Should ().Be ("Homfu")));

      GenericCase<DateTime> ("simple DateTime case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should equal some random past DateTime", x => x.Result.Should ().Be (new DateTime(611490197737538675))));
    }

    [Group]
    void ValueProviderWithEmptyDomain ()
    {
      GenericCase<byte> ("simple byte case", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid byte", x => x.Result.Should ().Be(default(byte))));

      GenericCase<char> ("simple char case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid char", x => x.Result.Should ().Be(default(char))));

      GenericCase<decimal> ("simple decimal case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid decimal", x => x.Result.Should ().Be(default(decimal))));

      GenericCase<double> ("simple double case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid double", x => x.Result.Should ().Be(default(double))));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid Enum", x => x.Result.Should ().Be(SomeEnum.SomeMember1)));

      GenericCase<float> ("simple float case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid float", x => x.Result.Should ().Be(default(float))));

       GenericCase<int> ("simple int case", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid int", x => x.Result.Should ().Be(default(int))));

      GenericCase<long> ("simple long case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid long", x => x.Result.Should ().Be(default(long))));

       GenericCase<sbyte> ("simple sbyte case", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid sbyte", x => x.Result.Should ().Be(default(sbyte))));

      GenericCase<short> ("simple short case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid short", x => x.Result.Should ().Be(default(short))));

      GenericCase<uint> ("simple uint case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid uint", x => x.Result.Should ().Be(default(uint))));

      GenericCase<ulong> ("simple ulong case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid ulong", x => x.Result.Should ().Be(default(ulong))));

      GenericCase<ushort> ("simple ushort case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid ushort", x => x.Result.Should ().Be(default(ushort))));

      GenericCase<string> ("simple string case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should equal test", x => x.Result.Should ().BeNull ()));

      GenericCase<DateTime> ("simple DateTime case", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should equal some DateTime", x => x.Result.GetType ().Should ().Be (typeof (DateTime))));
    }

    Context NullModifierContext ()
    {
      return c => c
      .Given ("domain with null modifier", x =>
      {
        Domain = new DomainConfiguration
                 {
                     BuildValueProvider = builder => builder.AddInstanceModifier (new NullModifier (1))
                 };
      })
      .Given (ValueProviderContext ());
    }

    [Group]
    void ValueProviderConstraints ()
    {
      Specify (x =>
          ValueProvider.Create<ClassWithConstraints> (MaxRecursionDepth, null))
          .Elaborate ("should fill", _ => _
              .Given (NullModifierContext ())
              .It ("long name considering MinLength attribute", x => 
                x.Result.LongName.Length.Should ().BeGreaterOrEqualTo (10000))
              .It ("short name considering MaxLength attribute", x => x.Result.ShortName.Length.Should ().BeLessOrEqualTo (1))
              .It ("medium name considering Min and MaxLength attribute", x => x.Result.MediumName.Length.Should ().BeInRange (10, 20))
              .It("ranged name considering StringLength attribute", x=>x.Result.RangedName.Length.Should().BeInRange(10,100))
              .It ("ranged number considering Range attribute", x => x.Result.RangedNumber.Should ().BeInRange (10, 100))
              .It ("required prop considering Required attribute", x => x.Result.RequiredProp.Should ().NotBeNull ())
              .It ("other prop considering no attribute", x => x.Result.OtherProp.Should ().BeNull ()));
    }

    [Group]
    void ValueProviderWithInvalidConstraints ()
    {
      Specify (x =>
          ValueProvider.Create<ClassWithInvalidStringLengthConstraint> (MaxRecursionDepth, null))
          .Elaborate ("should raise argument out of range exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof (ArgumentOutOfRangeException)
              )
              .It ("has correct message",
                  x =>
                      x.Exception.Message.Should ()
                          .Contain ("On the property System.String InvalidRangedName the StringLength attribute has an invalid range")));

      Specify (x =>
          ValueProvider.Create<ClassWithInvalidRangeConstraint> (MaxRecursionDepth, null))
          .Elaborate ("should raise argument exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof (ArgumentOutOfRangeException))
              .It ("has correct message",
                  x =>
                      x.Exception.Message.Should ()
                          .Contain (
                              "On the property System.Int32 InvalidRangedNumber the Range attribute has an invalid range")));
    }

    Context SimpleStringContext (int recursionDepth)
    {
      return c => c.Given ("simple string domain", x =>
      {
        Domain = new DomainConfiguration ()
                 {
                     UseDefaults = false,
                     BuildValueProvider = builder => builder.AddProvider ((string s) => s, context => "SomeString")
                 };
      })
      .Given(ValueProviderContext(recursionDepth));
    }

    [Group]
    void ValueProviderDeepClassFilling ()
    {
      Specify (x =>
          ValueProvider.Create<Universe> (MaxRecursionDepth, null))
          .Elaborate ("should fill normal property deep in hierarchy", _ => _
              .Given (SimpleStringContext (3))
              .It ("fills properties in 1st level deep hierarchy", x => x.Result.Galaxy1.StarSystem1.Planet1.President.Name.Should ().Be ("SomeString"))
              .It ("fill properties in 2nd level deep hierarchy",
                  x =>
                      x.Result.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1.QuantumUniverse.Galaxy1.StarSystem1.Planet1.President.Name.Should ()
                          .Be ("SomeString"))
              .It ("aborts hierarchy filling at 3rd level top element (QuantumUniverse)",
                  x =>
                      x.Result.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1.QuantumUniverse.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1
                          .QuantumUniverse.Galaxy1.Should ()
                          .BeNull ()));
    }

    Context PropertyProviderContext ()
    {
      return c => c.Given ("simple property domain", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider = builder =>
                     {
                       builder.AddProvider ((int i) => i, context => 5);
                       builder.AddProvider ((double d) => d, context => 3.6);
                       //no default float provider!

                       builder.AddProvider ((Vehicle.LandVehicle lv) => lv.Weight, context => 100);
                       builder.AddProvider ((Vehicle.LandVehicle lv) => lv.MainColor, context => Vehicle.Color.Red);

                       builder.AddProvider ((Vehicle.AirVehicle av) => av.Engine, context => new Vehicle.JetEngine ());
                       builder.AddProvider ((Vehicle.JetEngine je) => je.PowerInNewtons, context => 5000);
                     }
                 };
      })
          .Given (ValueProviderContext ());
    }

    [Group]
    void ValueProviderPropertyProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (PropertyProviderContext ())
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (100))
              .It ("should fill main color", x => x.Result.MainColor.Should ().Be (Vehicle.Color.Red))
              .It ("should fill tire diameter", x => x.Result.Tire.Diameter.Should ().Be (3.6))
              .It ("should fill grip", x => x.Result.Tire.Grip.Should ().Be (3.6)));

      Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (PropertyProviderContext ())
              .It ("should fill weight with default int", x => x.Result.Weight.Should ().Be (5))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (Vehicle.JetEngine)))
              .It ("should fill fuel use per second with default float",
                  x => ((Vehicle.JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (0f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (5000f)));
    }

    Context HierarchyPropertyProviderContext ()
    {
      return c => c.Given ("hierachical property domain", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,

                     BuildValueProvider = builder =>
                     {
                       //no default double/int provider!
                       builder.AddProvider ((float f) => f, context => 2.1f);

                       builder.AddProvider ((Vehicle v) => v.Weight, context => 50);

                       //alternate between engine types
                       int i = 0;
                       builder.AddProvider ((Vehicle.AirVehicle av) => av.Engine, context =>
                       {
                         i++;
                         return i % 2 == 0 ? (Vehicle.Engine) new Vehicle.JetEngine () : new Vehicle.PropellorEngine ();
                       });

                       builder.AddProvider ((Vehicle.Engine e) => e.PowerInNewtons, context => 1200);
                       builder.AddProvider ((Vehicle.PropellorEngine pe) => pe.PowerInNewtons, context => 250);
                     }
                 };
      })
      .Given(ValueProviderContext());
    }

    [Group]
    void ValueProviderTypeHierarchyPropertyProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill default color", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill tire diameter with default value", x => x.Result.Tire.Diameter.Should ().Be (0))
              .It ("should fill grip with default value", x => x.Result.Tire.Grip.Should ().Be (0)));

      Specify (x =>
          ValueProvider.CreateMany<Vehicle.AirVehicle> (2,MaxRecursionDepth, null).First())
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))

              //start testing concrete domain logic
              .It ("should fill engine with propellorengine", x => x.Result.Engine.Should ().BeOfType (typeof (Vehicle.PropellorEngine)))
              .It ("should fill fuel use per second with float",
                  x => 
                    ((Vehicle.PropellorEngine) x.Result.Engine).AverageRotationSpeed.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (250f)));

      Specify (x =>
          ValueProvider.CreateMany<Vehicle.AirVehicle> (2, MaxRecursionDepth, null).Last())
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))

              //start testing concrete domain logic
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (Vehicle.JetEngine)))
              .It ("should fill fuel use per second with float", x => ((Vehicle.JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as with float", x => x.Result.Engine.PowerInNewtons.Should ().Be (1200)));
    }


    Context AttributeProviderContext ()
    {
      return c => c.Given ("simple attribute domain", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,

                     BuildValueProvider =
                         builder =>
                         {
                           builder.AddProvider ((double d, Vehicle.InitialValueForChainAttribute bva) => d, context => context.Attribute.BaseValue + 0.1d);
                           builder.AddProvider ((int i, Vehicle.InitialValueForChainAttribute bva) => i, context => context.Attribute.BaseValue + 2); //TODO: replace expression with typeof?
                         }
                 };
      })
      .Given(ValueProviderContext());
    }

    [Group]
    void ValueProviderAttributeProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given(AttributeProviderContext())
              .It ("should fill tire usage", x => x.Result.Tire.TireUsage.Should ().Be (100.1d))
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (52)));
    }

    Context TypeHierarchyChainProviderContext ()
    {
      return c => c.Given ("simple hierachical type chained domain", x =>
      {
        Domain = new DomainConfiguration
                 {

                    UseDefaults = false,

                     BuildValueProvider = builder =>
                     {
                       //TODO: use typeof here? and use AddProviderForType/AddProviderForExression - should we support both with types?
                       //TODO: what about resharper simplification - is the type lost

                       builder.AddProvider ((string s) => s, context => "8");
                       builder.AddProvider ((string s) => s, context => "7" + context.GetPreviousValue ());
                       builder.AddProvider ((Vehicle v) => v.Name, context => "4" + context.GetPreviousValue ());
                       builder.AddProvider ((Vehicle v) => v.Name, context => "3" + context.GetPreviousValue ());
                       builder.AddProvider ((Vehicle.LandVehicle av) => av.Name, context => "2" + context.GetPreviousValue ());
                       builder.AddProvider ((Vehicle.LandVehicle av) => av.Name, context => "1" + context.GetPreviousValue ());
                       builder.AddProvider ((string s, Vehicle.InitialStringValueForChainAttribute bva) => s, context => "6" + context.Attribute.BaseValue + context.GetPreviousValue());
                       builder.AddProvider ((string s, Vehicle.InitialStringValueForChainAttribute bva) => s, context => "5" + context.Attribute.BaseValue + context.GetPreviousValue());
                     }
                 };
      })
      .Given(ValueProviderContext());
    }

    [Group]
    void ValueProviderTypeHierarchyChainProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (TypeHierarchyChainProviderContext ())
              .It ("should fill name", x => x.Result.Name.Should ().Be ("12345!6!78")));
    }

    Context CustomContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider = builder => builder.AddProvider ((int i) => i, new IntProviderWithCustomContext(contextValue))
                 };
      })
      .Given(ValueProviderContext());
    }

    [Group]
    void ValueProviderWithCustomContext ()
    {
      Specify (x =>
          ValueProvider.Create<int> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (CustomContext (20))
              .It ("should fill int", x => x.Result.Should ().Be (20)));
    }

    Context CustomAttributeContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider = builder => builder.AddProvider ((int i, ClassWithAttribute.CoolIntAttribute cia) => i, new CoolIntCustomContextValueProvider(contextValue))
                 };
      })
      .Given(ValueProviderContext());
    }

    [Group]
    void ValueProviderWithCustomAttributeContext ()
    {
      Specify (x =>
          ValueProvider.Create<ClassWithAttribute> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (CustomAttributeContext (20))
              .It ("should fill int", x => x.Result.AttributedInt.Should ().Be (31)));
    }

     Context ValueProviderSubTypeContext ()
     {
       return c => c.Given ("domain provider with sub type provider", x =>
       {
         Domain = new DomainConfiguration
                  {
                      UseDefaults = false,
                      BuildValueProvider = builder => builder.AddProvider ((Vehicle v) => v, new VehicleSubTypeProvider ())
                  };
       })
           .Given (ValueProviderContext (RecursionDepth.DoNotFillSecondLevelProperties));
     }

    [Group]
    void ValueProviderForSubTypes()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill tire diameter", x => x.Result.Tire.Diameter.Should ().Be (10)));

       Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill jet engine fuel per second", x => ((Vehicle.JetEngine)x.Result.Engine).FuelUsePerSecond.Should ().Be (20)));
    }

    //TODO: Test what happens when no concrete type is injected for an abstract type - check exception...

    Context ValueProviderNoSubTypeContext ()
    {
      return c => c.Given ("domain provider with sub type provider", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider =
                         builder =>
                         builder.AddProvider ((Vehicle v) => v,
                             context =>
                         context.PropertyType == typeof (Vehicle.AirVehicle)
                             ? (Vehicle) new Vehicle.AirVehicle { Engine = new Vehicle.PropellorEngine () }
                             : new Vehicle.LandVehicle { Tire = new Vehicle.Tire () }) //TODO: make more clear - concrete class instead of func?
                 };
      })
          .Given (ValueProviderContext ((int) RecursionDepth.DoNotFillSecondLevelProperties));
    }

    [Group]
    void ValueProviderNotForSubTypes()
    {
      //TODO: Make more clear
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill (no exception)", _ => _
              .Given (ValueProviderNoSubTypeContext ())
              .It ("should not create land vehicle", x => x.Result.Tire.Should().BeNull()));

       Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill (no exception)", _ => _
              .Given (ValueProviderNoSubTypeContext ())
              .It ("should not create air vehicle", x =>x.Result.Engine.Should().BeNull()));
    }

    void GenericCase<T> (string caseDescription, Func<IAgainstOrArrangeOrAssert<DontCare, T>, IAssert<DontCare, T>> succession)
    {
      Specify (x => ValueProvider.Create<T> (MaxRecursionDepth, null)).Elaborate (caseDescription, succession);
    }
  }

  class CoolIntCustomContextValueProvider : AttributeBasedValueProvider<int, ClassWithAttribute.CoolIntAttribute,CoolIntCustomContextValueProvider.CoolIntCustomContext>
  {
    private readonly int _additionalValue;

    public CoolIntCustomContextValueProvider (int additionalValue)
    {
      _additionalValue = additionalValue;
    }

    protected override CoolIntCustomContext CreateContext (ValueProviderObjectContext objectContext)
    {
      return new CoolIntCustomContext (objectContext, _additionalValue);
    }

    protected override int CreateValue (CoolIntCustomContext context)
    {
      return context.AdditionalValue + context.Attribute.Value;
    }

    internal class CoolIntCustomContext : AttributeValueProviderContext<int, ClassWithAttribute.CoolIntAttribute>
    {
      public int AdditionalValue { get; private set; }

      protected internal CoolIntCustomContext (ValueProviderObjectContext objectContext, int additionalValue)
          : base (objectContext)
      {
        AdditionalValue = additionalValue;
      }
    }

  }

  #region HelperCode

  // TODO: Move out of spec file into TestDomain package, maybe even into multiple files?!
  class VehicleSubTypeProvider:SubTypeValueProvider<Vehicle>
  {
    protected override Vehicle CreateValue (ValueProviderContext<Vehicle> context)
    {
      if(context.PropertyType==typeof(Vehicle.AirVehicle))
      {
        return new Vehicle.AirVehicle { Engine = new Vehicle.JetEngine { FuelUsePerSecond = 20 } };
      }
      
      if(context.PropertyType==typeof(Vehicle.LandVehicle))
      {
        return new Vehicle.LandVehicle { Tire = new Vehicle.Tire { Diameter = 10 } };
      }

      throw new InvalidOperationException ("property of type " + context.PropertyType + " is not supported by " + this.GetType ().FullName);
    }
  }

  class IntProviderWithCustomContext:ValueProvider<int, IntProviderWithCustomContext.CustomIntContext>
  {
    readonly int _contextValue;

    public IntProviderWithCustomContext(int contextValue)
    {
      _contextValue = contextValue;
    }

    internal class CustomIntContext : ValueProviderContext<int>
    {
      public int ContextValue { get; private set; }

      public CustomIntContext (ValueProviderObjectContext objectContext, int contextValue)
          : base (objectContext)
      {
        ContextValue = contextValue;
      }
    }

    protected override CustomIntContext CreateContext (ValueProviderObjectContext objectContext)
    {
      return new CustomIntContext(objectContext, _contextValue);
    }

    protected override int CreateValue (CustomIntContext context)
    {
      return context.ContextValue;
    }
  }

  abstract class Vehicle
  {
     [InitialValueForChain(50)]
    public int Weight { get; set; }

    [InitialStringValueForChain("!")]
    public string Name { get; set; }

    public Color MainColor { get; set; }

    internal class LandVehicle : Vehicle
    {
      public Tire Tire { get; set; }
    }

    internal class Tire
    {
      public double Diameter { get; set; }
      public double Grip { get; set; }

      [InitialValueForChain(100)]
      public double TireUsage { get; set; }
    }

    internal class AirVehicle : Vehicle
    {
      public Engine Engine { get; set; }
    }

    internal class JetEngine : Engine
    {
      public float FuelUsePerSecond { get; set; }
    }

    internal class PropellorEngine:Engine
    {
      public float AverageRotationSpeed { get;set;}
    }

    internal abstract class Engine
    {
      public float PowerInNewtons { get;set; }
    }

    internal enum Color
    {
      White,
      Green,
      Red,
      Black
    }

    internal class InitialValueForChainAttribute : Attribute
    {
      public int BaseValue { get; private set; }

      public InitialValueForChainAttribute (int baseValue)
      {
        BaseValue = baseValue;
      }
    }

    internal class InitialStringValueForChainAttribute : Attribute
    {
      public string BaseValue { get; private set; }

      public InitialStringValueForChainAttribute (string baseValue)
      {
        BaseValue = baseValue;
      }
    }
  }

  internal class Universe
  {
    public Galaxy Galaxy1 { get; set; }

    internal class Galaxy
    {
      public StarSystem StarSystem1{ get; set; }

      internal class StarSystem
      {
        public Planet Planet1{ get; set; }
        public Moon Moon1{ get; set; }
        public Star Sun{ get; set; }

        internal class Planet
        {
          public Human President{ get; set; }

          internal class Human
          {
            public string Name{ get; set; }
            public Atom Atom1{ get; set; }

            internal class Atom
            {
              public QuantumParticle Particle1{ get; set; }

              internal class QuantumParticle
              {
                public Universe QuantumUniverse{ get; set; }
              }
            }
          }
        }

        internal class Moon
        {

        }

        internal class Star
        {

        }
      }
    }
  }

  class ClassWithInvalidStringLengthConstraint
  {
    [StringLength (10, MinimumLength = 20)]
    public string InvalidRangedName { get; set; }
  }

  class ClassWithInvalidRangeConstraint
  {
    [Range (20, 10)]
    public int InvalidRangedNumber { get; set; }
  }

  class ClassWithConstraints
  {
    [MinLength (10000)]
    [Required]
    public string LongName { get; set; }

    [MaxLength (1)]
    [Required]
    public string ShortName { get; set; }

    [MinLength (10)]
    [MaxLength (20)]
    [Required]
    public string MediumName { get; set; }

    [StringLength(100,MinimumLength = 10)]
    [Required]
    public string RangedName { get; set; }

    [Range (10, 100)]
    public int RangedNumber { get; set; }

    [Required]
    public string RequiredProp { get; set; }

    public string OtherProp { get; set; }
  }

  class ClassWithAttribute
  {
    [CoolInt(11)]
    public int AttributedInt { get; set; }

    internal class CoolIntAttribute : Attribute
    {
      internal int Value { get; private set; }

      public CoolIntAttribute(int value)
      {
        Value = value;
      }
    }
  }

  

  enum SomeEnum
  {
    SomeMember1,
    SomeMember2, 
    SomeMember3,
    SomeMember4,
    SomeMember5,
    SomeMember6
  }
#endregion
}