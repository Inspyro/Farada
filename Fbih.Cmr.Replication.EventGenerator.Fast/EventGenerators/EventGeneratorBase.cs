using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fbih.Cmr.Domain.EntryData;
using Fbih.Cmr.Domain.EntryData.Common;
using Fbih.Cmr.Domain.EntryData.Printing;
using Fbih.Cmr.Domain.Events;
using Fbih.Cmr.Domain.Events.Notes;
using Fbih.Cmr.Domain.Events.Printing;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Fbih.Cmr.Domain.TestInfrastructure;
using Fbih.Cmr.Domain.Validation;
using Fbih.Cmr.Replication.EventGenerator.Tool.Fast.SpecimenBuilder;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rubicon.RegisterNova.Infrastructure.Events;
using Rubicon.RegisterNova.Infrastructure.EventStore;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Validation;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators

{
  public abstract class EventGeneratorBase<TEntryCreatedEvent, TEntryChangedEvent>
      where TEntryCreatedEvent : BookSpecificEntryEventBase
      where TEntryChangedEvent : BookSpecificEntryEventBase
  {
    private readonly Func<IFixture> _fixtureFactory;
    private readonly IEventAppender _eventAppender;
    protected readonly RandomGenerator _randomGenerator;
    private readonly EventGeneratorParameters _parameters;

    private readonly IReadOnlyList<TEntryCreatedEvent> _entryCreatedEvents;
    private readonly IReadOnlyList<TEntryChangedEvent> _entryChangedEvents;

    private readonly IReadOnlyList<NoteCreatedEvent> _noteCreatedEvents;
    private readonly IReadOnlyList<NoteChangedEvent> _noteChangedEvents;
    private readonly IReadOnlyList<NoteDeletedEvent> _noteDeletedEvents;

    private readonly IReadOnlyList<PrintingCreatedEvent> _printingCreatedEvents;
    private readonly IReadOnlyList<PrintingDeletedEvent> _printingDeletedEvents;

    private readonly IReadOnlyList<EntryDeletedEvent> _entryDeletedEvents;

    // IDEA: Move all (int) parameters into a parameter object for easier refactoring.
    protected EventGeneratorBase (
        Func<IFixture> fixtureFactory,
        IEventAppender eventAppender,
        ICatalogRepository catalogRepository,
        RandomGenerator randomGenerator,
        BookType bookType,
        EventGeneratorParameters parameters)
    {
      _fixtureFactory = fixtureFactory;
      _eventAppender = eventAppender;
      _randomGenerator = randomGenerator;
      _parameters = parameters;

      var generator = TestDataGeneratorFactory.CreateCompoundValueProvider(
          new BaseDomainConfiguration
          {
              BuildValueProvider = builder =>
              {
                builder.AddProvider(
                    (BookSpecificEntryEventBase bc) => bc.MunicipalityId,
                    context => parameters.MunicipalityId);

                builder.AddProvider(
                    (BookSpecificEntryEventBase bc) => bc.BookType,
                    context => bookType);

                builder.AddProvider(
                    (PrintingData bc) => bc.PrintingMunicipalityId,
                    context => parameters.PrintingMunicipalityId);

                builder.AddProvider((DateTime dt)=>dt, new EventDateTimeProvider());

                builder.AddProvider((int? i, CatalogValueAttribute cva) => i, new CatalogRepositoryProvider(catalogRepository));

                builder.AddInstanceModifier(new NullModifier(_randomGenerator.NullPercentage));
              }
          });

      #region Entry created & changed

      _entryCreatedEvents = generator.CreateMany<TEntryCreatedEvent>(parameters.NumberOfEntryCreatedEvents);
      _entryChangedEvents = generator.CreateMany<TEntryChangedEvent>(parameters.NumberOfEntryChangedEvents);
      #endregion

      //#region Note created & changed & deleted

      _noteCreatedEvents = generator.CreateMany<NoteCreatedEvent>(parameters.NumberOfNoteCreatedEvents);
      _noteChangedEvents = generator.CreateMany<NoteChangedEvent>(parameters.NumberOfNoteChangedEvents);
      _noteDeletedEvents = generator.CreateMany<NoteDeletedEvent>(parameters.NumberOfNoteDeletedEvents);


      //#endregion

      //#region Printings created & deleted

      _printingCreatedEvents = generator.CreateMany<PrintingCreatedEvent>(parameters.NumberOfPrintingCreatedEvents);
      _printingDeletedEvents = generator.CreateMany<PrintingDeletedEvent>(parameters.NumberOfPrintingDeletedEvents);

      //#endregion

      //#region Entry deleted
      _entryDeletedEvents = generator.CreateMany<EntryDeletedEvent>(parameters.NumberOfEntryDeletedEvents);
      //#endregion

      Console.WriteLine();
    }

    protected abstract void MutateEvent (TEntryCreatedEvent evt, SharedPersonData sharedPersonData);
    protected abstract void MutateEvent (TEntryChangedEvent evt, SharedPersonData sharedPersonData);

    private IReadOnlyList<T> CreateEvents<T> (int numberOfEvents, params ISpecimenBuilder[] specimenBuilders)
        where T : EventBase
    {
      Console.WriteLine("Creating {0} {1}s.", numberOfEvents, typeof (T).Name);

      

      var fixture = _fixtureFactory();
      var builders = new Queue<ISpecimenBuilder>(specimenBuilders);
      fixture.Customizations.AddMany(builders.Dequeue, builders.Count);

      var events = fixture.CreateMany<T>(numberOfEvents);
      return events.ToList();
    }

    public void AppendBatch ()
    {
      // Prepare EntryCreated events first, other events depend on them
      for (int i = 0; i < _entryCreatedEvents.Count; ++i)
      {
        _entryCreatedEvents[i].EntryId = i;
      }

      if (!_parameters.PrintingsOnlyMode)
      {
        #region Entry created & changed
        Console.WriteLine("Appending {0} {1}s", _entryCreatedEvents.Count, typeof (TEntryCreatedEvent).Name);
        _eventAppender.AppendEvents(_entryCreatedEvents.Select(CreateAppendableEvent));
        _eventAppender.AppendEvents(_entryChangedEvents.Select(CreateAppendableEvent));

        #endregion

        #region Note created & changed & deleted

        /*Console.WriteLine("Appending {0} {1}s for random entries.", _noteCreatedEvents.Count, typeof (NoteCreatedEvent).Name);
        var nextNoteIdByEntry = new Dictionary<int, int>();
        for (int i = 0; i < _noteCreatedEvents.Count; ++i)
        {
          var entryId = _randomGenerator.PickRandom(_entryCreatedEvents).EntryId;
          _noteCreatedEvents[i].EntryId = entryId;

          int noteId;
          nextNoteIdByEntry.TryGetValue(entryId, out noteId);
          _noteCreatedEvents[i].NoteId = noteId;
          nextNoteIdByEntry[entryId] = noteId + 1;
        }
        _eventAppender.AppendEvents(_noteCreatedEvents.Select(CreateAppendableEvent));

        Console.WriteLine("Appending {0} {1}s for random entries.", _entryChangedEvents.Count, typeof (NoteChangedEvent).Name);
        for (int i = 0; i < _noteChangedEvents.Count; ++i)
        {
          var noteCreatedEvent = _randomGenerator.PickRandom(_noteCreatedEvents);

          _noteChangedEvents[i].EntryId = noteCreatedEvent.EntryId;
          _noteChangedEvents[i].NoteId = noteCreatedEvent.NoteId;
        }
        _eventAppender.AppendEvents(_noteChangedEvents.Select(CreateAppendableEvent));

        Console.WriteLine("Appending {0} {1}s.", _noteDeletedEvents.Count, typeof (NoteDeletedEvent).Name);
        for (int i = 0; i < _noteDeletedEvents.Count; ++i)
        {
          _noteDeletedEvents[i].EntryId = _noteCreatedEvents[i].EntryId;
          _noteDeletedEvents[i].NoteId = _noteCreatedEvents[i].NoteId;
        }
        _eventAppender.AppendEvents(_noteDeletedEvents.Select(CreateAppendableEvent));
        */
        #endregion
      }
      else
      {
        Console.WriteLine("Printings Only mode => No entries or notes are created, changed, or deleted.");
      }

      #region Printing created & deleted

      /*Console.WriteLine("Appending {0} {1}s for random entries.", _printingCreatedEvents.Count, typeof (PrintingCreatedEvent).Name);
      var nextPrintingIdByEntry = new Dictionary<int, int>();
      for (int i = 0; i < _printingCreatedEvents.Count; ++i)
      {
        var entryId = _randomGenerator.PickRandom(_entryCreatedEvents).EntryId;
        _printingCreatedEvents[i].EntryId = entryId;

        int printingId;
        nextPrintingIdByEntry.TryGetValue(entryId, out printingId);
        _printingCreatedEvents[i].PrintingId = printingId;
        nextPrintingIdByEntry[entryId] = printingId + 1;
      }
      _eventAppender.AppendEvents(_printingCreatedEvents.Select(CreateAppendableEvent));

      Console.WriteLine("Appending {0} {1}s.", _printingDeletedEvents.Count, typeof (PrintingDeletedEvent).Name);
      for (int i = 0; i < _printingDeletedEvents.Count; ++i)
      {
        _printingDeletedEvents[i].EntryId = _printingCreatedEvents[i].EntryId;
        _printingDeletedEvents[i].PrintingId = _printingCreatedEvents[i].PrintingId;
      }
      _eventAppender.AppendEvents(_printingDeletedEvents.Select(CreateAppendableEvent));
      */
      #endregion

      if (!_parameters.PrintingsOnlyMode)
      {
        #region Entry deleted

        /*Console.WriteLine("Appending {0} {1}s.", _entryDeletedEvents.Count, typeof (EntryDeletedEvent).Name);
        for (int i = 0; i < _entryDeletedEvents.Count; ++i)
          _entryDeletedEvents[i].EntryId = _entryCreatedEvents[i].EntryId;
        _eventAppender.AppendEvents(_entryDeletedEvents.Select(CreateAppendableEvent));
        */
        #endregion
      }
    }

    private AppendableEvent CreateAppendableEvent (EventBase entryEvent)
    {
      var appendableEvent = new AppendableEvent(entryEvent, "user");
      return appendableEvent;
    }

    protected void MutateOrdinalAndDateOfEntry (EntryDataBase entryData)
    {
      entryData.Ordinal = _randomGenerator.GenerateOrdinal();
      entryData.DateOfEntry = _randomGenerator.GenerateRequiredDateOrYearData();
    }

    protected void MutateNames (params NameWithNameBeforeMarriageData[] names)
    {
      var nameBeforeMarriageDatas = _randomGenerator.GenerateNameBeforeMarriageData(names.Length).ToList();

      for (var index = 0; index < names.Length; index++)
      {
        var name = names[index];
        var otherName = nameBeforeMarriageDatas[index];

        name.LastName = otherName.LastName;
        name.FirstName = otherName.FirstName;
        name.LastNameBeforeMarriage = otherName.LastNameBeforeMarriage;
      }
    }

    protected void MutateJmbg (params FullPersonData[] persons)
    {
      foreach (var person in persons)
        person.Jmbg = _randomGenerator.GenerateJmbg();
    }

    protected void MutateJmbg (params PersonData[] persons)
    {
      foreach (var person in persons)
        person.Jmbg = _randomGenerator.GenerateJmbg();
    }

    protected void MutateBirths (params FullPersonData[] persons)
    {
      foreach (var person in persons)
        person.DateOfBirth = _randomGenerator.GenerateDateOrYearData();
    }

    protected SharedPersonData ApplyTypo (SharedPersonData personData)
    {
      return new SharedPersonData
             {
                 Jmbg = personData.Jmbg,
                 FirstName = _randomGenerator.ApplyTypo(personData.FirstName),
                 LastName = _randomGenerator.ApplyTypo(personData.LastName),
                 DateOfBirth = _randomGenerator.ApplyTypo(personData.DateOfBirth),
                 FirstNameOfFather = _randomGenerator.ApplyTypo(personData.FirstNameOfFather)
             };
    }
  }

  public class CatalogRepositoryProvider:AttributeValueProvider<int?, CatalogValueAttribute>
  {
    private readonly ICatalogRepository _catalogRepository;

    public CatalogRepositoryProvider (ICatalogRepository catalogRepository)
    {
      _catalogRepository = catalogRepository;
    }

    protected override int? CreateValue (AttributeValueProviderContext<int?, CatalogValueAttribute> context)
    {
      var allIds = _catalogRepository.GetAllEntries(context.Attribute.Catalog).Select(x => x.Id).ToArray();
      return allIds[context.Random.Next(allIds.Length)];
    }
  }

  public class EventDateTimeProvider:ValueProvider<DateTime>
  {
    protected override DateTime CreateValue (ValueProviderContext<DateTime> context)
    {
      var dateTime = context.GetPreviousValue();

      var dateTimeKindAttribute = context.PropertyInfo.GetCustomAttribute<RequiredDateTimeKindAttribute>();

      if (context.PropertyInfo.IsDefined(typeof (DateAttribute)))
        dateTime = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Unspecified);
      else if (dateTimeKindAttribute != null)
        dateTime = DateTime.SpecifyKind(dateTime, dateTimeKindAttribute.Kind);

      return dateTime;
    }
  }
}