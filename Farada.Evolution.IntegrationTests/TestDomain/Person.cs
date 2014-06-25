using System;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  internal class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }

    public Person Father { get; set; }
    public Person Mother { get; set; }

    public Person ()
    {
    }

    public Person (string name, Gender gender, int age = 0)
    {
      Name = name;
      Gender = gender;
      Age = age;
    }
  }
}