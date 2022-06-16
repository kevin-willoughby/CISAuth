using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace CIS2Auth.Jwt
{

    public class JwtHandler
    {


        private readonly string _audience;

        private readonly string _clientId;

        private readonly string _kid;
        private readonly string _privateKey; 



        public JwtHandler( string audience, string clientId, string kid, string privateKey)      
        {
            _audience = audience; 
            _clientId = clientId; 
            _kid = kid; 
            _privateKey = privateKey;
        }

        public string generateJWT(int expInMinutes = 1)
        {

            var now = DateTime.UtcNow;

            var privateKey = Convert.FromBase64String( _privateKey);

            using RSA rsa = RSA.Create();
            
            rsa.ImportRSAPrivateKey(privateKey, out _);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha512)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };


            var claimList = new List<Claim>();
            claimList.Add(new Claim("jti", Guid.NewGuid().ToString()));
            claimList.Add(new Claim(JwtClaimTypes.Subject, _clientId));


            var token = new JwtSecurityToken(_clientId,
                _audience,
                claimList,
                now,
                now.AddMinutes(expInMinutes),
                signingCredentials
            );

            token.Payload.Remove("nbf");
            token.Header.Add("kid", _kid);
            token.Header.Remove("cty");
            

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }

    }
}
