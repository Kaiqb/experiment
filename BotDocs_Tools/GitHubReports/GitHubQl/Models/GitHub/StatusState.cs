namespace GitHubQl.Models.GitHub
{
    /// <summary>The possible commit status states.</summary>
    public enum StatusState
    {
        /// <summary>Status is errored.</summary>
        ERROR,

        /// <summary>Status is expected.</summary>
        EXPECTED,

        /// <summary>Status is failing.</summary>
        FAILURE,

        /// <summary>Status is pending.</summary>
        PENDING,

        /// <summary>Status is successful.</summary>
        SUCCESS,
    }
}
