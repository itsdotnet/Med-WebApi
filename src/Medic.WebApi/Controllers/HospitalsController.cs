using Medic.Service.DTOs.Hospitals;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class HospitalsController : BaseController
{
    private readonly IHospitalService _hospitalService;

    public HospitalsController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllHospitals()
    {
        var hospitals = await _hospitalService.GetAllAsync();
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = hospitals
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var hospital = await _hospitalService.GetByIdAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = hospital
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateHospital([FromBody] HospitalCreationDto hospitalCreationDto)
    {
        var createdHospital = await _hospitalService.CreateAsync(hospitalCreationDto);
        return Ok(new Response
        {
            StatusCode = 201,
            Message = "Hospital created successfully",
            Data = createdHospital
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateHospital([FromBody] HospitalUpdateDto hospitalUpdateDto)
    {
        var updatedHospital = await _hospitalService.UpdateAsync(hospitalUpdateDto);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Hospital updated successfully",
            Data = updatedHospital
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteHospital(long id)
    {
        var result = await _hospitalService.DeleteAsync(id);
        if (result)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Hospital deleted successfully"
            });
        }
        return NotFound(new Response
        {
            StatusCode = 404,
            Message = "Hospital not found"
        });
    }
}