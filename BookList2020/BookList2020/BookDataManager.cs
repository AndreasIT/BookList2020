using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookList2020
{
    class BookDataManager
    {
        HttpClient _client;

        public BookDataManager()
        {
            _client = new HttpClient(new ErrorHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri("https://shrouded-waters-99136.herokuapp.com/")
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var response = await _client.GetStringAsync("books");
            return JsonConvert.DeserializeObject<IEnumerable<Book>>(response);
         }

        public async Task CreateBook(Book book)
        {
            var request = JsonConvert.SerializeObject(book); 
            await _client.PostAsync("books", new StringContent(request, Encoding.UTF8, "application/json"));
        }

        public async Task PutBook(int id, Book book)
        {
            var request = JsonConvert.SerializeObject(book);
            await _client.PutAsync($"books/{id}", new StringContent(request));
        }

        public async Task DeleteBook(int id)
        {
            await _client.DeleteAsync($"books/{id}");
        }
    }

    public class ErrorHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue _authHeader;
        private Login _login;

        public ErrorHandler(HttpMessageHandler handler) : base(handler) 
        {
            _login = new Login
            {
                Identifier = "mdam@ucl.dk",
                Password = "Student1234"
            };
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SetupAuthentication(request, cancellationToken);
            request.Headers.Authorization = _authHeader;
            var response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task SetupAuthentication(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_authHeader == null)
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post,  $"https://{request.RequestUri.Host}/auth/local");
                var loginContent = JsonConvert.SerializeObject(_login);
                tokenRequest.Content = new StringContent(loginContent, Encoding.UTF8, "application/json");
                var response = await base.SendAsync(tokenRequest, cancellationToken);
                response.EnsureSuccessStatusCode();
                var token = JsonConvert.DeserializeObject<ApiToken>(await response.Content.ReadAsStringAsync());
                _authHeader = new AuthenticationHeaderValue("Bearer", token.jwt);
            }
        }
    }

    public class Book
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public object[] Image { get; set; }
        public Book(string isbn, string title)
        {
            ISBN = isbn;
            Title = title;
        }
    }

    public class Login
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    public class ApiToken
    {
        public string jwt { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string provider { get; set; }
        public bool confirmed { get; set; }
        public bool blocked { get; set; }
        public Role role { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

}
