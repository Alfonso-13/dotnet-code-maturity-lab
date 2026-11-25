namespace DotnetCodeMaturityLab.Features.Orders;

public sealed record MiddleLabEvidence(bool Passed, string Detail);

public interface IMiddleLabEvidenceService
{
    Task<MiddleLabEvidence> VerifyAsync(CancellationToken cancellationToken = default);
}
