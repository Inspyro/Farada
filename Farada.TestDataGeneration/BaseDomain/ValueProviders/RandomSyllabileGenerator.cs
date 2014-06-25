using System;
using System.Text;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  public class RandomSyllabileGenerator
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

    internal void Fill (ValueProviderContext<string> context, StringBuilder stringBuilder)
    {
      var len = context.Random.Next(_min, _max);

      for (var i = 0; i < len; i++)
      {
        var c = i == 1 ? s_vowels[context.Random.Next(s_vowels.Length)] : s_consonants[context.Random.Next(s_consonants.Length)];
        stringBuilder.Append(c);
      }
    }
  }
}