using Microsoft.AspNetCore.Mvc;
using PharmacyApi.Data;
using PharmacyApi.Models;

namespace PharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(SalesRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SaleRecord>>> GetAll()
    {
        return Ok(await repository.GetAllAsync());
    }
}
