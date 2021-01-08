using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Text.Json;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using ERacuni.Shared.Convertors;
using ERacuni.Shared.Validation;
using ERacuni.Shared.Models;
using FluentValidation;

namespace ERacuni.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddApiAuthorization()
                .AddAccountClaimsPrincipalFactory<RolesFactory>();


            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            builder.Services.AddSingleton(servis =>
            {
                string URI = servis.GetRequiredService<NavigationManager>().BaseUri;

                var Klijent = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

                var Kanal = GrpcChannel.ForAddress(URI, new GrpcChannelOptions { HttpClient = Klijent });

                return new ERacuniProtoNameSpace.ERacuniProtoServis.ERacuniProtoServisClient(Kanal);

            });

            builder.Services.AddSingleton(servis =>
            {
                string URI = servis.GetRequiredService<NavigationManager>().BaseUri;

                var Klijent = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

                var Kanal = GrpcChannel.ForAddress(URI, new GrpcChannelOptions { HttpClient = Klijent });

                return new ERacuniProtoNameSpace.BillProtoServis.BillProtoServisClient(Kanal);

            });
            builder.Services.AddTransient<ConvertGRPC>();
            builder.Services.AddTransient<IValidator<Bill>, ValidationBill>();
            builder.Services.AddTransient<IValidator<Product>, ValidationProduct>();


            await builder.Build().RunAsync();
        }
    }

    public class RolesFactory
 : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public RolesFactory(IAccessTokenProviderAccessor accessor)
            : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            if (user.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user.Identity;
                var roleClaims = identity.FindAll(identity.RoleClaimType);

                if (roleClaims != null && roleClaims.Any())
                {
                    foreach (var existingClaim in roleClaims)
                    {
                        identity.RemoveClaim(existingClaim);
                    }

                    var rolesElem = account.AdditionalProperties[identity.RoleClaimType];

                    if (rolesElem is JsonElement roles)
                    {
                        if (roles.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var role in roles.EnumerateArray())
                            {
                                identity.AddClaim(new Claim(options.RoleClaim, role.GetString()));
                            }
                        }
                        else
                        {
                            identity.AddClaim(new Claim(options.RoleClaim, roles.GetString()));
                        }
                    }
                }
            }

            return user;
        }
    }
}




