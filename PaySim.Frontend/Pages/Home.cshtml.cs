using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaySlip.Domain.Constants;
using PaySlip.Domain.Models;

namespace PaySim.Frontend.Pages
{
    public class HomeModel : PageModel
    {
        public bool IsLoading { get; set; } = false;

        private readonly HttpClient _httpClient;
        public HomeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        public IEnumerable<Transaction>? Transactions { get; set; }
        public decimal WalletBalance { get; set; }
        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            Transactions = await _httpClient.GetFromJsonAsync<IEnumerable<Transaction>>("Pay/TransactionHistory");
            WalletBalance = await _httpClient.GetFromJsonAsync<decimal>("Pay/WalletBalance");
        }
        public string Message { get; set; }
        public async Task<IActionResult> OnPostCancelAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"Pay/CancelTransaction/{id}", null);
            if(response.IsSuccessStatusCode)
            {
                Message = "Transaction cancelled successfully! Money has been refunded to your wallet. :)";
                await LoadDataAsync();
                return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
            }
            else
            {
                Message = "Failed to cancel transaction. :(";
                await LoadDataAsync();
                return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
            }
        }

        [BindProperty]
        public string PaymentCredential { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public PaymentMethod PaymentMethod { get; set; }
        public async Task<IActionResult> OnPostProceedToPayAsync()
        {
            var firstResponse = await _httpClient.PostAsync($"Pay/CreateDeposit/?amount={Amount}&paymentMethod={PaymentMethod}", null);
            if (firstResponse.IsSuccessStatusCode)
            {
                var apiResult = await firstResponse.Content.ReadFromJsonAsync<Transaction>();
                var initiatePayment = await _httpClient.PostAsync($"Pay/ProcessPayment/{apiResult?.TransactionId}/{PaymentCredential}", null);
                if (initiatePayment.IsSuccessStatusCode)
                {
                    Message = "Payment done successfully. :)";
                    await LoadDataAsync();
                    return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
                }
                else
                {
                    Message = "Payment failed. Please try again :(";
                    await LoadDataAsync();
                    return Content($@"<script>alert('{Message}');</script>", "text/html");
                }
            }
            else
            {
                Message = "Insufficient Balance. Failed to initiate the payment. Please check your wallet balance. :(";
                await LoadDataAsync();
                return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
            }
            
        }

        [BindProperty]
        public decimal AddAmt { get; set; }
        public async Task<IActionResult> OnPostAddToWalletAsync()
        {
            var response = await _httpClient.PostAsync($"Pay/AddWalletBalance/{AddAmt}", null);
            if (response.IsSuccessStatusCode)
            {
                Message = "Money added to your wallet. Happy transacting! :)";
                await LoadDataAsync();
                return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
            }
            else
            {
                Message = "Failed to add money to your wallet. Please try again. :(";
                await LoadDataAsync();
                return Content($@"<script>alert('{Message}'); window.location.href='/Home';</script>", "text/html");
            }

        }

    }
}
