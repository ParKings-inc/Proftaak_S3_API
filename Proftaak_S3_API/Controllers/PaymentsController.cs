using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using System.Globalization;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        IPaymentClient paymentClient;
        public PaymentsController()
        {
            paymentClient = new PaymentClient("test_3zANg8a2rfkhPuK7GnN7QrewHxRmRd");
        }

        [HttpPost("{cost}")]
        public async Task<ActionResult<PaymentResponse>> CreatePayment(decimal cost)
        {
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, cost),
                Description = "Parking fees",
                RedirectUrl = "http://localhost:3000/",
            };

            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
            return paymentResponse;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponse>> GetPaymentById(string id)
        {
            PaymentResponse paymentResponse;
            try
            {
                paymentResponse = await paymentClient.GetPaymentAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
            
            return paymentResponse;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ListResponse<PaymentResponse>>> GetAllPayments()
        {
            ListResponse<PaymentResponse> response = await paymentClient.GetPaymentListAsync();
            return response;
        }

    }
}