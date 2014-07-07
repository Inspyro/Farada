using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  //public static class Test
  //{
  //  public class MyValueProvider : ValueProvider<string>
  //  {
  //    protected override string CreateValue (ValueProviderContext<string> context)
  //    {
  //      throw new NotImplementedException();
  //    }
  //  }

  //  static void Main (string[] args)
  //  {
  //    var configurator = default(ITestDataConfigurator);

  //    configurator
  //        .For<string>()
  //        .AddProvider (x => x.Random.Next().ToString())
  //        .AddProvider (x => x.GetPreviousValue())
  //        //
  //        .For<int>().AddProvider (x => 1);

  //    IB a;
  //    a.For<string, MyValueProvider>();
  //  }

  //  public static IA For<TType, TProvider> (this IA configurator)
  //  {
  //    return configurator.For<TType>().AddProvider<TProvider>();
  //  }
  //}


  //public interface IA
  //{
  //  IA For<T> ();
  //  IA AddProvider<T> ();
  //}

  //class A : IA, IB
  //{
  //  private string _controller;

  //  public IB For<T> ()
  //  {
  //    _controller.InitializeConfigurationFor<T>();
  //    return this;
  //  }

  //  public void UseDefaults ()
  //  {
  //    _controller.AddProvider (null);
  //  }
  //}

  //// IA<int>.Bla() => return IA<string>   ( new ControllerContainer<string>(_oldContainer.Controller);

  //public interface IA<T>
  //{
  //}

  //public interface IB : IA
  //{
  //  IB UseDefaults ();
  //}

  public interface ITestDataConfigurator:IChainConfigurator, IDomainConfigurator
  {

  }
}