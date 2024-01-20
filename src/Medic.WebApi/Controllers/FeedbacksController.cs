using Medic.Service.DTOs.Feedbacks;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class FeedbacksController : BaseController
{
    private readonly IFeedbackService _feedbackService;

    public FeedbacksController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllFeedbacks()
    {
        var feedbacks = await _feedbackService.GetAllAsync();
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = feedbacks
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetFeedbackById(long id)
    {
        var feedback = await _feedbackService.GetByIdAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = feedback
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreationDto feedbackCreationDto)
    {
        var createdFeedback = await _feedbackService.CreateAsync(feedbackCreationDto);
        return Ok(new Response
        {
            StatusCode = 201,
            Message = "Feedback created successfully",
            Data = createdFeedback
        });
    }

    [HttpDelete("get/{id}")]
    public async Task<IActionResult> DeleteFeedback(long id)
    {
        var result = await _feedbackService.DeleteAsync(id);
        if (result)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Feedback deleted successfully"
            });
        }
        return NotFound(new Response
        {
            StatusCode = 404,
            Message = "Feedback not found"
        });
    }

    [HttpGet("get-by-user/{userId}")]
    public async Task<IActionResult> GetFeedbacksByUser(long userId)
    {
        var feedbacks = await _feedbackService.GetByUserAsync(userId);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = feedbacks
        });
    }

    [HttpGet("get-bet-doctor/{doctorId}")]
    public async Task<IActionResult> GetFeedbacksByDoctor(long doctorId)
    {
        var feedbacks = await _feedbackService.GetByDoctorAsync(doctorId);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = feedbacks
        });
    }
}