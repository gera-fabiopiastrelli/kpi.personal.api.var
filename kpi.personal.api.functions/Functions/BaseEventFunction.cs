using System;
using System.Security.Cryptography;
using System.Text;

using kpi.personal.api.functions.Model.Events;

namespace kpi.personal.api.functions.Functions
{
    public abstract class BaseEventFunction
    {
        /// <summary>
        /// Hash secret
        /// </summary>
        private static readonly String Secret = Environment.GetEnvironmentVariable("EventsSecret");
            
        /// <summary>
        /// Validade the Json Web Token signature and get its payload segment
        /// </summary>
        protected string ValidateJWT(string token)
        {
            // Verifies jwt content
            if (string.IsNullOrEmpty(token))
            {
                throw new ApplicationException("EmptyJWTContent");
            }

            // Gets jwt segments
            string[] segments = token.Split('.');
            if (segments.Length != 3)
            {
                throw new ApplicationException("InvalidJWTContent");
            }

            // Verifies jwt signature
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(segments[0] + "." + segments[1]));
                if (Base64UrlEncode(hash) != segments[2])
                {
                    throw new ApplicationException("InvalidJWTSignature");
                }
            }

            // Returns jwt payload
            return Encoding.UTF8.GetString(Base64UrlDecode(segments[1]));
        }

        #region Private methods

        /// <summary>
        /// From JWT spec
        /// </summary>
        private string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        /// from JWT spec
        /// </summary>
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new ApplicationException("Illegal base64url string");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        #endregion
    }
}