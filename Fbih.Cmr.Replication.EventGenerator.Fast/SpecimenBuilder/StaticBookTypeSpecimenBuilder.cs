using System;
using System.Reflection;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.TestInfrastructure;
using Ploeh.AutoFixture.Kernel;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.SpecimenBuilder
{
  public class StaticBookTypeSpecimenBuilder : PropertySpecimenBuilderBase
  {
    private readonly byte _bookType;

    public StaticBookTypeSpecimenBuilder (BookType bookType)
        : base("BookType")
    {
      _bookType = (byte) bookType;
    }

    protected override object Create (PropertyInfo propertyInfo, ISpecimenContext context)
    {
      return _bookType;
    }
  }
}