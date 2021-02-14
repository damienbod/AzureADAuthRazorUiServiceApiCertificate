using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

namespace WebApiWithRolesAclGraphAuthz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            IdentityModelEventSource.ShowPII = true;

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph("https://graph.microsoft.com/beta", "user.read Directory.Read.All User.ReadBasic.All")
                .AddInMemoryTokenCaches();

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.RoleClaimType = "roles";
                options.TokenValidationParameters.NameClaimType = "name";
            });

            services.AddScoped<GraphApiClientService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllSpaOrigin",
                    builder =>
                    {
                        builder
                            .AllowCredentials()
                            .WithOrigins(
                                "https://localhost:4200")
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web API with roles",
                    Version = "v1",
                    Description = "Web API with roles",
                    Contact = new OpenApiContact
                    {
                        Name = "damienbod",
                        Email = string.Empty,
                        Url = new Uri("https://damienbod.com/"),
                    },
                });
            });

            services.AddAuthorization(policies =>
            {
                //policies.AddPolicy("p-web-api-with-roles-user", p => 
                //{
                //    p.RequireClaim("roles","web-api-with-roles-user");
                //});
                //policies.AddPolicy("p-web-api-with-roles-student", p =>
                //{
                //    p.RequireClaim("roles", "web-api-with-roles-student");
                //});
                //policies.AddPolicy("p-web-api-with-roles-admin", p =>
                //{
                //    p.RequireClaim("roles", "web-api-with-roles-admin");
                //});

            });

            services.AddControllers(options =>
            {
                // global
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                   // .RequireClaim("email") // disabled this to test with users that have no email (no license added)
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API with roles");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("AllSpaOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
