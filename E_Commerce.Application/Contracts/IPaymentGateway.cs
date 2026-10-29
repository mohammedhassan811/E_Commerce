using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        //Create PaymentIntent
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default);
        //Update PaymentIntent
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string PaymentIntentId, CancellationToken ct = default);
    }
}
