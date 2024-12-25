using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME"); ;
//string kvUri = $"https://{keyVaultName}.vault.azure.net";

//string kvUri = "https://okbeshopkeyvault.vault.azure.net";
//SecretClient client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
//KeyVaultSecret secret = await client.GetSecretAsync("CosmosDbConnectionString");
//var connectionString = secret.Value;
//Console.WriteLine(connectionString);


var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) => 
    {
        //var settings = config.Build();

        //var config1 = context.Configuration;
        //var vaultUri = config1["VaultUri"];

        //var configsection = config1.GetSection("Values");
        //var someValue = configsection["VaultUri"];

        //// Get Key Vault URL from local.settings.json
        //var keyVaultUrl = settings["VaultUri"];

        //if (!string.IsNullOrEmpty(keyVaultUrl))
        //{
        //}

        // Configure Key Vault with DefaultAzureCredential
        config.AddAzureKeyVault(new Uri("https://okbeshopkeyvault.vault.azure.net"), new DefaultAzureCredential());

    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        var connectionString = config["cosmosconnection"];

        // Register CosmosDB
        services.AddSingleton(s =>
        {
            return new CosmosClient(connectionString);
        });
    })
    .Build();

host.Run();
