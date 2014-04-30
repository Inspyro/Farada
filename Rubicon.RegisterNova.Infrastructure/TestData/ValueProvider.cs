using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  class Program
  {
    public static void Main ()
    {
      var valueProviderBuilder = ChainValueProviderBuilderFactory.GetDefault();
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("dog friend first name"), d => d.BestDogFriend.FirstName);
      valueProviderBuilder.SetProvider(new CatGenerator());

      var typeValueProvider = new TypeValueProvider(valueProviderBuilder.ToValueProvider());

      var someString=typeValueProvider.Get<string>();
      Console.WriteLine(someString);

      var dog = typeValueProvider.Get<Dog>();
      Console.WriteLine(dog.FirstName);
      Console.WriteLine(dog.LastName);
      Console.WriteLine("DogAge:"+dog.Age);
      Console.WriteLine("BestCatFriendName:" + dog.BestCatFriend.Name);

      var cat = typeValueProvider.Get<Cat>();
      Console.WriteLine(cat.Name);

      int someInt = typeValueProvider.Get<int>();
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

  class TypeValueProvider
  {
    private readonly IChainValueProvider _valueChain;
    public TypeValueProvider(IChainValueProvider valueChain)
    {
      _valueChain = valueChain;
    }

    private Dictionary<Type, int> _typeFillCountDictionary;
    public TValue Get<TValue>(int maxDepth=1)
    {
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var value = Get(_valueChain, typeof (TValue), null, maxDepth);
      return value == null ? default(TValue) : (TValue) value;
    }

  
    private object Get(IChainValueProvider currentChain, Type currentType, string currentFilter, int maxDepth)
    {
      IChainValueProvider directValueProvider = null;
      if(currentChain.HasChainProvider(currentType, currentFilter))
      {
        directValueProvider = currentChain.GetChainProvider(currentType, currentFilter);
      }

      var hasDirectValue = directValueProvider != null && directValueProvider.HasValue();

      //if we try to get a basic type (e.g. no class) we can only get the value directly...
      if (!currentType.IsClass||currentType==typeof(string))
      {
        return hasDirectValue ? directValueProvider.GetValue() : null;
      }

      var instance = hasDirectValue ? directValueProvider.GetValue() : Activator.CreateInstance(currentType);

      if(!MayFill(currentType, maxDepth))
      {
        return null;
      }

      RaiseFillCount(currentType);

      //we go over all properties and generate values for them
      var properties = currentType.GetProperties();
      foreach (var property in properties)
      {
        //if there is no direct value provider we try to get all values from the base chain
        if (directValueProvider == null)
        {
          TryFillProperty(_valueChain, property, instance, maxDepth);
        }
        else //else we try to get all properties from the concrete chain
        {
          if (!TryFillProperty(directValueProvider, property, instance, maxDepth)) //if we cant fill the properties with the direct chain (current level) we get the values from the default chain
          {
            TryFillProperty(_valueChain, property, instance, maxDepth);
          }
        }
      }

      return instance;
    }

    private void RaiseFillCount (Type currentType)
    {
      if(_typeFillCountDictionary.ContainsKey(currentType))
      {
        _typeFillCountDictionary[currentType]++;
      }
      else
      {
        _typeFillCountDictionary.Add(currentType, 1);
      }
    }

    private bool MayFill (Type currentType, int maxDepth)
    {
      return !_typeFillCountDictionary.ContainsKey(currentType) || _typeFillCountDictionary[currentType] < maxDepth;
    }

    private bool TryFillProperty (IChainValueProvider valueProvider, PropertyInfo property, object instance, int maxDepth)
    {
      if (CanFillProperty(valueProvider, property, true))
      {
        FillProperty(valueProvider, property, instance, true, maxDepth);
        return true;
      }

      if (CanFillProperty(valueProvider, property, false))
      {
        FillProperty(valueProvider, property, instance, false, maxDepth);
        return true;
      }

      return false;
    }

    private static bool CanFillProperty (IChainValueProvider valueProvider, PropertyInfo property, bool filterName)
    {
      return valueProvider.HasChainProvider(property.PropertyType, filterName ? property.Name : null);
    }

    private void FillProperty (IChainValueProvider directValueProvider, PropertyInfo property, object instance, bool filterName, int maxDepth)
    {
      var propertyValue = Get(directValueProvider, property.PropertyType, filterName?property.Name:null, maxDepth);
      property.SetValue(instance, propertyValue);
    }
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

    public static IEnumerable<ChainKey> ToChain(this Expression expression)
    {
      var parameterExpression = expression as ParameterExpression;
      if (parameterExpression != null)
      {
        yield return new ChainKey(parameterExpression.Type);
      }

      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain())
        {
          yield return chainKey;
        }

        yield return new ChainKey(memberExpression.Type, memberExpression.Member.Name);
      }
    }

    public static IEnumerable<ChainKey> ToChain (this LambdaExpression expression)
    {
      return expression.Body.ToChain();
    }
  }

  //Provide a base value provider like this
  internal static class ChainValueProviderBuilderFactory
  {
    public static ChainValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new BasicStringGenerator(new CustomStringGenerator()));

      return defaultProvider;
    }

    public static ChainValueProviderBuilder GetEmpty ()
    {
      return new ChainValueProviderBuilder();
    }
  }


  public class ChainValueProviderBuilder
  {
    private readonly IChainValueProvider _chainValueProvider;

    public ChainValueProviderBuilder()
    {
      _chainValueProvider = new ChainValueProvider();
    }

    public void SetProvider<TProperty>(ValueProvider<TProperty> valueProvider)
    {
      _chainValueProvider.SetChainProvider(valueProvider, typeof (TProperty));
    }

    public void SetProvider<TProperty, TContainer>(ValueProvider<TProperty> valueProvider, Expression<Func<TContainer, TProperty>> chainExpression)
    {
      var expressionChain = chainExpression.ToChain().ToList();
      var currentValueProvider = _chainValueProvider;

      for (var i = 0; i < expressionChain.Count-1; i++)
      {
        var chainKey = expressionChain[i];
        currentValueProvider = currentValueProvider.SetChainProvider(null, chainKey.Type, chainKey.Name);
      }

      var finalExpression = expressionChain.Last();
      currentValueProvider.SetChainProvider(valueProvider, finalExpression.Type, finalExpression.Name);
    }

    public IChainValueProvider ToValueProvider()
    {
      return _chainValueProvider;
    }
  }


  public class ChainValueProvider:IChainValueProvider
  {
    private IValueProvider _valueProvider;
    private readonly Dictionary<ChainKey, IChainValueProvider> _nextProviders;

    public ChainValueProvider(IValueProvider valueProvider=null)
    {
      _nextProviders = new Dictionary<ChainKey, IChainValueProvider>(new ChainKeyComparer());
      _valueProvider = valueProvider;
    }

    public void SetProvider(IValueProvider valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public IChainValueProvider SetChainProvider(IValueProvider valueProvider, Type providerType, string nameFilter=null)
    {
      IChainValueProvider chainValueProvider = null;
      var key = GetKey(providerType, nameFilter);
      if(HasChainProvider(providerType, nameFilter))
      {
        chainValueProvider = GetChainProvider(providerType, nameFilter);
        chainValueProvider.SetProvider(valueProvider);
      }
      else
      {
        chainValueProvider = new ChainValueProvider(valueProvider);
        _nextProviders.Add(key,chainValueProvider );
      }

      return chainValueProvider;
    }

    public bool HasChainProvider(Type providerType, string nameFilter=null)
    {
      return _nextProviders.ContainsKey(GetKey(providerType, nameFilter));
    }

    public IChainValueProvider GetChainProvider(Type providerType, string nameFilter)
    {
      return _nextProviders[GetKey(providerType, nameFilter)];
    }

    public bool HasValue()
    {
      return _valueProvider != null;
    }

    public object GetValue()
    {
      return !HasValue() ? null : _valueProvider.GetObjectValue();
    }

    private static ChainKey GetKey (Type providerType, string nameFilter)
    {
      return new ChainKey(providerType, nameFilter);
    }
  }

  public interface IChainValueProvider
  {
    bool HasValue ();
    object GetValue();

    void SetProvider (IValueProvider valueProvider);
    IChainValueProvider SetChainProvider(IValueProvider valueProvider, Type providerType, string nameFilter=null);
    bool HasChainProvider(Type providerType, string nameFilter=null);
    IChainValueProvider GetChainProvider(Type providerType, string nameFilter);
  }

  internal class ChainKeyComparer : IEqualityComparer<ChainKey>
  {
    public bool Equals (ChainKey x, ChainKey y)
    {
      return x.Equals(y);
    }

    public int GetHashCode (ChainKey obj)
    {
      return obj.GetHashCode();
    }
  }

  internal class ChainKey
  {
    public Type Type { get; private set; }
    public string Name { get; private set; }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      return Equals((ChainKey) obj);
    }

    public ChainKey(Type type, string name=null)
    {
      Type = type;
      Name = name;
    }

    public bool Equals (ChainKey other)
    {
      return Type == other.Type && string.Equals(Name, other.Name);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
      }
    }
  }

  public interface IValueProvider
  {
    object GetObjectValue ();
  }

  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    private readonly ValueProvider<TProperty> _nextProvider;

    protected ValueProvider(ValueProvider<TProperty> nextProvider)
    {
      _nextProvider = nextProvider;
    }

    protected virtual TProperty GetValue (TProperty currentValue=default(TProperty))
    {
      return _nextProvider != null ? _nextProvider.GetValue(currentValue) : currentValue;
    }

    public object GetObjectValue ()
    {
      return GetValue();
    }
  }


  #region Sample code
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
    public Dog BestDogFriend { get; set; }
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
#endregion
}
