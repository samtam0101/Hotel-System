using Domain.DTOs.PaymentDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class PaymentController(IPaymentService paymentService):ControllerBase
{
    [HttpGet("Get-Payments")]
    public async Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync([FromQuery]PaymentFilter filter)
    {
        return await paymentService.GetPaymentsAsync(filter);
    }
    [HttpPost("Add-Payment")]
    public async Task<Response<string>> AddPaymentAsync(AddPaymentDto addPaymentDto)
    {
        return await paymentService.AddPaymentAsync(addPaymentDto);
    }
    [HttpPut("Update-Payment")]
    public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePaymentDto)
    {
        return await paymentService.UpdatePaymentAsync(updatePaymentDto);
    }
    [HttpGet("Get-PaymentById")]
    public async Task<Response<GetPaymentDto>>GetPaymentByIdAsync(int id)
    {
        return await paymentService.GetPaymentByIdAsync(id);
    }
    [HttpDelete("Delete-Payment")]
    public async Task<Response<bool>> DeletePaymentAsync(int id)
    {
        return await paymentService.DeletePaymentAsync(id);
    }
}
