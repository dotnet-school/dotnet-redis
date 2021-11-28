using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using StackExchange.Redis;

namespace ConsoleApp
{
    public class RedisClient
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisClient()
        {
            // Configuration for Redis
            var redisConfig = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379"},
                Ssl = false,
            };

            redisConfig.CertificateSelection += SelectCertifcate;
            redisConfig.CertificateValidation += ValidateRemoteCertificate;
            // Create connection with Redis
            _connection =  ConnectionMultiplexer.Connect(redisConfig);
        }

        private bool ValidateRemoteCertificate(
            object sender, 
            X509Certificate? certificate, 
            X509Chain? chain, 
            SslPolicyErrors sslpolicyerrors)
        {
            // Check if remote Redis server has valid certificate
            return true;
        }

        private X509Certificate SelectCertifcate(
            object sender, 
            string targethost, 
            X509CertificateCollection localcertificates, 
            X509Certificate? remotecertificate, 
            string[] acceptableissuers)
        {
            return new X509Certificate("my/cert/path", "my_secret");
        }
    }
}