﻿using NUnit.Framework;
using Newtonsoft.Json;

namespace NuGetKeyVaultSignTool
{
    public class AzureKeyVaultConfigTests
    {
        [Test]
        public void JsonIsNotValidTest()
        {
            var content = """
            {
                "AzureKeyVaultCertificate": "",
                "AzureKeyVaultUrl": "",
                "AzureKeyVaultClientId": "",
                "AzureKeyVaultTenantId": ""
            }
            """;

            var azureKeyVaultFile = JsonConvert.DeserializeObject<AzureKeyVaultConfig>(content);
            Assert.IsNotNull(azureKeyVaultFile);
            Assert.IsFalse(azureKeyVaultFile.IsValid());
        }

        [Test]
        public void JsonIsValidTest()
        {
            var content = """
            {
                "AzureKeyVaultCertificate": "AzureKeyVaultCertificate",
                "AzureKeyVaultUrl": "AzureKeyVaultUrl",
                "AzureKeyVaultClientId": "AzureKeyVaultClientId",
                "AzureKeyVaultTenantId": "AzureKeyVaultTenantId"
            }
            """;

            var azureKeyVaultFile = JsonConvert.DeserializeObject<AzureKeyVaultConfig>(content);
            Assert.IsNotNull(azureKeyVaultFile);
            Assert.IsTrue(azureKeyVaultFile.IsValid());
            Assert.AreEqual("AzureKeyVaultCertificate", azureKeyVaultFile.AzureKeyVaultCertificate);
            Assert.AreEqual("AzureKeyVaultUrl", azureKeyVaultFile.AzureKeyVaultUrl);
            Assert.AreEqual("AzureKeyVaultClientId", azureKeyVaultFile.AzureKeyVaultClientId);
            Assert.AreEqual("AzureKeyVaultTenantId", azureKeyVaultFile.AzureKeyVaultTenantId);
        }
    }

}