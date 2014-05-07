using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public interface IValueProvider
  {
    object GetObjectValue (Random random, object currentValue);
    bool WantsPreviousValue ();
  }
}