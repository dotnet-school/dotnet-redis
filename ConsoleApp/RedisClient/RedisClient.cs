using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using StackExchange.Redis;

namespace ConsoleApp
{
    public class RedisClient
    {
        private readonly Lazy<IConnectionMultiplexer> _lazyConnection;
        private readonly ConfigurationOptions _redisConfig;

        public IConnectionMultiplexer Connection => _lazyConnection.Value;

        public RedisClient()
        {
            // Configuration for Redis (these should come from env variables)
            _redisConfig = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379"},
                Ssl = false,
            };
            _redisConfig.CertificateSelection += SelectCertifcate;
            _redisConfig.CertificateValidation += ValidateRemoteCertificate;
            _lazyConnection = new Lazy<IConnectionMultiplexer>(CreateConnection);
        }

        private IConnectionMultiplexer CreateConnection()
        {
            // Create connection with Redis
            var connection =  ConnectionMultiplexer.Connect(_redisConfig);
            connection.ConnectionRestored += OnReconnect;
            connection.ConnectionFailed += OnConnectionFailed;
            return connection;
        }

        private void OnConnectionFailed(
            object? sender, 
            ConnectionFailedEventArgs e)
        {
            // Log that connection has failed
        }

        private void OnReconnect(
            object? sender, 
            ConnectionFailedEventArgs e)
        {
            // Log that redis was reconnected
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