﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security;
using Microsoft.WindowsAzure.Commands.Common.Models;
using Microsoft.WindowsAzure.Commands.Common.Properties;
using Microsoft.WindowsAzure.Commands.Utilities.Common.Authentication;

namespace Microsoft.WindowsAzure.Commands.Common.Factories
{
    public class AuthenticationFactory : IAuthenticationFactory
    {
        public const string CommonAdTenant = "Common";

        public AuthenticationFactory()
        {
            TokenProvider = new AdalTokenProvider();
        }

        public ITokenProvider TokenProvider { get; set; }

        public IAccessToken Authenticate(ref AzureAccount account, AzureEnvironment environment, string tenant, SecureString password,
            ShowDialog promptBehavior)
        {
            var token = TokenProvider.GetAccessToken(GetAdalConfiguration(environment, tenant), promptBehavior, account.Id, password, account.Type);
            account.Id = token.UserId;
            return token;
        }

        public SubscriptionCloudCredentials GetSubscriptionCloudCredentials(AzureContext context)
        {
            if (context.Subscription == null)
            {
                throw new ApplicationException(Resources.InvalidCurrentSubscription);
            }

            var account = context.Account;

            if (account == null)
            {
                throw new ArgumentException(Resources.InvalidSubscriptionState);
            }

            if (account.Type == AzureAccount.AccountType.Certificate)
            {
                var certificate = ProfileClient.DataStore.GetCertificate(account.Id);
                return new CertificateCloudCredentials(context.Subscription.Id.ToString(), certificate);
            }

            var tenants = context.Subscription.GetPropertyAsArray(AzureSubscription.Property.Tenants)
                  .Intersect(context.Account.GetPropertyAsArray(AzureAccount.Property.Tenants));

            IAccessToken token = null;
            foreach (var tenant in tenants)
            {
                try
                {
                    token = Authenticate(ref account, context.Environment, tenant, null, ShowDialog.Never);
                    break;
                }
                catch
                {
                    // Skip
                }
            }

            //try again
            if (token != null)
            {
                return new AccessTokenCredential(context.Subscription.Id, token);
            }
            else 
            {
                throw new ArgumentException(Resources.InvalidSubscriptionState);
            }
        }

        private AdalConfiguration GetAdalConfiguration(AzureEnvironment environment, string tenantId)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            var adEndpoint = environment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectory];
            var adResourceId = environment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectoryServiceEndpointResourceId];

            return new AdalConfiguration
            {
                AdEndpoint = adEndpoint,
                ResourceClientUri = adResourceId,
                AdDomain = tenantId
            };
        }
    }
}
