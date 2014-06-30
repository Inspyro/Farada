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
  [Subject(typeof(StringConstraints)), System.Runtime.InteropServices.GuidAttribute("499547D2-CA5C-4A48-86E6-ED489D704CEA")]
  public class StringConstraintsSpeck:Specs<DontCare>
  {
    StringConstraints StringConstraints;

    [Faked]
    IFastPropertyInfo PropertyInfo;

    Context MaxAttributeContext (bool isDefined, int length=0)
    {
      return
          c => c.Given (string.Format ("max attribute isDefined:{0} of lenght: {1}", isDefined, length), x =>
          {
            A.CallTo (() => PropertyInfo.IsDefined (typeof (MaxLengthAttribute))).Returns (isDefined);

            var maxLengthAttribute = isDefined ? new MaxLengthAttribute (length) : null;

            A.CallTo (() => PropertyInfo.GetCustomAttribute<MaxLengthAttribute> ()).Returns (maxLengthAttribute);
          });
    }

    Context MinAttributeContext (bool isDefined, int length=0)
    {
      return
          c => c.Given (string.Format ("max attribute isDefined:{0} of lenght: {1}", isDefined, length), x =>
          {
            A.CallTo (() => PropertyInfo.IsDefined (typeof (MinLengthAttribute))).Returns (isDefined);

            var minLengthAttribute = isDefined ? new MinLengthAttribute (length) : null;

            A.CallTo (() => PropertyInfo.GetCustomAttribute<MinLengthAttribute> ()).Returns (minLengthAttribute);
          });
    }

    Context StringLengthAttributeContext (bool isDefined, int minLength=0, int maxLength=0)
    {
      return
          c => c.Given (string.Format ("string length attribute isDefined:{0} between: {1} and {2}", isDefined, minLength, maxLength), x =>
          {
            A.CallTo (() => PropertyInfo.IsDefined (typeof (StringLengthAttribute))).Returns (isDefined);

            var stringLengthAttribute = isDefined ? new StringLengthAttribute (maxLength) { MinimumLength = minLength } : null;

            A.CallTo (() => PropertyInfo.GetCustomAttribute<StringLengthAttribute> ()).Returns (stringLengthAttribute);
          });
    }

    [Group]
    public void StringConstraintsBasic()
    {
      Specify (x => x.ToString())
          //
          .Elaborate ("FromProperty with null property", _ => _
              .Given (x => StringConstraints = StringConstraints.FromProperty (null))
              .It ("should return null", x => StringConstraints.Should ().BeNull ()))
          //
          .Elaborate ("FromProperty without stringlength attribute", _ => _
              .Given (StringLengthAttributeContext (false))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return null", x => StringConstraints.Should ().BeNull ()))
          //
          .Elaborate ("FromProperty without correct string length attribute", _ => _
              .Given (StringLengthAttributeContext (true, 3, 5))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return correct min value", x => StringConstraints.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => StringConstraints.MaxLength.Should ().Be (5)));

      Specify (x => x.ToString ())
          //
          .Elaborate ("FromProperty with correct min and max property", _ => _
              .Given (MinAttributeContext (true, 3))
              .Given (MaxAttributeContext (true, 5))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return correct min value", x => StringConstraints.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => StringConstraints.MaxLength.Should ().Be (5)))
          .Elaborate ("FromProperty with correct min value", _ => _
              .Given (MinAttributeContext (true, 3))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return correct min value", x => StringConstraints.MinLength.Should ().Be (3))
              .It ("should return correct assumed max value", x => StringConstraints.MaxLength.Should ().Be (103)))
          .Elaborate ("FromProperty with correct max attribute", _ => _
              .Given (MaxAttributeContext (true, 103))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return correct assumed min value", x => StringConstraints.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => StringConstraints.MaxLength.Should ().Be (103)))
          .Elaborate ("FromProperty with correct but low attribute", _ => _
              .Given (MaxAttributeContext (true, 3))
              .Given (x => StringConstraints = StringConstraints.FromProperty (PropertyInfo))
              .It ("should return correct assumed min value", x => StringConstraints.MinLength.Should ().Be (0))
              .It ("should return correct max value", x => StringConstraints.MaxLength.Should ().Be (3)));

      Specify (x => StringConstraints.FromProperty (PropertyInfo))
          .Elaborate ("FromProperty with invalid stringlength property", _ => _
              .Given (StringLengthAttributeContext (true, 5, 3))
              .ItThrows (typeof (ArgumentOutOfRangeException)))
          .Elaborate ("FromProperty with invalid min and max attribute", _ => _
              .Given (MinAttributeContext (true, 5))
              .Given (MaxAttributeContext (true, 3))
              .ItThrows (typeof (ArgumentOutOfRangeException)));
    }
  }
}
