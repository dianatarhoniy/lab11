using Clinic.Application.DTOs;
using Clinic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController(IPrescriptionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(PrescriptionRequestDto dto, CancellationToken ct)
    {
        var id = await service.AddPrescriptionAsync(dto, ct);
        return CreatedAtRoute(
            routeName: "GetPatient",
            routeValues: new { id = dto.Patient.IdPatient ?? 0 },
            value: new { IdPrescription = id });
    }
}
