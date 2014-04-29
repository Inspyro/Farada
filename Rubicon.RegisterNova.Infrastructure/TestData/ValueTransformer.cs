using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  class Program
  {
    public static void Main ()
    {
      var valueProvider = ValueProviderFactory.GetDefaultProvider();
      valueProvider.SetProvider<string, Dog>(new DogNameGenerator(), d => d.Name);
      valueProvider.SetProvider(new CatGenerator());

      var typeFiller = new TypeFiller(valueProvider);

      var someString=typeFiller.Get<string>();
      Console.WriteLine(someString);

      var dog = typeFiller.Get<Dog>();
      Console.WriteLine(dog.Name);

      var cat = typeFiller.Get<Cat>();
      Console.WriteLine(cat.Name);

      int someInt = typeFiller.Get<int>();
      Console.WriteLine(someInt);

      Console.ReadKey();
    }
  }

  internal class CatGenerator:ValueTransformer<Cat>
  {
    public CatGenerator (ValueTransformer<Cat> nextTransformer=null)
        : base(nextTransformer)
    {
    }

    protected override Cat GetValue (Cat currentValue)
    {
      return new Cat { Name = "Nice cat" };
    }
  }

  class TypeFiller
  {
    private readonly TypeValuesProvider _typeValuesProvider;
    public TypeFiller(TypeValuesProvider typeValuesProvider)
    {
      _typeValuesProvider = typeValuesProvider;
    }

    public TValue Get<TValue> ()
    {
      var valueType = typeof (TValue);
      if (_typeValuesProvider.Has<TValue>())
      {
        return _typeValuesProvider.Get<TValue>();
      }

      if (!valueType.IsClass)
      {
        return default(TValue);
      }

      var instance = (TValue) Activator.CreateInstance(valueType);

      //create the filter expression
      var parameter = Expression.Parameter(valueType);
      var property = Expression.Property(parameter, "Name");
      var typeExpression = Expression.Lambda<Func<TValue, string>>(property, parameter);

      var nameValue = _typeValuesProvider.Get(typeExpression);

      //reflect:
      var nameProperty = valueType.GetProperty("Name");
      nameProperty.SetValue(instance, nameValue);

      return instance;
    }
  }

  internal class Cat
  {
    public string Name { get; set; }
  }

  internal class Dog
  {
    public string Name { get; set; }
  }

  internal static class ExpressionExtensions
  {
    public static string GetName (this LambdaExpression expression)
    {
      var memberExpression = expression.Body as MemberExpression;

      if (memberExpression == null)
      {
        throw new NotSupportedException();
      }

      var returnType = memberExpression.Type.FullName;
      var bodyInfo = memberExpression.Member.Name;
      var typeInfo = memberExpression.Expression.Type;

      return string.Format("{0} {1}.{2}", returnType, typeInfo, bodyInfo);
    }
  }

  //Provide a base value provider like this
  public static class ValueProviderFactory
  {
    public static TypeValuesProvider GetDefaultProvider()
    {
      var defaultProvider = GetEmptyProvider();
      defaultProvider.SetProvider<string>(new BasicStringGenerator());

      return defaultProvider;
    }

    public static TypeValuesProvider GetEmptyProvider()
    {
      return new TypeValuesProvider();
    }
  }

  public class TypeValuesProvider
  {
    private readonly Dictionary<string, IValueTransformer> _typeToValueTransformer;

    internal TypeValuesProvider()
    {
      _typeToValueTransformer = new Dictionary<string, IValueTransformer>();
    }

    public void SetProvider<TValue> (ValueTransformer<TValue> valueTransformer)
    {
      SetProvider<TValue, object>(valueTransformer, null);
    }

    public void SetProvider<TValue, TSource> (ValueTransformer<TValue> valueTransformer, Expression<Func<TSource, TValue>> filterExpression)
    {
      var sourceType = typeof (TValue);
      var key = filterExpression == null ? sourceType.FullName : filterExpression.GetName();

      if(_typeToValueTransformer.ContainsKey(key))
      {
        _typeToValueTransformer.Remove(key);
      }

      _typeToValueTransformer.Add(key, valueTransformer);
    }

    public bool Has<TValue> ()
    {
      return Has<TValue, object>(null);
    }

    public bool Has<TValue, TSource> (Expression<Func<TSource, TValue>> valueExpression)
    {
      var key = GetKey(valueExpression);
      return _typeToValueTransformer.ContainsKey(key);
    }

    public TValue Get<TValue> ()
    {
      return Get<TValue, object>(null);
    }

    public TValue Get<TValue, TSource> (Expression<Func<TSource, TValue>> valueExpression)
    {
      var key=GetKey(valueExpression);
      return (TValue) _typeToValueTransformer[key].GetValue();
    }

    private string GetKey<TSource, TValue> (Expression<Func<TSource, TValue>> valueExpression)
    {
       var sourceType = typeof (TValue);

       string key = sourceType.FullName;
      if (valueExpression != null)
      {
        var concreteKey = valueExpression.GetName();
        if (_typeToValueTransformer.ContainsKey(concreteKey)) //use more concrete transformer if possible
        {
          key = concreteKey;
        }
      }

      return key;
    }
  }

  public abstract class ValueTransformer<TProperty>:IValueTransformer
  {
    private readonly ValueTransformer<TProperty> _nextTransformer;

    protected ValueTransformer(ValueTransformer<TProperty> nextTransformer)
    {
      _nextTransformer = nextTransformer;
    }

    protected virtual TProperty GetValue (TProperty currentValue)
    {
      return _nextTransformer != null ? _nextTransformer.GetValue(currentValue) : currentValue;
    }

    public object GetValue(object value=null)
    {
      return GetValue((TProperty) value);
    }
  }

  public interface IValueTransformer
  {
    object GetValue (object currentValue=null);
  }

  class BasicStringGenerator:ValueTransformer<string>
  {
    public BasicStringGenerator (ValueTransformer<string> nextTransformer=null)
        : base(nextTransformer)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue + "some String...");
    }
  }

  internal class CustomStringGenerator : ValueTransformer<string>
  {
    public CustomStringGenerator (ValueTransformer<string> nextTransformer=null)
        : base(nextTransformer)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue.Substring(1,2));
    }
  }

  internal class DogNameGenerator:ValueTransformer<string>
  {
    public DogNameGenerator (ValueTransformer<string> nextTransformer=null)
        : base(nextTransformer)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return "I am a dog";
    }
  }
}
