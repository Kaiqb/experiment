namespace GitHubQl.Models.GitHub
{
    /// <summary>The association a comment author has with a repository.</summary>
    [QlEnum]
    public enum CommentAuthorAssociation
    {
        /// <summary>Author has been invited to collaborate on the repository.</summary>
        COLLABORATOR,

        /// <summary>Author has previously committed to the repository.</summary>
        CONTRIBUTOR,

        /// <summary>Author has not previously committed to GitHub.</summary>
        FIRST_TIMER,

        /// <summary>Author has not previously committed to the repository.</summary>
        FIRST_TIME_CONTRIBUTOR,

        /// <summary>Author is a member of the organization that owns the repository.</summary>
        MEMBER,

        /// <summary>Author has no association with the repository.</summary>
        NONE,

        /// <summary>Author is the owner of the repository.</summary>
        OWNER,
    }
}
