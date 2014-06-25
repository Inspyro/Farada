using System;
using System.ComponentModel.DataAnnotations;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithInvalidRangeConstraint
  {
    [Range (20, 10)]
    public int InvalidRangedNumber { get; set; }
  }
}