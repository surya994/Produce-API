using API.Models;
using Client.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountRepository(Address address, string request = "Account/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext.Session.GetString("JWToken"));
        }
        public HttpStatusCode Register(RegisterVM registerVM)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(registerVM), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(request + "register/", content).Result;
            return result.StatusCode;
        }
        public HttpStatusCode Update(UpdateMasterVM updateMasterVM)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(updateMasterVM), Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(request + "master/update/", content).Result;
            return result.StatusCode;
        }
        public async Task<Employee> GetByEmail(string email)
        {
            Employee entity = null;
            using (var response = await httpClient.GetAsync(request + "email/" + email))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return entity;
        }
        public async Task<Employee> GetByPhone(string phone)
        {
            Employee entity = null;
            using (var response = await httpClient.GetAsync(request + "phone/" + phone))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return entity;
        }
        public async Task<Employee> GetByEmail(string email, string nik)
        {
            Employee entity = null;
            using (var response = await httpClient.GetAsync(request + "email/" + email + "/" + nik))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return entity;
        }
        public async Task<Employee> GetByPhone(string phone, string nik)
        {
            Employee entity = null;
            using (var response = await httpClient.GetAsync(request + "phone/" + phone + "/" + nik))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return entity;
        }
    }
}