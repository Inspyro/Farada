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
  [Subject (typeof (RangeContstraints<>), "TODO")]
  public class RangeConstraintsSpeck : SpecK
  {
    RangeContstraints<int> RangeConstraints;

    [Faked] IFastPropertyInfo PropertyInfo;
    RangeAttribute RangeAttribute;

    public RangeConstraintsSpeck ()
    {
      Specify (x => x.ToString ())
          //
          .Case ("Constructor works as expected", _ => _
              .Define (x => RangeConstraints = new RangeContstraints<int> (5, 8))
              .It ("MinValue should be 5", x => RangeConstraints.MinValue.Should ().Be (5))
              .It ("MaxValue should be 8", x => RangeConstraints.MaxValue.Should ().Be (8)));

      Specify (x => x.ToString ())
          //
          .Case ("FromProperty without range attribute", _ => _
              .Define (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .Given (PropertyInfoContext (false))
              .It ("should return null", x => RangeConstraints.Should ().BeNull ()))
          //
          .Case ("FromProperty with wrongly typed range property", _ => _
              .Define (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .Given (PropertyInfoContext (true, typeof (double), "3", "5"))
              .It ("should return null", x => RangeConstraints.Should ().BeNull ()))
          //
          .Case ("FromProperty with correctly typed range property", _ => _
              .Define (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .Given (PropertyInfoContext (true, typeof (int), "3", "5"))
              .It ("should return correct min value", x => RangeConstraints.MinValue.Should ().Be (3))
              .It ("should return correct max value", x => RangeConstraints.MaxValue.Should ().Be (5)));

      Specify (x => RangeContstraints<int>.FromProperty (PropertyInfo))
          .Case ("FromProperty with correctly typed range property but wrong ranges", _ => _
              .Given (PropertyInfoContext (true, typeof (int), "5", "3"))
              .ItThrows<ArgumentOutOfRangeException> ());
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