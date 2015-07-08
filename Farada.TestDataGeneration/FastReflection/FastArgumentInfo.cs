using System;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastArgumentInfo:FastMemberBase, IFastArgumentInfo
  {
    private readonly Type _declaringType;
    private PropertyInfo _cachedProperty;

    internal FastArgumentInfo (ParameterInfo parameterInfo)
      :base(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.GetCustomAttributes())
    {
      _declaringType = parameterInfo.Member.DeclaringType;
    }

    public IFastMemberWithValues ToMember (IParameterConversionService parameterConversion)
    {
      if (_cachedProperty == null)
      {
        var propertyName = parameterConversion.ToPropertyName (Name);
        _cachedProperty = _declaringType.GetProperty (propertyName);
      }

      return new FastPropertyInfo (_cachedProperty);
    }
  }

  public interface IFastArgumentInfo:IFastMemberInfo
  {
    IFastMemberWithValues ToMember (IParameterConversionService parameterConversion);
  }
}