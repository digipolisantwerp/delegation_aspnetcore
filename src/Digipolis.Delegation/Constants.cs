namespace Digipolis.Delegation
{   
    public static class AuthSchemes
    {
        public const string CookieAuth = "CookieAuth";
        public const string JwtHeaderAuth = "JwtHeaderAuth";
    }
        
    public static class Claims
    {
        public const string PermissionsType = "permissions";
        public const string Sub = "sub";
        public const string RRNr = "rrnr";
        public const string Name = "name";
        public const string GivenName = "givenname";
        public const string SurName = "surname";
        public const string ProfileId = "profileId";
        public const string ProfileType = "profileType";
        public const string XCredentialUserName = "X-Credential-Username";
    };

    internal static class HeaderKeys
    {
        public const string Apikey = "apikey";
        public const string Delegation = "dgp-authorization-for";
    }
    
}
