using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IntelART.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("customerApi", "Customer-specific API"),
                new ApiResource("loanApplicationApi", "API for creating and submitting loan appications"),
                new ApiResource("bankInternalApi", "Internal API to be used by the bank employees and systems"),
            };
        }

        public static IEnumerable<Client> GetClients(IConfigurationSection config)
        {
            string customerApplicationUrl = config.GetSection("CustomerApplication")["Url"];
            string bankApplicationUrl = config.GetSection("BankApplication")["Url"];
            string shopApplicationUrl = config.GetSection("ShopApplication")["Url"];
            IConfigurationSection extenralClients = config.GetSection("ExternalClients");
            string oidcEndpoint = "/signin-oidc";
            string customerOidcEndpoint = string.Format("{0}{1}", customerApplicationUrl, oidcEndpoint);
            string bankOidcEndpoint = string.Format("{0}{1}", bankApplicationUrl, oidcEndpoint);
            string shopOidcEndpoint = string.Format("{0}{1}", shopApplicationUrl, oidcEndpoint);
            List<Client> clients = new List<Client>();
            clients.Add(new Client
            {
                ClientId = "customerApplication",
                ClientName = "Online application to be used by the customer",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowAccessTokensViaBrowser = true,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = { customerOidcEndpoint },
                PostLogoutRedirectUris = { customerOidcEndpoint },
                AllowedCorsOrigins = { customerApplicationUrl },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "loanApplicationApi",
                    "customerApi",
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
            });

            clients.Add(new Client
            {
                ClientId = "customerApplication2",
                ClientName = "Online application to be used by the customer",
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                AllowAccessTokensViaBrowser = true,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = { customerOidcEndpoint },
                PostLogoutRedirectUris = { customerOidcEndpoint },
                AllowedCorsOrigins = { customerApplicationUrl },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "loanApplicationApi",
                    "customerApi",
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
            });

            clients.Add(new Client
            {
                ClientId = "shopApplication",
                ClientName = "Online application to be used at shops",
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                AllowAccessTokensViaBrowser = true,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = { shopOidcEndpoint },
                PostLogoutRedirectUris = { shopOidcEndpoint },
                AllowedCorsOrigins = { shopApplicationUrl },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "loanApplicationApi",
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
            });

            clients.Add(new Client
            {
                ClientId = "shopApplication2",
                ClientName = "Online application to be used at shops",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowAccessTokensViaBrowser = true,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = { shopOidcEndpoint },
                PostLogoutRedirectUris = { shopOidcEndpoint },
                AllowedCorsOrigins = { shopApplicationUrl },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "loanApplicationApi",
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
            });

            clients.Add(new Client
            {
                ClientId = "bankApplication",
                ClientName = "Online application to be used at bank",
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ////AllowAccessTokensViaBrowser = true,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = { bankOidcEndpoint },
                PostLogoutRedirectUris = { bankOidcEndpoint },
                AllowedCorsOrigins = { bankApplicationUrl },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "bankInternalApi",
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
            });

            if (extenralClients != null)
            {
                IEnumerable<IConfigurationSection> children = extenralClients.GetChildren();
                if (children != null)
                {
                    foreach(IConfigurationSection clientConfiguration in children)
                    {
                        string clientId = clientConfiguration["ClientId"];
                        string clientName = clientConfiguration["ClientName"] ?? "";
                        IEnumerable<string> secrets = null;
                        IConfigurationSection secretsConfiguration = clientConfiguration.GetSection("Secrets");
                        if (secretsConfiguration != null)
                        {
                            secrets = secretsConfiguration.GetChildren().Select(item => item.Value);
                        }
                        if (clientId != null
                            && secrets != null
                            && !clients.Any(c => c.ClientId == clientId))
                        {
                            clients.Add(GetClient(
                                clientId,
                                clientName,
                                secrets,
                                shopOidcEndpoint,
                                shopApplicationUrl));
                        }
                    }
                }
            }

            if (!clients.Any(c => c.ClientId == "vega"))
            {
                clients.Add(GetClient(
                    "vega",
                    "Vega API",
                    new[] { "9475df51-0196-4946-a898-85289348a17e174c39f7-4e55-4f72-b292-779aa96d006f" },
                    shopOidcEndpoint,
                    shopApplicationUrl));
            }

            if (!clients.Any(c => c.ClientId == "ucom"))
            {
                clients.Add(GetClient(
                    "ucom",
                    "UCom API",
                    new[] { "fc3f2ebb-e0b4-4839-be38-28c9ea376af682f93bbd-a8bf-466c-8f7c-86ccc9d41a28" },
                    shopOidcEndpoint,
                    shopApplicationUrl));
            }

            if (!clients.Any(c => c.ClientId == "vivacell"))
            {
                clients.Add(GetClient(
                    "vivacell",
                    "Vivacell API",
                    new[] { "116ff603-ecdf-4b78-ab88-f07c085f926ad793fb89-1c75-49d1-bd05-478c0bf85da8" },
                    shopOidcEndpoint,
                    shopApplicationUrl));
            }

            if (!clients.Any(c => c.ClientId == "mobilecenter"))
            {
                clients.Add(GetClient(
                    "mobilecenter",
                    "Mobile center API",
                    new[] { "63189d66-2bdf-43eb-b57b-2a3fc09db05e8f44d300-2dda-4ba1-8b1a-bac47728a61d" },
                    shopOidcEndpoint,
                    shopApplicationUrl));
            }

            return clients;
        }

        private static Client GetClient(
            string clientId, 
            string clientName, 
            IEnumerable<string> secrets,
            string shopOidcEndpoint,
            string shopApplicationUrl)
        {
            return new Client
            {
                ClientId = clientId,
                ClientName = clientName,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowAccessTokensViaBrowser = true,

                ClientSecrets = new List<Secret>(secrets.Select(s => new Secret(s.Sha256()))),

                RedirectUris = { shopOidcEndpoint },
                PostLogoutRedirectUris = { shopOidcEndpoint },
                AllowedCorsOrigins = { shopApplicationUrl },

                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "loanApplicationApi",
                    },

                RequireConsent = false,
                AllowOfflineAccess = true,
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                };
        }

        public static void SetupIdentityServer(IdentityServerOptions options)
        {
            options.UserInteraction.LoginUrl = "/Authentication/Login";
            options.UserInteraction.LogoutUrl = "/Authentication/Logout";
        }
    }
}
