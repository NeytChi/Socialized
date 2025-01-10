namespace UseCases.InstagramApi
{
    public enum InstagramLoginState
    {
        Success,
        ChallengeRequired,
        TwoFactorRequired,
        InactiveUser,
        InvalidUser,
        BadPassword,
        LimitError,
        Exception
    }
}
