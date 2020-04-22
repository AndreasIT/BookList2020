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
            var login = new Login
            {
                Identifier = "mdam@ucl.dk",
                Password = "Student1234"
            };
            _client = new HttpClient(new ErrorHandler())
            {
                BaseAddress = new Uri("https://shrouded-waters-99136.herokuapp.com/")
            };
            var userToken = GetToken(login).Result;
            var authHeader = new AuthenticationHeaderValue("Bearer", userToken.jwt);
            _client.DefaultRequestHeaders.Authorization = authHeader;
        }

        private async Task<ApiToken> GetToken(Login login)
        {
            var loginString = JsonConvert.SerializeObject(login);
            var response = await _client.PostAsync("auth/local", new StringContent(loginString));
            return JsonConvert.DeserializeObject<ApiToken>(await response.Content.ReadAsStringAsync());

        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var response = await _client.GetAsync("books");
            return JsonConvert.DeserializeObject<IEnumerable<Book>>(await response.Content.ReadAsStringAsync());
        }

        public async Task CreateBook(Book book)
        {
            var request = JsonConvert.SerializeObject(book); 
            await _client.PostAsync("books", new StringContent(request));
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
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }

    public class Book
    {
            public int id { get; set; }
            public string Title { get; set; }
            public string ISBN { get; set; }
            public string Description { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public object[] Image { get; set; }
    }

    public class Login
    {
        public string Identifier { get; set; }
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
