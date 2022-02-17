using System;

namespace kpi.personal.api.functions.Model.Events
{
    public class BaseEventArgs
    {
        /// <summary>
        /// Issuer (aplicação emissora do evento)
        /// </summary>
        public String Iss { get; set; }

        /// <summary>
        /// Subject (tipo do evento)
        /// </summary>
        public String Sub { get; set; }

        /// <summary>
        /// Event id
        /// </summary>
        public Guid Jti { get; set; }

        /// <summary>
        /// Event date as a UTC unix timestamp
        /// </summary>
        public long Iat { get; set; }
    }
}
