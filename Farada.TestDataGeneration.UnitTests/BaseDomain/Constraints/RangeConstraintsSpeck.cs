using System;
using System.ComponentModel.DataAnnotations;
using FakeItEasy;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using TestFx.FakeItEasy;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.Constraints
{
  public class RangeConstraintsSpecK
  {
    [Subject (typeof (RangeContstraints<>), "Constructor")]
    public class ConstructorSpecK : SpecK
    {
      RangeAttribute RangeAttribute;

      public ConstructorSpecK ()
      {
        Specify (_=> new RangeContstraints<int>(5,8))
            //
            .Case ("Constructor works as expected", _ => _
                .It ("MinValue should be 5", x => x.Result.MinValue.Should ().Be (5))
                .It ("MaxValue should be 8", x => x.Result.MaxValue.Should ().Be (8)));
      }
    }

    [Subject (typeof (RangeContstraints<>), "FromProperty")]
    public class FromPropertySpecK : SpecK
    {
      [Faked] IFastPropertyInfo PropertyInfo;
      RangeAttribute RangeAttribute;

      public FromPropertySpecK ()
      {
        Specify (_ => RangeContstraints<int>.FromProperty (PropertyInfo))
            //
            .Case ("FromProperty without range attribute", _ => _
                .Given (PropertyInfoContext (false))
                .It ("should return null", x => x.Result.Should ().BeNull ()))
            //
            .Case ("FromProperty with wrongly typed range property", _ => _
                .Given (PropertyInfoContext (true, typeof (double), "3", "5"))
                .It ("should return null", x => x.Result.Should ().BeNull ()))
            //
            .Case ("FromProperty with correctly typed range property", _ => _
                .Given (PropertyInfoContext (true, typeof (int), "3", "5"))
                .It ("should return correct min value", x => x.Result.MinValue.Should ().Be (3))
                .It ("should return correct max value", x => x.Result.MaxValue.Should ().Be (5)))
            //
            .Case ("FromProperty with correctly typed range property but wrong ranges", _ => _
                .Given (PropertyInfoContext (true, typeof (int), "5", "3"))
                .ItThrows (typeof (ArgumentOutOfRangeException)));
      }

      Context PropertyInfoContext (bool isDefined, Type rangeType = null, string minValue = null, string maxValue = null)
      {
        return
            c =>
                c.Given (string.Format ("range attribute isDefined:{0} of type {1} between: {2} and {3}", isDefined, rangeType, minValue, maxValue),
                    x =>
                    {
                      A.CallTo (() => PropertyInfo.IsDefined (typeof (RangeAttribute))).Returns (isDefined);

                      RangeAttribute = isDefined ? new RangeAttribute (rangeType, minValue, maxValue) : null;
                      A.CallTo (() => PropertyInfo.GetCustomAttribute<RangeAttribute> ()).Returns (RangeAttribute);
                    });
      }
    }
  }
}