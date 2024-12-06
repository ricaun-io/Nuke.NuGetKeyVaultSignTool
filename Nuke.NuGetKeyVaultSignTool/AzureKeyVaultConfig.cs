namespace Nuke.NuGetKeyVaultSignTool
{
    /// <summary>
    /// Represents the configuration for Azure Key Vault.
    /// </summary>
    public class AzureKeyVaultConfig
    {
        /// <summary>
        /// Gets or sets the Azure Key Vault certificate.
        /// </summary>
        public string AzureKeyVaultCertificate { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault URL.
        /// </summary>
        public string AzureKeyVaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault client ID.
        /// </summary>
        public string AzureKeyVaultClientId { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault tenant ID.
        /// </summary>
        public string AzureKeyVaultTenantId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp URL.
        /// </summary>
        public string TimestampUrl { get; set; }

        /// <summary>
        /// Gets or sets the timestamp digest.
        /// </summary>
        public string TimestampDigest { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="AzureKeyVaultConfig"/> from the specified JSON content.
        /// </summary>
        /// <param name="jsonContent">The JSON content representing the Azure Key Vault configuration.</param>
        /// <returns>An instance of <see cref="AzureKeyVaultConfig"/>.</returns>
        public static AzureKeyVaultConfig Create(string jsonContent)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<AzureKeyVaultConfig>(jsonContent);
            }
            catch { }
            return default;
        }

        /// <summary>
        /// Checks if the Azure Key Vault configuration is valid.
        /// </summary>
        /// <returns><c>true</c> if the configuration is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AzureKeyVaultCertificate) &&
                   !string.IsNullOrEmpty(AzureKeyVaultUrl) &&
                   !string.IsNullOrEmpty(AzureKeyVaultClientId) &&
                   !string.IsNullOrEmpty(AzureKeyVaultTenantId);
        }
    }

}