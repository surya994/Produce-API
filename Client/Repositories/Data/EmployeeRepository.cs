using API.Models;
using Client.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeRepository(Address address, string request = "Employee/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext.Session.GetString("JWToken"));
        }
        public async Task<List<MasterDataVM>> GetMaster()
        {
            List<MasterDataVM> entities = new List<MasterDataVM>();
            using (var response = await httpClient.GetAsync(request + "master/"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entities = JsonConvert.DeserializeObject<List<MasterDataVM>>(apiResponse);
                }
            }
            return entities;
        }
        public async Task<List<MasterDataVM>> GetMaster(string id)
        {
            List<MasterDataVM> entity = new List<MasterDataVM>();
            using (var response = await httpClient.GetAsync(request + "master/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entity = JsonConvert.DeserializeObject<List<MasterDataVM>>(apiResponse);
                }
            }
            return entity;
        }
    }
}