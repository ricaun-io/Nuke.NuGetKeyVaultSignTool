namespace NuGetKeyVaultSignTool
{
    public class AzureKeyVaultConfig
    {
        public string AzureKeyVaultCertificate { get; set; }
        public string AzureKeyVaultUrl { get; set; }
        public string AzureKeyVaultClientId { get; set; }
        public string AzureKeyVaultTenantId { get; set; }
        public string TimestampUrl { get; set; }
        public string TimestampDigest { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AzureKeyVaultCertificate) &&
                   !string.IsNullOrEmpty(AzureKeyVaultUrl) &&
                   !string.IsNullOrEmpty(AzureKeyVaultClientId) &&
                   !string.IsNullOrEmpty(AzureKeyVaultTenantId);
        }
    }

}