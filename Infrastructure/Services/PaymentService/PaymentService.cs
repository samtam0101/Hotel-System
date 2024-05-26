using System.Net;
using AutoMapper;
using Domain.DTOs.PaymentDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.PaymentService;

public class PaymentService(ILogger<PaymentService> logger, IMapper mapper, DataContext context) : IPaymentService
{
    public async Task<Response<string>> AddPaymentAsync(AddPaymentDto addPaymentDto)
    {
        try
        {
            logger.LogInformation("AddPayment method started at {DateTime}", DateTime.Now);
            var existing = await context.Payments.AnyAsync(e => e.Id == addPaymentDto.UserId);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "User already exists!");
            var mapped = mapper.Map<Payment>(addPaymentDto);
            await context.Payments.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddPayment method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeletePaymentAsync(int id)
    {
        try
        {
            logger.LogInformation("AddPayment method started at {DateTime}", DateTime.Now);
            var existing = await context.Payments.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("Payment not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Payment not found!");
            }
            logger.LogInformation("AddPayment method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaymentFilter filter)
    {
        try
        {
            logger.LogInformation("AddPayment method started at {DateTime}", DateTime.Now);

            var payments = context.Payments.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Status))
                payments = payments.Where(x => x.Status.ToLower().Contains(filter.Status.ToLower()));
            var result = await payments.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await payments.CountAsync();

            var response = mapper.Map<List<GetPaymentDto>>(result);
            logger.LogInformation("AddPayment method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetPaymentDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetPaymentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("AddPayment method started at {DateTime}", DateTime.Now);

            var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                logger.LogWarning("Payment not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetPaymentDto>(HttpStatusCode.BadRequest, "Payment not found");
            }
            var payments = mapper.Map<GetPaymentDto>(existing);
            logger.LogInformation("AddPayment method finished at {DateTime}", DateTime.Now);
            return new Response<GetPaymentDto>(payments);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetPaymentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePaymentDto)
    {
        try
        {
            logger.LogInformation("AddPayment method started at {DateTime}", DateTime.Now);

            var existing = await context.Payments.AnyAsync(e => e.Id == updatePaymentDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Payment not found!");
            var mapped = mapper.Map<Payment>(updatePaymentDto);
            context.Payments.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddPayment method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
