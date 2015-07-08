using System;
using System.Collections.Generic;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastTypeInfo:IFastTypeInfo
  {
    public IList<IFastArgumentInfo> CtorArguments { get; private set; } 
    public IList<IFastMemberWithValues> Members { get; private set; }

    internal FastTypeInfo(IList<IFastArgumentInfo> ctorArguments, IList<IFastMemberWithValues> members )
    {
      CtorArguments = ctorArguments;
      Members = members;
    }
  }

  /// <summary>
  /// Provides a faster way to access a types properties than <see cref="Type.GetProperties()"/>
  /// </summary>
  public interface IFastTypeInfo
  {
    /// <summary>
    /// The ctor arguments of the type
    /// </summary>
    IList<IFastArgumentInfo> CtorArguments { get; }

        /// <summary>
    /// The properties of the type
    /// </summary>
    IList<IFastMemberWithValues> Members { get; }
  }
}