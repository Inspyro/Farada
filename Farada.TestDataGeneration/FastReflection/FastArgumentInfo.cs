using System;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastArgumentInfo:FastMemberBase, IFastArgumentInfo
  {
    private readonly IMemberExtensionService _memberExtensionService;
    private readonly Type _declaringType;
    private PropertyInfo _cachedProperty;

    internal FastArgumentInfo (IMemberExtensionService memberExtensionService, ParameterInfo parameterInfo)
      :base(memberExtensionService, parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.GetCustomAttributes())
    {
      _memberExtensionService = memberExtensionService;
      _declaringType = parameterInfo.Member.DeclaringType;
    }

    public IFastMemberWithValues ToMember (IParameterConversionService parameterConversion)
    {
      if (_cachedProperty == null)
      {
        var propertyName = parameterConversion.ToPropertyName (Name);
        _cachedProperty = _declaringType.GetProperty (propertyName);


        if (_cachedProperty == null)
        {
          throw new NotSupportedException (
              "Type: " + _declaringType.FullName + "'s - ctor argument " + Name + " had no corresponding member (searched for: " + propertyName + ").");
        }
      }

      return new FastPropertyInfo (_memberExtensionService, _cachedProperty);
    }
  }

  public interface IFastArgumentInfo:IFastMemberInfo
  {
    IFastMemberWithValues ToMember (IParameterConversionService parameterConversion);
  }
}