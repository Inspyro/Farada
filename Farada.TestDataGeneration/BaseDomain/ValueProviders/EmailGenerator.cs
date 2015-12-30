using System.ComponentModel.DataAnnotations;
using System.Text;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a value that respects the email address attribute
  /// </summary>
  public class EmailGenerator : AttributeBasedValueProvider<string, EmailAddressAttribute> //TODO: test and implement
  {
    private readonly RandomSyllabileGenerator _firstNameGenerator;
    private readonly RandomSyllabileGenerator _secondNameGenerator;
    private readonly RandomSyllabileGenerator _domainNameGenerator;
    private readonly RandomSyllabileGenerator _topLevelDomainGenerator;

    public EmailGenerator ()
    {
      _firstNameGenerator = new RandomSyllabileGenerator (3, 8);
      _secondNameGenerator = new RandomSyllabileGenerator (3, 8);

      _domainNameGenerator = new RandomSyllabileGenerator (2, 6);
      _topLevelDomainGenerator = new RandomSyllabileGenerator (2, 3);
    }

    protected override string CreateAttributeBasedValue (AttributeValueProviderContext<string, EmailAddressAttribute> context)
    {
      var emailSb = new StringBuilder();
      _firstNameGenerator.Fill (context, emailSb);

      //firstname.secondname@domain.tld or firstname@domain.tld
      if (context.Random.Next() > 0.5)
      {
        emailSb.Append ('.');
        _secondNameGenerator.Fill (context, emailSb);
      }

      emailSb.Append ('@');
      _domainNameGenerator.Fill (context, emailSb);
      emailSb.Append ('.');
      _topLevelDomainGenerator.Fill (context, emailSb);

      return emailSb.ToString();
    }
  }
}