using System;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.BookOfMarriages;
using Fbih.Cmr.Domain.EntryData.Common;
using Fbih.Cmr.Domain.Events.BookOfMarriages;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Ploeh.AutoFixture;
using Rubicon.RegisterNova.Infrastructure.EventStore;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class MarriageEventGenerator : EventGeneratorBase<MarriageEntryCreatedEvent, MarriageEntryChangedEvent>
  {
    public MarriageEventGenerator (
        Func<IFixture> fixtureFactory,
        IEventAppender eventAppender,
        ICatalogRepository catalogRepository,
        RandomGenerator randomGenerator,
        EventGeneratorParameters parameters)
        : base(fixtureFactory, eventAppender, catalogRepository, randomGenerator, BookType.BookOfMarriages, parameters)
    {
    }

    protected override void MutateEvent (MarriageEntryCreatedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.MarriageEntryData, sharedPersonData);
    }

    protected override void MutateEvent (MarriageEntryChangedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.MarriageEntryData, sharedPersonData);
    }

    private void MutateEntry (MarriageEntryData entryData, SharedPersonData sharedPersonData)
    {
      MutateOrdinalAndDateOfEntry(entryData);
      MutateJmbg(entryData.Groom.PersonData, entryData.Bride.PersonData);
      MutateJmbg(entryData.GroomFather, entryData.GroomMother, entryData.BrideFather, entryData.BrideMother);
      MutateNames(
          entryData.Groom.PersonData.Name,
          entryData.Bride.PersonData.Name,
          entryData.GroomFather.Name,
          entryData.GroomMother.Name,
          entryData.BrideFather.Name,
          entryData.BrideMother.Name);
      MutateBirths(entryData.Groom.PersonData, entryData.Bride.PersonData);

      if (_randomGenerator.NextBool())
      {
        ApplySharedPersonData(entryData.Groom, entryData.GroomFather, ApplyTypo(sharedPersonData));
      }
      else
      {
        ApplySharedPersonData(entryData.Bride, entryData.BrideFather, ApplyTypo(sharedPersonData));
      }
    }

    private void ApplySharedPersonData (SpouseData spouseData, PersonData fatherData, SharedPersonData sharedPersonData)
    {
      spouseData.PersonData.Jmbg = sharedPersonData.Jmbg;
      spouseData.PersonData.Name.LastName = sharedPersonData.LastName;
      spouseData.PersonData.Name.FirstName = sharedPersonData.FirstName;
      spouseData.PersonData.DateOfBirth = sharedPersonData.DateOfBirth;
      fatherData.Name.FirstName = sharedPersonData.FirstNameOfFather;
    }
  }
}