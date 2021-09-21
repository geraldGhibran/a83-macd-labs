using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BooksCatalogue.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksCatalogue.Controllers
{
    public class ReviewController : Controller
    {
        private string apiEndpoint = "https://cataloggeraldapi.azurewebsites.net/api/";
        private readonly HttpClient _client;
        HttpClientHandler clientHandler = new HttpClientHandler();
        public ReviewController()
        {
            _client = new HttpClient(clientHandler);
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        // GET: Review/AddReview/2
        public async Task<IActionResult> AddReview(int? bookId)
        {

            return View("AddReview");

        }

        

        // TODO: Tambahkan fungsi ini untuk mengirimkan atau POST data review menuju API
        // POST: Review/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddReview([Bind("Id,BookId,ReviewerName,Rating,Comment")][FromForm] Review review)
        {

            MultipartFormDataContent content = new MultipartFormDataContent();

            content.Add(new StringContent(review.BookId.ToString()), "bookid");
            content.Add(new StringContent(review.Comment), "comment");
            content.Add(new StringContent(review.ReviewerName), "reviewername");
            content.Add(new StringContent(review.Rating.ToString()), "rating");


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint + "review/");
            request.Content = content;
            HttpResponseMessage response = await _client.SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Created:
                    return Redirect("/");
                default:
                    return ErrorAction("Error. Status code = " + response.StatusCode + "; " + response.ReasonPhrase);
            }

        }

        private ActionResult ErrorAction(string message)
        {
            return new RedirectResult("/Home/Error?message=" + message);
        }
    }
}