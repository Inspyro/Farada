using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Autofac;
using Fbih.Cmr.Domain.ExternalCatalogs;
using Fbih.Cmr.Domain.ExternalCatalogs.Configuration;
using Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators;
using Ploeh.AutoFixture;
using Rubicon.RegisterNova.Infrastructure.EventStore;
using Rubicon.RegisterNova.Infrastructure.EventStoreRegistration;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast
{
  public static class EventGeneratorProgram
  {
    private const string c_connectionStringNameEventStore = "EventStore";

    private const double c_entryChangedEventsFactor = 0.2;
    private const double c_entryDeletedEventsFactor = 0.05;

    private const double c_noteChangedEventsFactor = 0.2;
    private const double c_noteDeletedEventsFactor = 0.05;

    private const double c_entryPrintingCreatedEventsFactor = 2.0;
    private const double c_entryPrintingDeletedEventsFactor = 0.05;

    private const double c_shareFactor = 0.8;
    private const double c_meaningfulFactor = 0.1;
    private const double c_nullFactor = 0.01;
    private const double c_typoFactor = 0.01;

    private static int Main (string[] args)
    {

      int totalNumberOfEntryCreatedEvents;
      int firstEntryId;
      int municipalityId;
      int printingMunicipalityId;
      bool printingsOnlyMode;

      ParseArguments(
          args,
          out totalNumberOfEntryCreatedEvents,
          out firstEntryId,
          out municipalityId,
          out printingMunicipalityId,
          out printingsOnlyMode);

      var stopWatch = Stopwatch.StartNew();

      var parameters = new EventGeneratorParameters
                       {
                           PrintingsOnlyMode = printingsOnlyMode,
                           MunicipalityId = municipalityId,
                           PrintingMunicipalityId = printingMunicipalityId
                       };

      parameters.NumberOfEntryCreatedEvents = totalNumberOfEntryCreatedEvents;
      parameters.NumberOfEntryChangedEvents = (int) (totalNumberOfEntryCreatedEvents * c_entryChangedEventsFactor);
      parameters.NumberOfEntryDeletedEvents = (int) (totalNumberOfEntryCreatedEvents * c_entryDeletedEventsFactor);

      parameters.NumberOfNoteCreatedEvents = totalNumberOfEntryCreatedEvents;
      parameters.NumberOfNoteChangedEvents = (int) (parameters.NumberOfNoteCreatedEvents * c_noteChangedEventsFactor);
      parameters.NumberOfNoteDeletedEvents = (int) (parameters.NumberOfNoteCreatedEvents * c_noteDeletedEventsFactor);

      parameters.NumberOfPrintingCreatedEvents = (int) (totalNumberOfEntryCreatedEvents * c_entryPrintingCreatedEventsFactor);
      parameters.NumberOfPrintingDeletedEvents = (int) (parameters.NumberOfPrintingCreatedEvents * c_entryPrintingDeletedEventsFactor);

      using (var container = BuildContainer())
      {
        var randomGenerator = new RandomGenerator(c_nullFactor, c_meaningfulFactor, c_typoFactor);
        var fixtureFactory = container.Resolve<Func<IFixture>>();
        var eventAppender = container.Resolve<IEventAppender>();
        var catalogRepository = container.Resolve<ICatalogRepository>();

        var birthEntryEventGenerator = new BirthEventGenerator(fixtureFactory, eventAppender, catalogRepository, randomGenerator, parameters);
        var marriageEntryEventGenerator = new MarriageEventGenerator(fixtureFactory, eventAppender, catalogRepository, randomGenerator, parameters);
        var deathEntryEventGenerator = new DeathEventGenerator(fixtureFactory, eventAppender, catalogRepository, randomGenerator, parameters);
        var citizenEntryEventGenerator = new CitizenEventGenerator(fixtureFactory, eventAppender, catalogRepository, randomGenerator, parameters);
        Console.WriteLine("Generating events took '{0}'.", stopWatch.Elapsed);

        stopWatch.Restart();

        birthEntryEventGenerator.AppendBatch();
        deathEntryEventGenerator.AppendBatch();
        marriageEntryEventGenerator.AppendBatch();
        citizenEntryEventGenerator.AppendBatch();
      }

      Console.WriteLine("Appending events took '{0}'.", stopWatch.Elapsed);
      Console.Write("Press any key to continue...");
      Console.ReadLine();
      return 0;
    }

    private static IContainer BuildContainer ()
    {
      var builder = new ContainerBuilder();

      // Specify that every event gets its own commit. This is more realistic in the FBiH CMR register domain than stuffing a lot of events into a 
      // single commit.
      builder.RegisterModule(new EventAppenderModule(1));
      builder.RegisterModule(new EventStoreModule(new SqlPersistence(c_connectionStringNameEventStore)));

      var catalogSettingsConfiguration = (CatalogSettingsConfigurationSection) ConfigurationManager.GetSection("catalogs");
      builder.RegisterModule(new CatalogModule(catalogSettingsConfiguration.BasePath));

      builder.RegisterType<Fixture>().As<IFixture>();

      return builder.Build();
    }

    private static void ParseArguments (
        string[] args,
        out int numberOfEntryCreatedEvents,
        out int firstEntryId,
        out int municipalityId,
        out int printingMunicipalityId,
        out bool printingsOnlyMode)
    {
      if (args.Length > 0 && StringComparer.OrdinalIgnoreCase.Equals(args[0], "/PrintingsOnly"))
      {
        Console.WriteLine("Printings Only mode. Generating only printing events.");
        printingsOnlyMode = true;
        args = args.Skip(1).ToArray();
      }
      else
      {
        Console.WriteLine("To generate only printing events, specify /PrintingsOnly as the first parameter.");
        printingsOnlyMode = false;
      }

      if (args.Length != 4)
      {
        args = new string[4];
        Console.Write("Number of entries per book: ");
        args[0] = Console.ReadLine();
        Console.Write("First entry id per book: ");
        args[1] = Console.ReadLine();
        Console.Write("Municipality id: ");
        args[2] = Console.ReadLine();
        Console.Write("Printing municipality id: ");
        args[3] = Console.ReadLine();
        Console.WriteLine();
      }

      numberOfEntryCreatedEvents = int.Parse(args[0], CultureInfo.CurrentCulture);
      firstEntryId = int.Parse(args[1], CultureInfo.CurrentCulture);
      municipalityId = int.Parse(args[2], CultureInfo.CurrentCulture);
      printingMunicipalityId = int.Parse(args[3], CultureInfo.CurrentCulture);
    }
  }
}