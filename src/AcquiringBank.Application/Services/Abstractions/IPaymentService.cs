using System.Threading.Tasks;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Abstractions;

public interface IPaymentService
{
    Task<PaymentTransactionResponse> ProcessTransaction(PaymentTransactionRequest paymentTransactionRequest);
}