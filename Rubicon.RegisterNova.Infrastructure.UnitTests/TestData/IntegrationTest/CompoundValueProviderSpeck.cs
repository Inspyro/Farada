﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Validation;
using SpecK;
using SpecK.Extensibility;
using SpecK.Specifications;
using SpecK.Specifications.InferredApi;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [Subject (typeof (ICompoundValueProvider))]
  public class CompoundValueProviderSpeck : Specs
  {
    BaseDomainConfiguration Domain;
    bool UseDefaults;
    ICompoundValueProvider ValueProvider;
    int MaxRecursionDepth;

    Context DefaultsContext(bool useDefaults)
    {
      return c => c.Given ("use defaults " + useDefaults, x => UseDefaults = useDefaults);
    }

    Context RecursionContext(int recursionDepth=2)
    {
      return c => c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth);
    }

    Context ValueProviderContext()
    {
      return
          c =>
              c.Given ("create compound value provider",
                  x => ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider (Domain, UseDefaults));
    }

    Context BasePropertyContext (bool useDefaults = true, int recursionDepth = 2)
    {
      return c => c.Given (DefaultsContext (useDefaults))
          .Given (RecursionContext (recursionDepth))
          .Given (ValueProviderContext ());
    }

    Context BaseDomainContext (bool useDefaults=true)
    {
      return c => c
          .Given ("empty base domain", x => Domain = new BaseDomainConfiguration ())
          .Given (BasePropertyContext (useDefaults));
    }

    [Group]
    void ValueProviderWithDefaultDomain ()
    {
        GenericCase<byte> ("simple byte case", _ => _
          .Given(BaseDomainContext())
          //
          .It ("should be a valid byte", x => x.Result.Should ().BeInRange(byte.MinValue, byte.MaxValue)));

      GenericCase<char> ("simple char case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid char", x => x.Result.Should ().BeOfType (typeof (char))));

      GenericCase<decimal> ("simple decimal case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid decimal", x => x.Result.Should ().BeInRange (decimal.MinValue, decimal.MaxValue)));

      GenericCase<double> ("simple double case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid double", x => x.Result.Should ().BeInRange (double.MinValue, double.MaxValue)));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid Enum", x => x.Result.Should ().BeOfType<SomeEnum>()));

      GenericCase<float> ("simple float case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid float", x => x.Result.Should ().BeInRange(float.MinValue, float.MaxValue)));

       GenericCase<int> ("simple int case", _ => _
         .Given(BaseDomainContext())
          //
          .It ("should be a valid int", x => x.Result.Should ().BeInRange(int.MinValue, int.MaxValue)));

      GenericCase<long> ("simple long case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid long", x => x.Result.Should ().BeInRange (long.MinValue, long.MaxValue)));

       GenericCase<sbyte> ("simple sbyte case", _ => _
         .Given(BaseDomainContext())
          //
          .It ("should be a valid sbyte", x => x.Result.Should ().BeInRange (sbyte.MinValue, sbyte.MaxValue)));

      GenericCase<short> ("simple short case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid short", x => x.Result.Should ().BeInRange (short.MinValue, short.MaxValue)));

      GenericCase<uint> ("simple uint case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid uint", x => x.Result.Should ().BeInRange (uint.MinValue, uint.MaxValue)));

      GenericCase<ulong> ("simple ulong case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid ulong", x => x.Result.Should ().BeInRange (ulong.MinValue, ulong.MaxValue)));

      GenericCase<ushort> ("simple ushort case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should be a valid ushort", x => x.Result.Should ().BeInRange (ushort.MinValue, ushort.MaxValue)));

      GenericCase<string> ("simple string case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should equal test", x => x.Result.Should ().NotBeNull ()));

      GenericCase<DateTime> ("simple DateTime case", _ => _
        .Given(BaseDomainContext())
          //
          .It ("should equal some past DateTime", x => x.Result.Should ().BeBefore (DateTime.Today)));
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
          .It ("should be a valid Enum", x => x.Result.Should ().BeOfType<SomeEnum>()));

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
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder => builder.AddInstanceModifier (new NullModifier (1))
                 };
      })
      .Given (BasePropertyContext ());
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
              .It ("never null prop considering NeverNull attribute", x => x.Result.NeverNullProp.Should ().NotBeNull ())
              .It ("other prop considering no attribute", x => x.Result.OtherProp.Should ().BeNull ()));
    }

    [Group]
    void ValueProviderWithInvalidConstraints ()
    {
      Specify (x =>
          ValueProvider.Create<ClassWithInvalidStringLengthConstraint> (MaxRecursionDepth, null))
          .Elaborate ("should raise argument exception", _ => _
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
        Domain = new BaseDomainConfiguration ()
                 {
                     BuildValueProvider = builder => builder.AddProvider ((string s) => s, context => "SomeString")
                 };
      })
      .Given(BasePropertyContext(false, recursionDepth));
    }

    [Group]
    void ValueProviderDeepClassFilling ()
    {
      Specify (x =>
          ValueProvider.Create<Universe> (MaxRecursionDepth, null))
          .Elaborate ("should fill normal property deep in hierarchy", _ => _
              .Given (SimpleStringContext (3))
              .It ("sets mayor name in 1st level deep hierarchy", x => x.Result.Galaxy1.StarSystem1.Planet1.President.Name.Should ().Be ("SomeString"))
              .It ("sets mayor name in 2nd level deep hierarchy",
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
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder =>
                     {
                       builder.AddProvider ((int i) => i, context => 5);
                       builder.AddProvider ((double d) => d, context => 3.6);
                       //no default float provider!

                       builder.AddProvider ((Vehicle.LandVehicle lv) => lv.Weight, context => 100);
                       builder.AddProvider ((Vehicle.LandVehicle lv) => lv.MainColor, context => Vehicle.Color.Red);

                       builder.AddProvider ((Vehicle.AirVehicle av) => av.Engines, context => new Vehicle.JetEngine ());
                       builder.AddProvider ((Vehicle.JetEngine je) => je.PowerInNewtons, context => 5000);
                     }
                 };
      })
          .Given (BasePropertyContext (false));
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
              .It ("should fill tire diameter", x => x.Result.Tires.Diameter.Should ().Be (3.6))
              .It ("should fill grip", x => x.Result.Tires.Grip.Should ().Be (3.6)));

      Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (PropertyProviderContext ())
              .It ("should fill weight with default int", x => x.Result.Weight.Should ().Be (5))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill engine with jetengine", x => x.Result.Engines.Should ().BeOfType (typeof (Vehicle.JetEngine)))
              .It ("should fill fuel use per second with default float",
                  x => ((Vehicle.JetEngine) x.Result.Engines).FuelUsePerSecond.Should ().Be (0f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engines.PowerInNewtons.Should ().Be (5000f)));
    }

    Context HierarchyPropertyProviderContext ()
    {
      return c => c.Given ("hierachical property domain", x =>
      {
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder =>
                     {
                       //no default double/int provider!
                       builder.AddProvider ((float f) => f, context => 2.1f);

                       builder.AddProvider ((Vehicle v) => v.Weight, context => 50);

                       //randomly return an engine
                       int i = 0;
                       builder.AddProvider ((Vehicle.AirVehicle av) => av.Engines, context =>
                       {
                         i++;
                         return i % 2 == 0 ? (Vehicle.Engine) new Vehicle.JetEngine () : new Vehicle.PropellorEngine ();
                       });

                       builder.AddProvider ((Vehicle.Engine e) => e.PowerInNewtons, context => 1200);
                       builder.AddProvider ((Vehicle.PropellorEngine pe) => pe.PowerInNewtons, context => 250);
                     }
                 };
      })
      .Given(BasePropertyContext(false));
    }

    [Group]
    void ValueProviderTypeHierarchyPropertyProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill default color", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill tire diameter with default value", x => x.Result.Tires.Diameter.Should ().Be (0))
              .It ("should fill grip with default value", x => x.Result.Tires.Grip.Should ().Be (0)));

      Specify (x =>
          ValueProvider.CreateMany<Vehicle.AirVehicle> (2,MaxRecursionDepth, null).First())
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill engine with propellorengine", x => x.Result.Engines.Should ().BeOfType (typeof (Vehicle.PropellorEngine)))
              .It ("should fill fuel use per second with float",
                  x => 
                    ((Vehicle.PropellorEngine) x.Result.Engines).AverageRotationSpeed.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engines.PowerInNewtons.Should ().Be (250f)));

      Specify (x =>
          ValueProvider.CreateMany<Vehicle.AirVehicle> (2, MaxRecursionDepth, null).Last())
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Vehicle.Color.White))
              .It ("should fill engine with jetengine", x => x.Result.Engines.Should ().BeOfType (typeof (Vehicle.JetEngine)))
              .It ("should fill fuel use per second with float", x => ((Vehicle.JetEngine) x.Result.Engines).FuelUsePerSecond.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as with float", x => x.Result.Engines.PowerInNewtons.Should ().Be (1200)));
    }


    Context AttributeProviderContext ()
    {
      return c => c.Given ("simple attribute domain", x =>
      {
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider =
                         builder =>
                         {
                           builder.AddProvider ((double d, Vehicle.BaseValueAttribute bva) => d, context => context.Attribute.BaseValue + 0.1d);
                           builder.AddProvider ((int i, Vehicle.BaseValueAttribute bva) => i, context => context.Attribute.BaseValue + 2);
                         }
                 };
      })
      .Given(BasePropertyContext(false));
    }

    [Group]
    void ValueProviderAttributeProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given(AttributeProviderContext())
              .It ("should fill tire usage", x => x.Result.Tires.TireUsage.Should ().Be (100.1d))
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (52)));
    }

    Context TypeHierarchyChainProviderContext ()
    {
      return c => c.Given ("simple hierachical type chained domain", x =>
      {
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder =>
                     {
                       builder.AddProvider ((int i) => i, context => 10);
                       builder.AddProvider ((int i) => i, context => context.GetPreviousValue ()+20);
                       builder.AddProvider ((Vehicle v) => v.Weight, context => context.GetPreviousValue () + 100);
                       builder.AddProvider ((Vehicle v) => v.Weight, context => context.GetPreviousValue () + 200);
                       builder.AddProvider ((Vehicle.LandVehicle av) => av.Weight, context => context.GetPreviousValue () + 1000);
                       builder.AddProvider ((Vehicle.LandVehicle av) => av.Weight, context => context.GetPreviousValue () + 2000);
                       builder.AddProvider ((int i, Vehicle.BaseValueAttribute bva) => i, context => context.GetPreviousValue()+10000);
                       builder.AddProvider ((int i, Vehicle.BaseValueAttribute bva) => i, context => context.GetPreviousValue()+20000);
                     }
                 };
      })
      .Given(BasePropertyContext(false));
    }

    [Group]
    void ValueProviderTypeHierarchyChainProviders ()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill properties according to provider chain", _ => _
              .Given (TypeHierarchyChainProviderContext ())
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (10 + 20 + 100 + 200 + 1000 + 2000 + 10000 + 20000)));
    }

    Context CustomContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder => builder.AddProvider ((int i) => i, new IntProviderWithCustomContext(contextValue))
                 };
      })
      .Given(BasePropertyContext(false));
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
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder => builder.AddProvider ((int i, ClassWithAttribute.CoolIntAttribute cia) => i, new CoolIntCustomContextValueProvider(contextValue))
                 };
      })
      .Given(BasePropertyContext(false));
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
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider = builder => builder.AddProvider ((Vehicle v) => v, new VehicleSubTypeProvider())
                 };
      })
      .Given(BasePropertyContext(false,1)); //TODO: is 1 correct?
    }

    [Group]
    void ValueProviderForSubTypes()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill tire diameter", x => x.Result.Tires.Diameter.Should ().Be (10)));

       Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill jet engine fuel per second", x => ((Vehicle.JetEngine)x.Result.Engines).FuelUsePerSecond.Should ().Be (20)));
    }

    Context ValueProviderNoSubTypeContext ()
    {
      return c => c.Given ("domain provider with sub type provider", x =>
      {
        Domain = new BaseDomainConfiguration
                 {
                     BuildValueProvider =
                         builder =>
                         builder.AddProvider ((Vehicle v) => v,
                             context =>
                         context.PropertyType == typeof (Vehicle.AirVehicle)
                             ? (Vehicle) new Vehicle.AirVehicle { Engines = new Vehicle.PropellorEngine () }
                             : new Vehicle.LandVehicle { Tires = new Vehicle.Tire () })
                 };
      })
          .Given (BasePropertyContext (false, 1)); //TODO: is 1 correct?
    }

    [Group]
    void ValueProviderNotForSubTypes()
    {
      Specify (x =>
          ValueProvider.Create<Vehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill (no exception)", _ => _
              .Given (ValueProviderNoSubTypeContext ())
              .It ("should not create land vehicle", x => x.Result.Tires.Should().BeNull()));

       Specify (x =>
          ValueProvider.Create<Vehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill (no exception)", _ => _
              .Given (ValueProviderNoSubTypeContext ())
              .It ("should not create air vehicle", x =>x.Result.Engines.Should().BeNull()));
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

  class VehicleSubTypeProvider:SubTypeValueProvider<Vehicle>
  {
    protected override Vehicle CreateValue (ValueProviderContext<Vehicle> context)
    {
      if(context.PropertyType==typeof(Vehicle.AirVehicle))
      {
        return new Vehicle.AirVehicle { Engines = new Vehicle.JetEngine { FuelUsePerSecond = 20 } };
      }
      
      if(context.PropertyType==typeof(Vehicle.LandVehicle))
      {
        return new Vehicle.LandVehicle { Tires = new Vehicle.Tire { Diameter = 10 } };
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

  class Vehicle
  {
     [BaseValue(50)]
    public int Weight { get; set; }

    public Color MainColor { get; set; }

    internal class LandVehicle : Vehicle
    {
      public Tire Tires { get; set; }
    }

    internal class Tire
    {
      public double Diameter { get; set; }
      public double Grip { get; set; }

      [BaseValue(100)]
      public double TireUsage { get; set; }
    }

    internal class AirVehicle : Vehicle
    {
      public Engine Engines { get; set; }
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

    internal class BaseValueAttribute : Attribute
    {
      public int BaseValue { get; private set; }

      public BaseValueAttribute (int baseValue)
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

    [NeverNull]
    public string NeverNullProp { get; set; }

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
  }
#endregion
}