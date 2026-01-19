// Domain/Events/CandidateChangedEvent.cs
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enumerators;

namespace ApplicantTracking.Domain.Events;

public record CandidateChangedEvent(
    TimelineTypes Type,
    int CandidateId,
    Candidate? OldValue,
    Candidate? NewValue);
