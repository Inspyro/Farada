using System;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.BookOfDeaths;
using Fbih.Cmr.Domain.Events.BookOfDeaths;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Ploeh.AutoFixture;
using Rubicon.RegisterNova.Infrastructure.EventStore;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class DeathEventGenerator : EventGeneratorBase<DeathEntryCreatedEvent, DeathEntryChangedEvent>
  {
    public DeathEventGenerator (
        Func<IFixture> fixtureFactory,
        IEventAppender eventAppender,
        ICatalogRepository catalogRepository,
        RandomGenerator randomGenerator,
        EventGeneratorParameters parameters)
        : base(fixtureFactory, eventAppender, catalogRepository, randomGenerator, BookType.BookOfDeaths, parameters)
    {
    }

    protected override void MutateEvent (DeathEntryCreatedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.DeathEntryData, sharedPersonData);
    }

    protected override void MutateEvent (DeathEntryChangedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.DeathEntryData, sharedPersonData);
    }

    private void MutateEntry (DeathEntryData entryData, SharedPersonData sharedPersonData)
    {
      MutateOrdinalAndDateOfEntry(entryData);
      MutateJmbg(entryData.PersonData);
      MutateJmbg(entryData.Father, entryData.Mother);
      MutateNames(entryData.PersonData.Name, entryData.Partner, entryData.Father.Name, entryData.Mother.Name);
      MutateBirths(entryData.PersonData);

      ApplySharedPersonData(entryData, ApplyTypo(sharedPersonData));
    }

    private void ApplySharedPersonData (DeathEntryData entryData, SharedPersonData sharedPersonData)
    {
      entryData.PersonData.Jmbg = sharedPersonData.Jmbg;
      entryData.PersonData.Name.LastName = sharedPersonData.LastName;
      entryData.PersonData.Name.FirstName = sharedPersonData.FirstName;
      entryData.PersonData.DateOfBirth = sharedPersonData.DateOfBirth;
      entryData.Father.Name.FirstName = sharedPersonData.FirstNameOfFather;
    }
  }
}