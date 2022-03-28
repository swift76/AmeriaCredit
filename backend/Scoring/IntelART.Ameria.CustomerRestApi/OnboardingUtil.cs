using System;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace IntelART.Ameria.CustomerRestApi
{
    public class OnboardingUtil
    {
        private const string EncryptionPassword = "3CD6F8DB90C04733937E195146C2A09E";
        private const string EncryptionSalt = "FE44570D2EBB4FD8BB8B68DD7E32CB36";

        public string EncryptData(string source)
        {
            string result = null;
            RijndaelManaged algorithm = null;
            try
            {
                algorithm = new RijndaelManaged();
                algorithm.Key = (new Rfc2898DeriveBytes(EncryptionPassword, Encoding.UTF8.GetBytes(EncryptionSalt))).GetBytes(algorithm.KeySize / 8);
                ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(algorithm.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(algorithm.IV, 0, algorithm.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        swEncrypt.Write(source);
                    result = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (algorithm != null)
                    algorithm.Clear();
            }
            return result;
        }

        public string DecryptData(string source)
        {
            string result = null;
            RijndaelManaged algorithm = null;
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    algorithm = new RijndaelManaged();
                    algorithm.Key = (new Rfc2898DeriveBytes(EncryptionPassword, Encoding.UTF8.GetBytes(EncryptionSalt))).GetBytes(algorithm.KeySize / 8);
                    byte[] rawLength = new byte[sizeof(int)];
                    msDecrypt.Read(rawLength, 0, rawLength.Length);
                    byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
                    msDecrypt.Read(buffer, 0, buffer.Length);
                    algorithm.IV = buffer;
                    ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        result = srDecrypt.ReadToEnd();
                }
            }
            finally
            {
                if (algorithm != null)
                    algorithm.Clear();
            }
            return result;
        }

        public OnboardingData GetOnboardingData(string encryptedID, CustomerUserRepository repository)
        {
            Guid id;
            if (!Guid.TryParse(encryptedID, out id))
                id = new Guid(DecryptData(Base64UrlEncoder.Decode(encryptedID)));

            OnboardingData result = repository.GetOnboardingCustomer(id);
            if (result == null)
            {
                string url = string.Format("{0}{1}", repository.GetServiceConfiguration("ONBG").URL, encryptedID);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ContentType = "application/json";
                webRequest.Method = "GET";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = webRequest.Timeout;
                WebResponse webResponse = webRequest.GetResponse();
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = JsonConvert.DeserializeObject<OnboardingData>(reader.ReadToEnd());
                }
                if (result != null)
                {
                    if (!string.IsNullOrWhiteSpace(result.mobile_number) && result.mobile_number.StartsWith("374") && result.mobile_number.Length > 3)
                        result.mobile_number = result.mobile_number.Substring(3);
                    result.id = id;
                    repository.SaveOnboardingCustomer(result);
                }
            }
            return result;
        }

        public ApplicationForOnboarding GetApplicationForOnboarding(string encryptedID, CustomerUserRepository repository)
        {
            Guid id;
            if (!Guid.TryParse(encryptedID, out id))
                id = new Guid(DecryptData(Base64UrlEncoder.Decode(encryptedID)));
            return repository.GetApplicationForOnboarding(id);
        }

        public string GetRedirectionLink(Guid id, CustomerUserRepository repository)
        {
            ServiceConfiguration configuration = repository.GetServiceConfiguration("ONBA");
            OnboardingAuthorizationRequest requestEntity = new OnboardingAuthorizationRequest()
            {
                partnerId = configuration.USER_NAME,
                partnerKey = configuration.USER_PASSWORD
            };
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(configuration.URL);
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            webRequest.Timeout = 180000;
            webRequest.ReadWriteTimeout = webRequest.Timeout;
            using (StreamWriter writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(JsonConvert.SerializeObject(requestEntity));
            }
            WebResponse webResponse = webRequest.GetResponse();
            string token;
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                token = reader.ReadToEnd();
            }

            ApplicationForOnboarding application = repository.GetApplicationForOnboarding(id);
            OnboardingIntegrationRequest integrationEntity = new OnboardingIntegrationRequest()
            {
                id = id,
                first_name_eng = application.FIRST_NAME_EN,
                last_name_eng = application.LAST_NAME_EN,
                email = application.EMAIL,
                mobile_number = application.PHONE,
                birth_date = application.BIRTH_DATE,
                document_type_id = byte.Parse(application.DOCUMENT_TYPE_CODE),
                document_number = application.DOCUMENT_NUMBER,
                soccard_number = application.SOCIAL_CARD_NUMBER,
                is_student = application.IS_STUDENT,                                
                back_url = repository.GetSetting("CALL_BACK_URL")
            };
            configuration = repository.GetServiceConfiguration("ONBP");
            webRequest = (HttpWebRequest)WebRequest.Create(configuration.URL);
            webRequest.Headers.Add("Authorization", $"Bearer {token}");
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            webRequest.Timeout = 180000;
            webRequest.ReadWriteTimeout = webRequest.Timeout;
            using (StreamWriter writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(JsonConvert.SerializeObject(integrationEntity));
            }
            webResponse = webRequest.GetResponse();
            string url;
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                url = reader.ReadToEnd();
            }
            return url;
        }
    }
}
