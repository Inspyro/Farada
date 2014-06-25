using System;
using System.ComponentModel.DataAnnotations;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithConstraints
  {
    [MinLength (10000)]
    [Required]
    public string LongName { get; set; }

    [MaxLength (1)]
    [Required]
    public string ShortName { get; set; }

    [MinLength (10)]
    [MaxLength (20)]
    [Required]
    public string MediumName { get; set; }

    [StringLength(100,MinimumLength = 10)]
    [Required]
    public string RangedName { get; set; }

    [Range (10, 100)]
    public int RangedNumber { get; set; }

    [Required]
    public string RequiredProp { get; set; }

    public string OtherProp { get; set; }
  }
}