namespace Products.Catalog.Infra.Authentication
{
    /// <summary>
    /// A model to provide authentication information.
    /// </summary>
    public struct AuthenticationModel
    {
        /// <summary>
        /// A user to login.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A password to login.
        /// </summary>
        public string Password { get; set; }
    }
}