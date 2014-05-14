using System;
using System.Collections.Generic;
using System.Globalization;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.Common;
using Rubicon.RegisterNova.TestInfrastructure;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class RandomGenerator
  {
    private readonly Random _random;
    private readonly double _meaningfulPercentage;
    private readonly double _typoPercentage;
    public double NullPercentage { get; private set; }

    public RandomGenerator(double nullPercentage, double meaningfulPercentage, double typoPercentage)
    {
      _random = new Random();
      NullPercentage = nullPercentage;
      _meaningfulPercentage = meaningfulPercentage;
      _typoPercentage = typoPercentage;
    }

    public IEnumerable<NameWithNameBeforeMarriageData> GenerateNameBeforeMarriageData (int count)
    {
      var meaningful = NextBool(_meaningfulPercentage);

      for (var i = 0; i < count; i++)
      {
        yield return new NameWithNameBeforeMarriageData
                     {
                         LastName = GetMeaningfulString(CommonData.LastNames, meaningful),
                         FirstName = GetMeaningfulString(CommonData.FirstNames, meaningful),
                         LastNameBeforeMarriage = GetMeaningfulString(CommonData.LastNames, meaningful)
                     };
      }
    }

    public string GenerateJmbg()
    {
      return Some.NonEmptyString(13, 13);
    }

    public DateOrYearData GenerateDateOrYearData()
    {
      return new DateOrYearData { Date = GetRandomDateOrNull(), IsFullDate = Some.Boolean };
    }

    public IEnumerable<SharedPersonData> GenerateSharedPersonData (int count)
    {
      for(var i=0;i<count;i++)
      {
        yield return GenerateSharedPersonData();
      }
    }

    public SharedPersonData GenerateSharedPersonData ()
    {
      var meaningful = _random.Next(10) == 0;

      return new SharedPersonData
             {
                 Jmbg = ValueOrNull(Some.NonEmptyString(13, 13)),
                 LastName = GetMeaningfulString(CommonData.LastNames, meaningful),
                 FirstName = GetMeaningfulString(CommonData.FirstNames, meaningful),
                 FirstNameOfFather = GetMeaningfulString(CommonData.FirstNames, meaningful),
                 DateOfBirth = GenerateDateOrYearData()
             };
    }

    private string GetMeaningfulString (string[] meaningfulNames, bool meaningful)
    {
      return meaningful
          ? meaningfulNames[_random.Next(meaningfulNames.Length)]
          : ValueOrNull(Some.NonEmptyString(10));
    }

    private DateTime? GetRandomDateOrNull()
    {
      return NextBool(NullPercentage) ? (DateTime?)null : Some.Date;
    }

    private T ValueOrNull<T>(T value) where T : class
    {
      return NextBool(NullPercentage) ? null : value;
    }

    public RequiredDateOrYearData GenerateRequiredDateOrYearData ()
    {
      return new RequiredDateOrYearData { Date = Some.Date, IsFullDate = Some.Boolean };
    }

    public string GenerateOrdinal ()
    {
      return Some.NonEmptyString(20);
    }

    public bool NextBool (double truePercentage=0.5)
    {
      return _random.NextDouble() <= truePercentage;
    }

    public string ApplyTypo (string s)
    {
      return s != null
          ? NextBool(_typoPercentage) ? s.Insert(_random.Next(s.Length), Some.Char.ToString(CultureInfo.InvariantCulture)) : s
          : NextBool(_typoPercentage) ? Some.NonEmptyString(10) : null;
    }

    public DateOrYearData ApplyTypo (DateOrYearData dateOfBirth)
    {
      return new DateOrYearData
             {
                 Date =
                     dateOfBirth.Date.HasValue
                         ? NextBool(_typoPercentage)
                             ? dateOfBirth.Date.Value.AddDays(_random.Next(1, 10))
                             : dateOfBirth.Date.Value
                         : NextBool(_typoPercentage) ? Some.Date : (DateTime?) null,
                 IsFullDate = NextBool(_typoPercentage) ? !dateOfBirth.IsFullDate : dateOfBirth.IsFullDate
             };
    }

    public T PickRandom<T> (IReadOnlyList<T> list)
    {
      return list[_random.Next(list.Count)];
    }
  }
}