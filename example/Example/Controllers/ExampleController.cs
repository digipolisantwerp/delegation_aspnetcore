using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Auth.Agents;
using Digipolis.Auth.Authorization.Attributes;
using Digipolis.Auth.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Example.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {

        private readonly ILogger<ExampleController> _logger;
        private readonly IMeAuthzAgent _meAuthzAgent;

        public ExampleController(ILogger<ExampleController> logger, IMeAuthzAgent meAuthzAgent)
        {
            _logger = logger;
            _meAuthzAgent = meAuthzAgent;
        }
        
        [HttpGet("[action]"), AllowAnonymous]
        public string GetJwtToken()
        {
            var request = WebRequest.Create(
                $"{Constants.MeAuthzUrl}/oauth2/authorize?client_id={Constants.ClientId}&client_secret={Constants.ClientSecret}&response_type=token&provision_key={Constants.ProvisionKey}&authenticated_userid={Constants.UserId}");
            request.Headers.Add("apikey", Constants.ApiKey);
            request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            request.Method = "POST";
            var response = request.GetResponse();

            using var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = JsonConvert.DeserializeObject<AccessToken>(reader.ReadToEnd());
            
            var accessToken = responseFromServer.Redirect_uri.Split("access_token=")[1]
                .Split('&')[0];

            var jwt = _meAuthzAgent.GetJwtToken(accessToken).Result;
            
            return jwt;
        }
        
        [AuthorizeWith(AuthenticationSchemes = AuthScheme.JwtHeaderAuth)]
        [HttpGet("[action]")]
        public bool GetProtected()
        {
            _logger.LogInformation("protected");
            return true;
        }
        
        private class AccessToken
        {
            public string Redirect_uri { get; set; }
        }
    }
}