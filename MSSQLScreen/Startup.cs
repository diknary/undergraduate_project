﻿using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(MSSQLScreen.Startup))]
namespace MSSQLScreen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //APIAuthorization
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var serverProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/request_token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = serverProvider
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            //WebAuthorization
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/user/login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
            });

            //SignalR for Chart
            app.MapSignalR();
        }
    }
}
