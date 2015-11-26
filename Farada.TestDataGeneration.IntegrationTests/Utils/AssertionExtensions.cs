using System;
using FluentAssertions;
using TestFx.SpecK;
using TestFx.SpecK.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests.Utils
{
  public static class AssertionExtensions
  {
    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrowsContains<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Type exceptionType,
        string message)
    {
      return assert
          .ItThrows (exceptionType)
          .It ("throws correct message", x => x.Exception.Message.Should ().Contain (message));
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrowsContainsInner<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Type exceptionType,
        string message)
    {
      return assert
          .It ("throws correct inner exception", x => x.Exception.InnerException.Should ().BeOfType (exceptionType))
          .It ("throws correct inner message", x => x.Exception.InnerException.Message.Should ().Contain (message));
    }

    public static IAssert<TSubject, TResult, TVars, TSequence> ItThrowsInner<TSubject, TResult, TVars, TSequence>(
        this IAssert<TSubject, TResult, TVars, TSequence> assert,
        Type exceptionType,
        string message)
    {
      return assert
          .It("throws correct inner exception", x => x.Exception.InnerException.Should().BeOfType(exceptionType))
          .It("throws correct inner message", x => x.Exception.InnerException.Message.Should().Be(message));
    }
  }
}