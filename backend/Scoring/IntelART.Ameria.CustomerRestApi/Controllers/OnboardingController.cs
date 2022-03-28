using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.CustomerRestApi.Controllers
{
    [Route("[controller]")]
    public class OnboardingController : Controller
    {
        private CustomerUserRepository repository;
        private OnboardingUtil onboardingUtil;

        public OnboardingController(IConfigurationRoot Configuration)
        {
            string connectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.repository = new CustomerUserRepository(connectionString);
            this.onboardingUtil = new OnboardingUtil();
        }

        [HttpGet("DataFrom/{id}")]
        public OnboardingData GetDataFrom(string id)
        {
            return onboardingUtil.GetOnboardingData(id, repository);
        }

        [HttpGet("DataTo/{id}")]
        public ApplicationForOnboarding GetDataTo(string id)
        {
            return onboardingUtil.GetApplicationForOnboarding(id, repository);
        }

        [HttpGet("RedirectionLink/{id}")]
        public string GetRedirectionLink(Guid id)
        {
            return onboardingUtil.GetRedirectionLink(id, repository);
        }
    }
}
