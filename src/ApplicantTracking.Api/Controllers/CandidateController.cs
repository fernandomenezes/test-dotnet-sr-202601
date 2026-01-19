using ApplicantTracking.Application.Commands;
using ApplicantTracking.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantTracking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/candidate
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCandidatesQuery(), ct);
        return Ok(result);
    }

    // GET: api/candidate/5
    [HttpGet("{idCandidate:int}")]
    public async Task<IActionResult> Get([FromRoute] int idCandidate, CancellationToken ct)
    {
        var candidates = await _mediator.Send(new GetCandidatesQuery(), ct);
        var candidate = candidates.FirstOrDefault(c => c.IdCandidate == idCandidate);
        if (candidate is null) return NotFound();
        return Ok(candidate);
    }

    // POST: api/candidate
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCandidateCommand cmd, CancellationToken ct)
    {
        var dto = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(Get), new { idCandidate = dto.IdCandidate }, dto);
    }

    // PUT: api/candidate/5
    [HttpPut("{idCandidate:int}")]
    public async Task<IActionResult> Edit([FromRoute] int idCandidate, [FromBody] UpdateCandidateCommand cmd, CancellationToken ct)
    {
        if (idCandidate != cmd.IdCandidate) return BadRequest("Id mismatch");
        var dto = await _mediator.Send(cmd, ct);
        return Ok(dto);
    }

    // DELETE: api/candidate/5
    [HttpDelete("{idCandidate:int}")]
    public async Task<IActionResult> Delete([FromRoute] int idCandidate, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCandidateCommand(idCandidate), ct);
        return NoContent();
    }
}
