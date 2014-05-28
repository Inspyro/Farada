using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
{
  public static class FastActivator
  {
    private static readonly Hashtable creatorCache = Hashtable.Synchronized(new Hashtable());
    private static readonly Type s_coType = typeof (CreateObject);

    public delegate object CreateObject ();

    ///<summary>
    /// Create an object that will used as a 'factory' to the specified type T  
    /// </summary>
    /// <returns></returns>
    public static CreateObject GetFactory (Type t)
    {
      var c = creatorCache[t] as CreateObject;
      if (c == null)
      {
        lock (creatorCache.SyncRoot)
        {
          c = creatorCache[t] as CreateObject;

          if (c != null)
          {
            return c;
          }

          var expression = Expression.Lambda<CreateObject>(Expression.New(t));
          c = expression.Compile();
          creatorCache.Add(t, c);
        }
      }
      return c;
    }
  }
}
