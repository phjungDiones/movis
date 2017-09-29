

/// <summary>
/// Event Log에 기록합니다.
/// </summary>
/// <param name="sSource">dotNET Sample App</param>
/// <param name="sLog">Application</param>
/// <param name="sEvent">Sample Event</param>
public static void WriteEventLog(string sSource, string sLog, string sEvent)
{
    sSource = "dotNET Sample App";
    sLog = "Application";
    sEvent = "Sample Event";

    if (!EventLog.SourceExists(sSource))
        EventLog.CreateEventSource(sSource, sLog);

    EventLog.WriteEntry(sSource, sEvent);
    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);


	// Create an EventLog instance and assign its source.
    EventLog myLog = new EventLog();
    myLog.Source = "MySource";

    // Write an informational entry to the event log.    
    myLog.WriteEntry("Writing to event log.");
}