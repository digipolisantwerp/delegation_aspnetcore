using Digipolis.Delegation.Utilities;
using System.Security.Claims;

namespace Digipolis.Delegation
{
    public class DelegationUser : IDelegationUser
    {
        public DelegationUser()
        {
            IsValid = false;
        }

        /// <summary>
        /// name
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// surname
        /// </summary>
        public string SurName { get; private set; }

        /// <summary>
        /// givenname
        /// </summary>
        public string GivenName { get; private set; }

        /// <summary>
        /// sub
        /// </summary>
        public string Sub { get; private set; }

        /// <summary>
        /// rrnr
        /// </summary>
        public string RRNr { get; private set; }

        /// <summary>
        /// X-Credential-Username
        /// </summary>
        public string XCredentialUserName { get; private set; }
        
        /// <summary>
        /// profileId
        /// </summary>
        public string ProfileId { get; private set; }

        /// <summary>
        /// profileType
        /// </summary>
        public string ProfileType { get; private set; }

        /// <summary>
        /// The user's JWT token as a base64 encoded string.
        /// </summary>
        public string JwtToken { get; private set; }

        internal bool TrySetValues(bool isValid, string name, string givenName, string surName, string sub, string rrNr, string xCredentialUserName, string profileId, string profileType, string jwtToken)
        {
            IsValid = isValid;
            Sub = sub;
            RRNr = rrNr;
            XCredentialUserName = xCredentialUserName;
            Name = name;
            GivenName = givenName;
            SurName = surName;
            ProfileId = profileId;
            ProfileType = profileType;
            JwtToken = jwtToken;
            return true;
        }

        internal bool TrySetValues(ClaimsIdentity identity, string jwtToken)
        {
            if (identity != null)
            {
                Sub = identity.GetClaimValue(Claims.Sub);
                RRNr = identity.GetClaimValue(Claims.RRNr);
                XCredentialUserName = identity.GetClaimValue(Claims.XCredentialUserName);
                Name = identity.GetClaimValue(Claims.Name);
                GivenName = identity.GetClaimValue(Claims.GivenName);
                SurName = identity.GetClaimValue(Claims.SurName);                                
                ProfileId = identity.GetClaimValue(Claims.ProfileId);
                ProfileType = identity.GetClaimValue(Claims.ProfileType);
                JwtToken = jwtToken;

                IsValid = true;
                return true;
            }
            IsValid = false;
            return false;
        }

        internal void SetValid(bool isValid)
        {
            IsValid = isValid;
        }
    }
}
