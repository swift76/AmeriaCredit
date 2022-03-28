using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;
using Newtonsoft.Json;
using IntelART.Ameria.Entities;
using IntelART.Utilities;
using IntelART.Ameria.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IntelART.Ameria.CustomerModuleWebApp.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private string identityServerUrl;

        public AccountController(IConfigurationRoot configuration)
        {
            this.identityServerUrl = configuration.GetSection("Authentication")["Authority"];
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            AuthenticationViewModel model = new AuthenticationViewModel();
            model.LoginModel.ReturnUrl = returnUrl;
            return Ok(true);
        }

        [HttpGet]
        public async Task<IActionResult> RequestPasswordReset()
        {
            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> RequestPasswordReset([FromForm] RequestPasswordResetModel model)
        {
            bool hasError = !ModelState.IsValid;
            PasswordResetResponse result = new PasswordResetResponse();
            Guid processId = Guid.NewGuid();
            try
            {
                if (!hasError)
                {
                    PasswordResetModel newModel = new PasswordResetModel();
                    newModel.RegistrationProcessId = processId;
                    newModel.Phone = model.Phone;

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                    HttpResponseMessage response = await client.PutAsync(string.Format("/api/customer/Account/{0}/PasswordManagerProcess/{1}", model.Phone, newModel.RegistrationProcessId), null);
                    if (!response.IsSuccessStatusCode)
                    {
                        hasError = true;
                        bool isUnknownError = true;
                        if (response.Content != null)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                            if (exception != null)
                            {
                                isUnknownError = false;
                                ModelState.AddModelError("", exception.Message);
                            }
                        }
                        if (isUnknownError)
                        {
                            ModelState.AddModelError("", "Համակարգային սխալ գաղտնաբառի փոփոխման ժամանակ");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                hasError = true;
                ModelState.AddModelError("", e.Message);
            }
            if (hasError)
            {
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        result.ErrorMessages.Add(error.ErrorMessage);
                    }
                }
            }
            else
            {
                result.RegistrationProcessID = processId;
                result.Phone = model.Phone;
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset([FromForm] PasswordResetModel model)
        {
            bool hasError = !ModelState.IsValid;
            try
            {
                if (!hasError)
                {
                    CustomerUserPasswordResetData data = new CustomerUserPasswordResetData();
                    data.SmsCode = model.VerificationCode;
                    data.NewPassword = model.Password;
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));

                    // password validation
                    if (!(await CheckRestrictedPasswords(client, model.Password)))
                    {
                        hasError = true;
                        ModelState.AddModelError("", "Անթույլատրելի գաղտնաբառ");
                    }
                    else
                    {
                        HttpResponseMessage response = await client.PostAsync(string.Format("/api/customer/Account/{0}/PasswordManagerProcess/{1}", model.Phone, model.RegistrationProcessId), new StringContent(JsonConvert.SerializeObject(data, Formatting.None), Encoding.UTF8));
                        if (!response.IsSuccessStatusCode)
                        {
                            hasError = true;
                            bool isUnknownError = true;
                            if (response.Content != null)
                            {
                                string content = await response.Content.ReadAsStringAsync();
                                ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                                if (exception != null)
                                {
                                    isUnknownError = false;
                                    ModelState.AddModelError("", exception.Message);
                                }
                            }
                            if (isUnknownError)
                            {
                                ModelState.AddModelError("", "Համակարգային սխալ գաղտնաբառի փոփոխման ժամանակ");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                hasError = true;
                ModelState.AddModelError("", e.Message);
            }

            PasswordResetResponse result = new PasswordResetResponse();
            if (hasError)
            {
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        result.ErrorMessages.Add(error.ErrorMessage);
                    }
                }
            }

            return Ok(result);
        }


        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var disco = await DiscoveryClient.GetAsync(this.identityServerUrl);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var client = new TokenClient(
                disco.TokenEndpoint,
                "customerApplication",
                "secret");

            string username = model.Username;
            string password = model.Password;

            TokenResponse tokenResponse = await client.RequestResourceOwnerPasswordAsync(username, password, "openid profile customerApi loanApplicationApi offline_access");

            if (!tokenResponse.IsError
                && tokenResponse.AccessToken != null)
            {
                JwtSecurityToken token = new JwtSecurityToken(tokenResponse.AccessToken);
                AuthenticationProperties props = new AuthenticationProperties();
                props.Items[".Token.access_token"] = tokenResponse.AccessToken;
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme)),
                    props);

                return Ok(tokenResponse.Json);
            }
            else
            {
                return BadRequest(tokenResponse.Json);
            }
        }

        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            Guid? processId;
            if (model.ONBOARDING_ID.HasValue)
                processId = null;
            else
                processId = Guid.NewGuid();

            string phone;
            bool hasError = !ModelState.IsValid;
            RegisterResponse result = new RegisterResponse();
            try
            {
                if (!hasError)
                {
                    phone = model.Phone.Replace(" ", "");

                    if (!model.AcceptedTermsAndConditions)
                    {
                        result.ErrorCode = "ERR-0034";
                        throw new ApplicationException("Պայմաններին և կանոններին Ձեր համաձայնությունը պարտադիր է։");
                    }
                    else
                    {
                        // password validation
                        ValidationManager.ValidateCustomerPasswordCreation(model.Phone,
                                                                           model.Password,
                                                                           model.ConfirmPassword);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                        if (!(await CheckRestrictedPasswords(client, model.Password)))
                        {
                            hasError = true;
                            ModelState.AddModelError("", "Անթույլատրելի գաղտնաբառ");
                        }
                        else
                        {
                            CustomerUserRegistrationPreVerification user = new CustomerUserRegistrationPreVerification();
                            user.FIRST_NAME_EN = model.Name;
                            user.LAST_NAME_EN = model.Lastname;
                            user.MOBILE_PHONE = phone;
                            user.EMAIL = model.Email;
                            user.HASH = Crypto.HashString(model.Password);
                            user.SOCIAL_CARD_NUMBER = model.SSN;
                            user.PROCESS_ID = processId;
                            user.ONBOARDING_ID = model.ONBOARDING_ID;
                            user.IS_STUDENT = model.IS_STUDENT;

                            HttpResponseMessage response = await client.PostAsync("/api/customer/Account", new StringContent(JsonConvert.SerializeObject(user, Formatting.None), Encoding.UTF8));
                            if (!response.IsSuccessStatusCode)
                            {
                                hasError = true;
                                bool isUnknownError = true;
                                if (response.Content != null)
                                {
                                    string content = await response.Content.ReadAsStringAsync();
                                    ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                                    if (exception != null)
                                    {
                                        isUnknownError = false;
                                        ModelState.AddModelError("", exception.Message);
                                    }
                                }
                                if (isUnknownError)
                                {
                                    ModelState.AddModelError("", "Համակարգային սխալ գրանցման ժամանակ");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                hasError = true;
                ModelState.AddModelError("", e.Message);
            }

            if (hasError)
            {
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        result.ErrorMessages.Add(error.ErrorMessage);
                    }
                }
            }
            else
            {
                result.RegistrationProcessID = processId;
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhone([FromForm] VerificationModel model)
        {
            bool hasError = !ModelState.IsValid;
            try
            {
                if (!hasError)
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
                    HttpResponseMessage response = await client.PutAsync(string.Format("/api/customer/Account/{0}/Verification/{1}", model.RegistrationProcessId, model.VerificationCode), null);
                    if (!response.IsSuccessStatusCode)
                    {
                        hasError = true;
                        bool isUnknownError = true;
                        if (response.Content != null)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                            if (exception != null)
                            {
                                isUnknownError = false;
                                ModelState.AddModelError("", exception.Message);
                            }
                        }
                        if (isUnknownError)
                        {
                            ModelState.AddModelError("", "Համակարգային սխալ գրանցման ժամանակ");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                hasError = true;
                ModelState.AddModelError("", e.Message);
            }

            PasswordResetResponse result = new PasswordResetResponse();
            if (hasError)
            {
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        result.ErrorMessages.Add(error.ErrorMessage);
                    }
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerificationCode([FromQuery] Guid processID)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));
            HttpResponseMessage response = await client.PostAsync(string.Format("/api/customer/Account/{0}/", processID), null);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                    throw new Exception(exception.Message);
                }
                throw new Exception();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(true);
        }

        private async Task<bool> CheckRestrictedPasswords(HttpClient client, string password)
        {
            HttpResponseMessage responsePasswords = await client.GetAsync("/api/customer/Account/restrictedPasswords");
            string responsePasswordsContent = await responsePasswords.Content.ReadAsStringAsync();
            List<string> restrictedPasswords = JsonConvert.DeserializeObject<List<string>>(responsePasswordsContent);
            if (restrictedPasswords != null)
            {
                foreach (string restrictedPassword in restrictedPasswords)
                {
                    if (password.ToLower().Contains(restrictedPassword))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class PasswordResetResponse
    {
        public Guid? RegistrationProcessID { get; set; }
        public string Phone { get; set; }
        public List<string> ErrorMessages { get; set; }

        public PasswordResetResponse()
        {
            RegistrationProcessID = null;
            Phone = null;
            ErrorMessages = new List<string>();
        }
    }

    public class RegisterResponse
    {
        public Guid? RegistrationProcessID { get; set; }
        public string ErrorCode { get; set; }
        public List<string> ErrorMessages { get; set; }

        public RegisterResponse()
        {
            RegistrationProcessID = null;
            ErrorCode = null;
            ErrorMessages = new List<string>();
        }
    }
}
