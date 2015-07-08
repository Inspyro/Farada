using System;
using System.Text;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates random words (strings) based on <see cref="RandomSyllabileGenerator"/>
  /// Reads the <see cref="StringConstraints"/> from each member and uses them with higher priority than the given constraints
  /// </summary>
  public class RandomWordGenerator:ValueProvider<string, StringConstrainedValueProviderContext>
  {
    private readonly RandomSyllabileGenerator _randomSyllabileGenerator;

    private readonly int _minWordLength;
    private readonly int _maxWordLength;

    /// <summary>
    /// Constructs a random word generator
    /// </summary>
    /// <param name="randomSyllabileGenerator"></param>
    /// <param name="minWordLength"></param>
    /// <param name="maxWordLength"></param>
    public RandomWordGenerator (RandomSyllabileGenerator randomSyllabileGenerator, int minWordLength = 3, int maxWordLength = 10)
    {
      if (maxWordLength < minWordLength)
        throw new ArgumentOutOfRangeException("maxWordLength", "The param max word lenght cannot be smaller than min word length");

      _minWordLength = minWordLength;
      _maxWordLength = maxWordLength;

      _randomSyllabileGenerator = randomSyllabileGenerator;
    }

    protected override StringConstrainedValueProviderContext CreateContext (ValueProviderObjectContext objectContext)
    {
      var stringConstraints = StringConstraints.FromMember(objectContext.MemberInfo) ?? new StringConstraints(_minWordLength, _maxWordLength);
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
}
