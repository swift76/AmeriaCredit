using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.CustomerModuleWebApp.Controllers.Authentication
{
    public class OnboardingController : Controller
    {
        [HttpGet("DataFrom/{id}")]
        public async Task<OnboardingData> GetDataFrom(string id)
        {
            OnboardingData result = null;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                HttpResponseMessage response = await client.GetAsync(string.Format("/api/customer/onboarding/DataFrom/{0}", id));
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OnboardingData>(content);
                }
            }
            catch
            {
            }
            return result;
        }

        [HttpGet("DataTo/{id}")]
        public async Task<ApplicationForOnboarding> GetDataTo(string id)
        {
            ApplicationForOnboarding result = null;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                HttpResponseMessage response = await client.GetAsync(string.Format("/api/customer/onboarding/DataTo/{0}", id));
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApplicationForOnboarding>(content);
                }
            }
            catch
            {
            }
            return result;
        }

        [HttpGet("RedirectionLink/{id}")]
        public async Task<string> GetRedirectionLink(Guid id)
        {
            string result = null;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                HttpResponseMessage response = await client.GetAsync(string.Format("/api/customer/onboarding/RedirectionLink/{0}", id));
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
