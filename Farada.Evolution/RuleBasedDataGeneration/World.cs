﻿using System;
using JetBrains.Annotations;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  internal class World : IWriteableWorld
  {
    private readonly WriteableUserData _userData;
    
    internal GeneratorResult CurrentData { get; set; }

    internal World ([CanBeNull] GeneratorResult initialData)
    {
      CurrentData = initialData;
      _userData = new WriteableUserData();
    }

    public T Read<T> (Func<dynamic, T> readFunc)
    {
      return readFunc(_userData.MakeReadOnly());
    }

    public int Count<T> ()
    {
      return CurrentData == null ? 0 : CurrentData.Count<T>();
    }

    public void Write (Action<dynamic> writeFunc)
    {
      writeFunc(_userData);
    }
  }

  public interface IWriteableWorld:IReadableWorld
  {
    void Write (Action<dynamic> writeFunc);
  }

  public interface IReadableWorld
  {
    [CanBeNull]
    T Read<T> (Func<dynamic, T> readFunc);
    int Count<T> ();
  }
}
