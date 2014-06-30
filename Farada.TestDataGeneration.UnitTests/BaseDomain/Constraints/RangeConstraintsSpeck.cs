using System;
using System.ComponentModel.DataAnnotations;
using FakeItEasy;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using SpecK;
using SpecK.Extensibility;
using SpecK.Extension.FakeItEasy;
using SpecK.Specifications;
using SpecK.Specifications.Extensions;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.Constraints
{
  [Subject(typeof(RangeContstraints<>))]
  public class RangeConstraintsSpeck:Specs<DontCare>
  {
    RangeContstraints<int> RangeConstraints;

    [Faked]
    IFastPropertyInfo PropertyInfo;
    RangeAttribute RangeAttribute;

    Context PropertyInfoContext (bool isDefined, Type rangeType=null, string minValue=null, string maxValue=null)
    {
      return
          c => c.Given (string.Format ("range attribute isDefined:{0} of type {1} between: {2} and {3}", isDefined, rangeType, minValue, maxValue), x =>
          {
            A.CallTo (() => PropertyInfo.IsDefined (typeof (RangeAttribute))).Returns (isDefined);

            RangeAttribute = isDefined ? new RangeAttribute (rangeType, minValue, maxValue) : null;
            A.CallTo (() => PropertyInfo.GetCustomAttribute<RangeAttribute> ()).Returns (RangeAttribute);
          });
    }

    [Group]
    public void IntRangeConstraints()
    {
      Specify (x => x.ToString ())
          //
          .Elaborate ("Constructor works as expected", _ => _
              .Given (x => RangeConstraints = new RangeContstraints<int> (5, 8))
              .It ("MinValue should be 5", x => RangeConstraints.MinValue.Should ().Be (5))
              .It ("MaxValue should be 8", x => RangeConstraints.MaxValue.Should ().Be (8)));

      Specify (x => x.ToString ())
          //
          .Elaborate ("FromProperty with null property", _ => _
              .Given (x => RangeConstraints = RangeContstraints<int>.FromProperty (null))
              .It ("should return null", x => RangeConstraints.Should ().BeNull ()))
          //
          .Elaborate ("FromProperty without range attribute", _ => _
              .Given (PropertyInfoContext (false))
              .Given (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .It ("should return null", x => RangeConstraints.Should ().BeNull ()))
          //
          .Elaborate ("FromProperty with wrongly typed range property", _ => _
              .Given (PropertyInfoContext (true, typeof (double), "3", "5"))
              .Given (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .It ("should return null", x => RangeConstraints.Should ().BeNull ()))
          //
          .Elaborate ("FromProperty with correctly typed range property", _ => _
              .Given (PropertyInfoContext (true, typeof (int), "3", "5"))
              .Given (x => RangeConstraints = RangeContstraints<int>.FromProperty (PropertyInfo))
              .It ("should return correct min value", x => RangeConstraints.MinValue.Should ().Be (3))
              .It ("should return correct max value", x => RangeConstraints.MaxValue.Should ().Be (5)));

      Specify (x => RangeContstraints<int>.FromProperty (PropertyInfo))
          .Elaborate ("FromProperty with correctly typed range property but wrong ranges", _ => _
              .Given (PropertyInfoContext (true, typeof (int), "5", "3"))
              .ItThrows (typeof (ArgumentOutOfRangeException)));
    }
  }
}
