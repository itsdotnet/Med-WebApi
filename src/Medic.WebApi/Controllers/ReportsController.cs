using Medic.Service.DTOs.Reports;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class ReportsController : BaseController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllReports()
    {
        var reports = await _reportService.GetAllAsync();
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = reports
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var report = await _reportService.GetByIdAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = report
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateReport([FromBody] ReportCreationDto reportCreationDto)
    {
        var createdReport = await _reportService.CreateAsync(reportCreationDto);
        return Ok(new Response
        {
            StatusCode = 201,
            Message = "Report created successfully",
            Data = createdReport
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateReport([FromBody] ReportUpdateDto reportUpdateDto)
    {
        var updatedReport = await _reportService.UpdateAsync(reportUpdateDto);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Report updated successfully",
            Data = updatedReport
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteReport(long id)
    {
        var result = await _reportService.DeleteAsync(id);
        if (result)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Report deleted successfully"
            });
        }
        return NotFound(new Response
        {
            StatusCode = 404,
            Message = "Report not found"
        });
    }
}