namespace Digipolis.Delegation
{
    public interface IDelegationUser
    {
        /// <summary>
        /// is a valid delegation user detected
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// surname
        /// </summary>
        string SurName { get; }

        /// <summary>
        /// givenname
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// sub
        /// </summary>
        string Sub { get; }

        /// <summary>
        /// rrnr
        /// </summary>
        string RRNr { get; }

        /// <summary>
        /// X-Credential-Username
        /// </summary>
        string XCredentialUserName { get; }

        /// <summary>
        /// profileId
        /// </summary>
        string ProfileId { get; }

        /// <summary>
        /// profileType
        /// </summary>
        string ProfileType { get; }
    }
}
