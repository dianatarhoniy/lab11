using Clinic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetPatient")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
        => Ok(await service.GetPatientAsync(id, ct));
}
