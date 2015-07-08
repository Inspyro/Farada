using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using FakeItEasy;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using TestFx.FakeItEasy;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.Constraints
{
  [Subject (typeof (StringConstraints), "FromProperty")]
  [Guid ("499547D2-CA5C-4A48-86E6-ED489D704CEA")]
  public class StringConstraintsSpeck : SpecK
  {
    [Faked] IFastMemberWithValues Member;

    public StringConstraintsSpeck ()
    {
      Specify (x => StringConstraints.FromMember (Member))
          //
          .Case ("FromProperty without stringlength attribute", _ => _
              .Given (StringLengthAttributeContext (false))
              .It ("should return null", x => x.Result.Should ().BeNull ()))
          //
          .Case ("FromProperty without correct string length attribute", _ => _
              .Given (StringLengthAttributeContext (true, 3, 5))
              .It ("should return correct min value", x => x.Result.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => x.Result.MaxLength.Should ().Be (5)))
          .Case ("FromProperty with correct min and max property", _ => _
              .Given (MinAttributeContext (true, 3))
              .Given (MaxAttributeContext (true, 5))
              .It ("should return correct min value", x => x.Result.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => x.Result.MaxLength.Should ().Be (5)))
          .Case ("FromProperty with correct min value", _ => _
              .Given (MinAttributeContext (true, 3))
              .It ("should return correct min value", x => x.Result.MinLength.Should ().Be (3))
              .It ("should return correct assumed max value", x => x.Result.MaxLength.Should ().Be (103)))
          .Case ("FromProperty with correct max attribute", _ => _
              .Given (MaxAttributeContext (true, 103))
              .It ("should return correct assumed min value", x => x.Result.MinLength.Should ().Be (3))
              .It ("should return correct max value", x => x.Result.MaxLength.Should ().Be (103)))
          .Case ("FromProperty with correct but low attribute", _ => _
              .Given (MaxAttributeContext (true, 3))
              .It ("should return correct assumed min value", x => x.Result.MinLength.Should ().Be (0))
              .It ("should return correct max value", x => x.Result.MaxLength.Should ().Be (3)))
          .Case ("FromProperty with invalid stringlength property", _ => _
              .Given (StringLengthAttributeContext (true, 5, 3)).ItThrows (typeof (ArgumentOutOfRangeException)))
          .Case ("FromProperty with invalid min and max attribute", _ => _
              .Given (MinAttributeContext (true, 5))
              .Given (MaxAttributeContext (true, 3)).ItThrows (typeof (ArgumentOutOfRangeException)));
    }

    Context MaxAttributeContext (bool isDefined, int length = 0)
    {
      return
          c => c.Given (string.Format ("max attribute isDefined:{0} of length: {1}", isDefined, length), x =>
          {
            A.CallTo (() => Member.IsDefined (typeof (MaxLengthAttribute))).Returns (isDefined);

            var maxLengthAttribute = isDefined ? new MaxLengthAttribute (length) : null;

            A.CallTo (() => Member.GetCustomAttribute<MaxLengthAttribute> ()).Returns (maxLengthAttribute);
          });
    }

    Context MinAttributeContext (bool isDefined, int length = 0)
    {
      return
          c => c.Given (string.Format ("max attribute isDefined:{0} of length: {1}", isDefined, length), x =>
          {
            A.CallTo (() => Member.IsDefined (typeof (MinLengthAttribute))).Returns (isDefined);

            var minLengthAttribute = isDefined ? new MinLengthAttribute (length) : null;

            A.CallTo (() => Member.GetCustomAttribute<MinLengthAttribute> ()).Returns (minLengthAttribute);
          });
    }

    Context StringLengthAttributeContext (bool isDefined, int minLength = 0, int maxLength = 0)
    {
      return
          c => c.Given (string.Format ("string length attribute isDefined:{0} between: {1} and {2}", isDefined, minLength, maxLength), x =>
          {
            A.CallTo (() => Member.IsDefined (typeof (StringLengthAttribute))).Returns (isDefined);

            var stringLengthAttribute = isDefined ? new StringLengthAttribute (maxLength) { MinimumLength = minLength } : null;

            A.CallTo (() => Member.GetCustomAttribute<StringLengthAttribute> ()).Returns (stringLengthAttribute);
          });
    }
  }
}