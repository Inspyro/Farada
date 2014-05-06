using System;
using System.Text;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String
{
  public class WordValueProvider : ValueProvider<string, RandomWordGenerator>
  {
    public WordValueProvider (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return currentValue + RandomGenerator.Next();
    }
  }

  public class RandomWordGenerator:RandomSyllabileGenerator
  {
    private readonly int _minWordSyllabiles;
    private readonly int _maxWordSyllabiles;

    public RandomWordGenerator(int minWordSyllabiles=3, int maxWordSyllabiles=5, int minSyllabileLength=3, int maxSyllabileLength=5):base(minSyllabileLength, maxSyllabileLength)
    {
      _minWordSyllabiles = minWordSyllabiles;
      _maxWordSyllabiles = maxWordSyllabiles;
    }

    public override string Next ()
    {
      var syllabiles = Random.Next(_minWordSyllabiles, _maxWordSyllabiles);
      var word = new StringBuilder();
      for (var i = 0; i < syllabiles; i++)
      {
        word.Append(base.Next());
      }

      if (Random.Next() > 0.5)
      {
        word[0] = Char.ToUpper(word[0]);
      }

      return word.ToString();
    }
  }

  public class RandomSyllabileGenerator:RandomGenerator<string>
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

    public override string Next ()
    {
     var len = Random.Next(_min, _max);

      var syllabile = new StringBuilder();
      for (var i = 0; i < len; i++)
      {
        var c = i == 1 ? s_vowels[Random.Next(s_vowels.Length)] : s_consonants[Random.Next(s_consonants.Length)];
        syllabile.Append(c);
      }

      return syllabile.ToString();
    }
  }
}
