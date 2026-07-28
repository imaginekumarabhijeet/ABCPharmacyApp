using Microsoft.AspNetCore.Mvc;
using PharmacyApi.Data;
using PharmacyApi.Models;
using PharmacyApi.Services;

namespace PharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController(MedicineRepository repository, SalesService salesService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Medicine>>> GetAll([FromQuery] string? search)
    {
        return Ok(await repository.GetAllAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Medicine>> GetById(string id)
    {
        var medicine = await repository.GetByIdAsync(id);
        return medicine is null ? NotFound() : Ok(medicine);
    }

    [HttpPost]
    public async Task<ActionResult<Medicine>> Create([FromBody] CreateMedicineDto dto)
    {
        var medicine = new Medicine
        {
            FullName = dto.FullName.Trim(),
            Notes = dto.Notes?.Trim() ?? string.Empty,
            ExpiryDate = dto.ExpiryDate,
            Quantity = dto.Quantity,
            Price = Math.Round(dto.Price, 2, MidpointRounding.AwayFromZero),
            Brand = dto.Brand.Trim()
        };

        var created = await repository.AddAsync(medicine);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id}/sell")]
    public async Task<ActionResult<SaleRecord>> Sell(string id, [FromBody] SellRequestDto dto)
    {
        var (success, error, sale) = await salesService.SellAsync(id, dto.Quantity);
        if (!success)
        {
            return BadRequest(new { message = error });
        }

        return Ok(sale);
    }
}
