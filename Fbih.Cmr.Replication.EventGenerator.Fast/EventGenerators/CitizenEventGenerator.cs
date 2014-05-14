using System;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.BookOfCitizens;
using Fbih.Cmr.Domain.Events.BookOfCitizens;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Ploeh.AutoFixture;
using Rubicon.RegisterNova.Infrastructure.EventStore;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class CitizenEventGenerator : EventGeneratorBase<CitizenEntryCreatedEvent, CitizenEntryChangedEvent>
  {
    public CitizenEventGenerator (
        Func<IFixture> fixtureFactory,
        IEventAppender eventAppender,
        ICatalogRepository catalogRepository,
        RandomGenerator randomGenerator,
        EventGeneratorParameters parameters)
        : base(fixtureFactory, eventAppender, catalogRepository, randomGenerator, BookType.BookOfCitizens, parameters)
    {
    }

    protected override void MutateEvent (CitizenEntryCreatedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.CitizenEntryData, sharedPersonData);
    }

    protected override void MutateEvent (CitizenEntryChangedEvent evt, SharedPersonData sharedPersonData)
    {
      MutateEntry(evt.CitizenEntryData, sharedPersonData);
    }

    private void MutateEntry (CitizenEntryData entryData, SharedPersonData sharedPersonData)
    {
      MutateOrdinalAndDateOfEntry(entryData);
      MutateJmbg(entryData.PersonData);
      MutateJmbg(entryData.Father, entryData.Mother);
      MutateNames(entryData.PersonData.Name, entryData.MarriagePartner, entryData.Father.Name, entryData.Mother.Name);
      MutateBirths(entryData.PersonData);

      ApplySharedPersonData(entryData, ApplyTypo(sharedPersonData));
    }

    private void ApplySharedPersonData (CitizenEntryData entryData, SharedPersonData sharedPersonData)
    {
      entryData.PersonData.Jmbg = sharedPersonData.Jmbg;
      entryData.PersonData.Name.LastName = sharedPersonData.LastName;
      entryData.PersonData.Name.FirstName = sharedPersonData.FirstName;
      entryData.PersonData.DateOfBirth = sharedPersonData.DateOfBirth;
      entryData.Father.Name.FirstName = sharedPersonData.FirstNameOfFather;
    }
  }
}