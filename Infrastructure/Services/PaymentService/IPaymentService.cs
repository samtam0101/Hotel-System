using Domain.DTOs.PaymentDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PaymentService;

public interface IPaymentService
{
    Task<Response<string>> AddPaymentAsync(AddPaymentDto addPaymentDto);
    Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePaymentDto);
    Task<Response<bool>> DeletePaymentAsync(int id );
    Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int id);
    Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaymentFilter filter);
}
