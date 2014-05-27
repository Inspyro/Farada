using System;
using System.Text;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class RandomWordGenerator : RandomSyllabileGenerator
  {
    private readonly int _minWordLength;
    private readonly int _maxWordLength;

    public RandomWordGenerator (int minWordLength = 3, int maxWordLength = 10, int minSyllabileLength = 3, int maxSyllabileLength = 5)
        : base(minSyllabileLength, maxSyllabileLength)
    {
      //TODO chech
      _minWordLength = minWordLength;
      _maxWordLength = maxWordLength;
    }

    protected override string CreateValue (ValueProviderContext<string> context)
    {
      var constraints = context.PropertyInfo != null ? context.PropertyInfo.Constraints : null; //TODO: Generalize constraints to support custom attributes

      var minWordLength = _minWordLength;
      var maxWordLength = _maxWordLength;

      if (constraints != null)
      {
        if (constraints.MinLength != null)
        {
          minWordLength = constraints.MinLength.Value;
          
          if (constraints.MaxLength != null)
          {
            maxWordLength = constraints.MaxLength.Value;
          }
          else
          {
            maxWordLength = minWordLength + 100;
          }
        }
        else if (constraints.MaxLength != null)
        {
          maxWordLength = constraints.MaxLength.Value;

          if (constraints.MinLength != null)
          {
            minWordLength = constraints.MinLength.Value;
          }
          else
          {
            minWordLength = 0;
          }
        }
      }

      var targetWordLength = context.Random.Next(minWordLength, maxWordLength);

      var wordBuilder = new StringBuilder();
      while (wordBuilder.Length < targetWordLength)
      {
        wordBuilder.Append(base.CreateValue(context));
      }

      if (context.Random.Next() > 0.5&&wordBuilder.Length>0)
      {
        wordBuilder[0] = Char.ToUpper(wordBuilder[0]);
      }

      return wordBuilder.ToString().Substring(0, targetWordLength);
    }
  }

  public class RandomSyllabileGenerator:ValueProvider<string>
  {
    private readonly int _min;
    private readonly int _max;

    private static readonly char[] s_vowels = { 'a', 'e', 'i', 'o', 'u' };
    private static readonly char[] s_consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };

    public RandomSyllabileGenerator(int min=3, int max=5)
    {
      _min = min;
      _max = max;
    }

    protected override string CreateValue (ValueProviderContext<string> context)
    {
     var len = context.Random.Next(_min, _max);

      var syllabile = new StringBuilder();
      for (var i = 0; i < len; i++)
      {
        var c = i == 1 ? s_vowels[context.Random.Next(s_vowels.Length)] : s_consonants[context.Random.Next(s_consonants.Length)];
        syllabile.Append(c);
      }

      return syllabile.ToString();
    }
  }
}
