namespace Digipolis.Delegation
{
    public interface IDelegationUser
    {
        /// <summary>
        /// Is a valid delegation user detected.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// The user's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The user's surname.
        /// </summary>
        string SurName { get; }

        /// <summary>
        /// The user's given name.
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// The subject of the JWT token.
        /// </summary>
        string Sub { get; }

        /// <summary>
        /// The user's RRNr.
        /// </summary>
        string RRNr { get; }

        /// <summary>
        /// X-Credential-Username.
        /// </summary>
        string XCredentialUserName { get; }

        /// <summary>
        /// The user's profile Id.
        /// </summary>
        string ProfileId { get; }

        /// <summary>
        /// The user's profile type.
        /// </summary>
        string ProfileType { get; }

        /// <summary>
        /// The JWT token in as a base64 encoded string.
        /// </summary>
        string JwtToken { get; }
    }
}
