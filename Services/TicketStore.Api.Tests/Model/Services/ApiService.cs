using System;
using RestSharp;
using TicketStore.Api.Tests.Model.Services.UploadPoster;
using TicketStore.Api.Tests.Model.Services.Verify.Requests;

namespace TicketStore.Api.Tests.Model.Services
{
    public class ApiService : TicketStoreService
    {
        protected override int Port()
        {
            return 3000;
        }

        public IRestResponse SendTestPayment()
        {
            var request = new RestRequest("api/payments", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("test_notification", true);
            return Client.Execute(request);
        }

        public IRestResponse SendPayment(String sender, YandexPaymentLabel label, String email, Decimal withdraw_amount, Decimal amount)
        {
            var request = new RestRequest("api/payments", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("sender", sender);
            request.AddParameter("label", label.Value());
            request.AddParameter("email", email);
            request.AddParameter("withdraw_amount", withdraw_amount);
            request.AddParameter("amount", amount);
            return Client.Execute(request);
        }

        public IRestResponse VerifyBarcode(TurnstileScan scan)
        {
            var request = CreateVerifyRequest(scan);
            request.AddHeader("Authorization", "Bearer pkR9vfZ9QdER53mf");
            return Client.Execute(request);
        }

        public IRestResponse VerifyBarcodeWithoutAuth(TurnstileScan scan)
        {
            var request = CreateVerifyRequest(scan);
            return Client.Execute(request);
        }

        private RestRequest CreateVerifyRequest(TurnstileScan scan)
        {
            var request = new RestRequest("api/verify", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(scan);
            return request;
        }

        public IRestResponse UploadPoster(Poster poster)
        {
            var request = new RestRequest("api/uploads/poster", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(poster);
            return Client.Execute(request);
        }
    }
}
