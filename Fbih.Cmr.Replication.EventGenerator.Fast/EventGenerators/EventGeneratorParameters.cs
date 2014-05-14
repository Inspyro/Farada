using System;

namespace Fbih.Cmr.Replication.EventGenerator.Tool.Fast.EventGenerators
{
  public class EventGeneratorParameters
  {
    public bool PrintingsOnlyMode;
    public int MunicipalityId { get; set; }
    public int PrintingMunicipalityId { get; set; }
    public int NumberOfEntryCreatedEvents { get; set; }
    public int NumberOfEntryChangedEvents { get; set; }
    public int NumberOfNoteCreatedEvents { get; set; }
    public int NumberOfNoteChangedEvents { get; set; }
    public int NumberOfNoteDeletedEvents { get; set; }
    public int NumberOfEntryDeletedEvents { get; set; }
    public int NumberOfPrintingCreatedEvents { get; set; }
    public int NumberOfPrintingDeletedEvents { get; set; }
  }
}