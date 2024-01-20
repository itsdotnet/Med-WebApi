using Medic.Service.DTOs.Doctors;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class DoctorsController : BaseController
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = doctors
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetDoctorById(long id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = doctor
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreationDto doctorCreationDto)
    {
        var createdDoctor = await _doctorService.CreateAsync(doctorCreationDto);
        return Ok(new Response
        {
            StatusCode = 201,
            Message = "Doctor created successfully",
            Data = createdDoctor
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateDoctor(DoctorUpdateDto doctorUpdateDto)
    {
        var updatedDoctor = await _doctorService.UpdateAsync(doctorUpdateDto);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Doctor updated successfully",
            Data = updatedDoctor
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteDoctor(long id)
    {
        var result = await _doctorService.DeleteAsync(id);
        if (result)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Doctor deleted successfully"
            });
        }
        return NotFound(new Response
        {
            StatusCode = 404,
            Message = "Doctor not found"
        });
    }
}
