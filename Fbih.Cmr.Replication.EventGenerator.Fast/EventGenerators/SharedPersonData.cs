using System;
using Fbih.Cmr.Domain.EntryData;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class SharedPersonData
  {
    private string _jmbg;
    private string _lastName;
    private string _firstName;
    private string _firstNameOfFather;
    private DateOrYearData _dateOfBirth;

    public string Jmbg
    {
      get { return _jmbg; }
      set { _jmbg = value; }
    }

    public string LastName
    {
      get { return _lastName; }
      set { _lastName = value; }
    }

    public string FirstName
    {
      get { return _firstName; }
      set { _firstName = value; }
    }

    public string FirstNameOfFather
    {
      get { return _firstNameOfFather; }
      set { _firstNameOfFather = value; }
    }

    public DateOrYearData DateOfBirth
    {
      get { return _dateOfBirth; }
      set { _dateOfBirth = value; }
    }
  }
}