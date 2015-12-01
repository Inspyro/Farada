namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderAndChainConfigurator<TContainer, TMember> : IChainConfigurator, IValueProviderConfigurator<TContainer, TMember>
  {
  }

  public interface IValueProviderAndChainConfigurator<TMember> : IChainConfigurator, IValueProviderConfigurator<TMember>
  {
  }
}