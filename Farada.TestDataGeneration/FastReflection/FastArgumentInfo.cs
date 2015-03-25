using System;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastArgumentInfo:FastPropertyInfo, IFastArgumentInfo
  {
    internal FastArgumentInfo (IParameterConversionService parameterConversion, ParameterInfo parameterInfo)
        : base(parameterConversion.ToPropertyName(parameterInfo.Name), parameterInfo.ParameterType, parameterInfo.GetCustomAttributes())
    {
    }
  }

  public interface IFastArgumentInfo:IFastParameterInfo
  {
     
  }
}