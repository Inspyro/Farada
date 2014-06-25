using System;
using System.ComponentModel.DataAnnotations;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithInvalidStringLengthConstraint
  {
    [StringLength (10, MinimumLength = 20)]
    public string InvalidRangedName { get; set; }
  }
}