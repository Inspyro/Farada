using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  class Program
  {
    public static void Main ()
    {
      var valueProvider = ValueProviderFactory.GetDefaultProvider();
      valueProvider.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
      valueProvider.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
      valueProvider.SetProvider(new CatGenerator());

      var typeFiller = new TypeFiller(valueProvider);

      var someString=typeFiller.Get<string>();
      Console.WriteLine(someString);

      var dog = typeFiller.Get<Dog>();
      Console.WriteLine(dog.FirstName);
      Console.WriteLine(dog.LastName);
      Console.WriteLine("DogAge:"+dog.Age);
      Console.WriteLine("BestCatFriendName:" + dog.BestCatFriend.Name);

      var cat = typeFiller.Get<Cat>();
      Console.WriteLine(cat.Name);

      int someInt = typeFiller.Get<int>();
      Console.WriteLine(someInt);

      Console.ReadKey();
    }
  }

  internal class CatGenerator:ValueProvider<Cat>
  {
    public CatGenerator (ValueProvider<Cat> nextProvider=null)
        : base(nextProvider)
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

    public TValue Get<TValue>()
    {
      return Get<TValue, object>(null);
    }

    private TValue Get<TValue, TSource> (Expression<Func<TSource, TValue>> valueExpression)
    {
      if (_typeValuesProvider.Has(valueExpression))
      {
        return _typeValuesProvider.Get(valueExpression);
      }

      var valueType = typeof (TValue);
      if (!valueType.IsClass)
      {
        return default(TValue);
      }

      var instance = (TValue) Activator.CreateInstance(valueType);

      //create the filter expression
      var parameter = Expression.Parameter(valueType);

      //we go over all properties and generate values for them
      var properties = valueType.GetProperties();
      foreach (var property in properties)
      {
        var propertyExpression = Expression.Property(parameter, property.Name);

        var gen=typeof (Func<,>).MakeGenericType(typeof (TValue), property.PropertyType);

        var typeExpression = Expression.Lambda(gen, propertyExpression, parameter);

        var getValueGeneric = GetType().GetMethods(BindingFlags.Instance|BindingFlags.NonPublic).First(m => m.GetGenericArguments().Length == 2);
        var concreteGeneric = getValueGeneric.MakeGenericMethod(property.PropertyType, typeof (TValue));

        var nameValue = concreteGeneric.Invoke(this, new object[]{typeExpression});
        property.SetValue(instance, nameValue);
      }

      return instance;
    }
  }

  internal class Cat
  {
    public string Name { get; set; }
  }

  internal class Dog
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Cat BestCatFriend { get; set; }
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
      defaultProvider.SetProvider(new BasicStringGenerator());

      return defaultProvider;
    }

    public static TypeValuesProvider GetEmptyProvider()
    {
      return new TypeValuesProvider();
    }
  }

  public class TypeValuesProvider
  {
    private readonly Dictionary<string, IValueProvider> _typeToValueTransformer;

    internal TypeValuesProvider()
    {
      _typeToValueTransformer = new Dictionary<string, IValueProvider>();
    }

    public void SetProvider<TValue> (ValueProvider<TValue> valueProvider)
    {
      SetProvider<TValue, object>(valueProvider, null);
    }

    public void SetProvider<TValue, TSource> (ValueProvider<TValue> valueProvider, Expression<Func<TSource, TValue>> filterExpression)
    {
      var sourceType = typeof (TValue);
      var key = filterExpression == null ? sourceType.FullName : filterExpression.GetName();

      if(_typeToValueTransformer.ContainsKey(key))
      {
        _typeToValueTransformer.Remove(key);
      }

      _typeToValueTransformer.Add(key, valueProvider);
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

  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    private readonly ValueProvider<TProperty> _nextProvider;

    protected ValueProvider(ValueProvider<TProperty> nextProvider)
    {
      _nextProvider = nextProvider;
    }

    protected virtual TProperty GetValue (TProperty currentValue)
    {
      return _nextProvider != null ? _nextProvider.GetValue(currentValue) : currentValue;
    }

    public object GetValue(object value=null)
    {
      return GetValue((TProperty) value);
    }
  }

  public interface IValueProvider
  {
    object GetValue (object currentValue=null);
  }

  class BasicStringGenerator:ValueProvider<string>
  {
    public BasicStringGenerator (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue + "some String...");
    }
  }

  internal class CustomStringGenerator : ValueProvider<string>
  {
    public CustomStringGenerator (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue.Substring(1,2));
    }
  }

  internal class DogNameGenerator:ValueProvider<string>
  {
    private readonly string _additionalContent;

    public DogNameGenerator (string additionalContent, ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
      _additionalContent = additionalContent;
    }

    protected override string GetValue (string currentValue)
    {
      return "I am a dog - " + _additionalContent;
    }
  }
}
