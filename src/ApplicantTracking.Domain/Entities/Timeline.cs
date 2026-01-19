namespace ApplicantTracking.Domain.Entities;

public class Timeline
{
    public int IdTimeline { get; private set; }
    public byte IdTimelineType { get; private set; }
    public int IdAggregateRoot { get; private set; }
    public string? OldData { get; private set; }
    public string? NewData { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Timeline Create(byte type, int aggregateId, string? oldData, string? newData)
        => new Timeline
        {
            IdTimelineType = type,
            IdAggregateRoot = aggregateId,
            OldData = oldData,
            NewData = newData,
            CreatedAt = DateTime.UtcNow
        };
}
