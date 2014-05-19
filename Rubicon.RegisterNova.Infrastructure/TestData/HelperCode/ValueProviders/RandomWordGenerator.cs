using System;
using System.Text;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class RandomWordGenerator:RandomSyllabileGenerator
  {
    private readonly int _minWordSyllabiles;
    private readonly int _maxWordSyllabiles;

    public RandomWordGenerator(int minWordSyllabiles=3, int maxWordSyllabiles=5, int minSyllabileLength=3, int maxSyllabileLength=5):base(minSyllabileLength, maxSyllabileLength)
    {
      _minWordSyllabiles = minWordSyllabiles;
      _maxWordSyllabiles = maxWordSyllabiles;
    }

    protected override string CreateValue (ValueProviderContext<string> context)
    {
      var syllabiles = context.Random.Next(_minWordSyllabiles, _maxWordSyllabiles);
      var word = new StringBuilder();
      for (var i = 0; i < syllabiles; i++)
      {
        word.Append(base.CreateValue(context));
      }

      if (context.Random.Next() > 0.5)
      {
        word[0] = Char.ToUpper(word[0]);
      }

      return word.ToString();
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
