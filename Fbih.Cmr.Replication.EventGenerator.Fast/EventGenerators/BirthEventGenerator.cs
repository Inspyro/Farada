using System;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.BookOfBirths;
using Fbih.Cmr.Domain.Events.BookOfBirths;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Ploeh.AutoFixture;
using Rubicon.RegisterNova.Infrastructure.EventStore;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class BirthEventGenerator : EventGeneratorBase<BirthEntryCreatedEvent, BirthEntryChangedEvent>
  {
    public BirthEventGenerator (
        Func<IFixture> fixtureFactory,
        IEventAppender eventAppender,
        ICatalogRepository catalogRepository,
        RandomGenerator randomGenerator,
        EventGeneratorParameters parameters)
        : base(fixtureFactory, eventAppender, catalogRepository, randomGenerator, BookType.BookOfBirths, parameters)
    {
    }

    protected override void MutateEvent (BirthEntryCreatedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.BirthEntryData, sharedPersonData);
    }

    protected override void MutateEvent (BirthEntryChangedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.BirthEntryData, sharedPersonData);
    }

    private void MutateEntry (BirthEntryData entryData, SharedPersonData sharedPersonData)
    {
      MutateOrdinalAndDateOfEntry(entryData);
      MutateJmbg(entryData.PersonData);
      MutateJmbg(entryData.Father.PersonData, entryData.Mother.PersonData);
      MutateNames(entryData.PersonData.Name, entryData.Father.PersonData.Name, entryData.Mother.PersonData.Name);
      MutateBirths(entryData.PersonData, entryData.Father.PersonData, entryData.Mother.PersonData);

      ApplySharedPersonData(entryData, ApplyTypo(sharedPersonData));
    }

    private void ApplySharedPersonData (BirthEntryData entryData, SharedPersonData sharedPersonData)
    {
      entryData.PersonData.Jmbg = sharedPersonData.Jmbg;
      entryData.PersonData.Name.LastName = sharedPersonData.LastName;
      entryData.PersonData.Name.FirstName = sharedPersonData.FirstName;
      entryData.PersonData.DateOfBirth = sharedPersonData.DateOfBirth;
      entryData.Father.PersonData.Name.FirstName = sharedPersonData.FirstNameOfFather;
    }
  }
}