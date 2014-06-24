using System;
using System.Text;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  public class RandomWordGenerator:ValueProvider<string, StringConstrainedValueProviderContext>
  {
    private readonly RandomSyllabileGenerator _randomSyllabileGenerator;

    private readonly int _minWordLength;
    private readonly int _maxWordLength;

    public RandomWordGenerator (RandomSyllabileGenerator randomSyllabileGenerator, int minWordLength = 3, int maxWordLength = 10)
    {
      //TODO chech
      _minWordLength = minWordLength;
      _maxWordLength = maxWordLength;

      _randomSyllabileGenerator = randomSyllabileGenerator;
    }

    protected override StringConstrainedValueProviderContext CreateContext (ValueProviderObjectContext objectContext)
    {
      var stringConstraints = StringConstraints.FromProperty(objectContext.PropertyInfo) ?? new StringConstraints(_minWordLength, _maxWordLength);
      return new StringConstrainedValueProviderContext(objectContext, stringConstraints);
    }

    protected override string CreateValue (StringConstrainedValueProviderContext context)
    {
      var targetWordLength = context.Random.Next(context.StringConstraints.MinLength, context.StringConstraints.MaxLength);

      var wordBuilder = new StringBuilder();
      while (wordBuilder.Length < targetWordLength)
      {
        _randomSyllabileGenerator.Fill(context, wordBuilder);
      }

      if (context.Random.Next() > 0.5&&wordBuilder.Length>0)
      {
        wordBuilder[0] = Char.ToUpper(wordBuilder[0]);
      }

      return wordBuilder.ToString().Substring(0, targetWordLength);
    }
  }

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
