﻿using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using SmtpServer;
using SmtpServer.ComponentModel;
using SmtpServer.Tracing;
using System.Threading.Tasks;

namespace SMTP
{
    public static class SecureServerExample
    {
        public static void Run()
        {
            // this is important when dealing with a certificate that isnt valid
            ServicePointManager.ServerCertificateValidationCallback = IgnoreCertificateValidationFailureForTestingOnly;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ISmtpServerOptions options = new SmtpServerOptionsBuilder()
                .ServerName("SmtpServer SampleApp")
                .Endpoint(builder =>
                    builder
                        .Port(9025, true)
                        .AllowUnsecureAuthentication(false)
                        .Certificate(CreateCertificate()))
                .Build();

            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.Add(new SampleUserAuthenticator());

            SmtpServer.SmtpServer server = new SmtpServer.SmtpServer(options, serviceProvider);
            server.SessionCreated += OnSessionCreated;

            Task serverTask = server.StartAsync(cancellationTokenSource.Token);

            SampleMailClient.Send(user: "user", password: "password", useSsl: true);

            cancellationTokenSource.Cancel();
            serverTask.Wait();
        }

        static void OnSessionCreated(object sender, SessionEventArgs e)
        {
            Console.WriteLine("Session Created.");

            e.Context.CommandExecuting += OnCommandExecuting;
        }

        static void OnCommandExecuting(object sender, SmtpCommandEventArgs e)
        {
            Console.WriteLine("Command Executing.");

            new TracingSmtpCommandVisitor(Console.Out).Visit(e.Command);
        }

        static bool IgnoreCertificateValidationFailureForTestingOnly(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static X509Certificate2 CreateCertificate()
        {
            // to create an X509Certificate for testing you need to run MAKECERT.EXE and then PVK2PFX.EXE
            // http://www.digitallycreated.net/Blog/38/using-makecert-to-create-certificates-for-development

            byte[] certificate = File.ReadAllBytes(@"C:\Users\cain\Dropbox\Documents\Cain\Programming\SmtpServer\SmtpServer.pfx");
            string password = File.ReadAllText(@"C:\Users\cain\Dropbox\Documents\Cain\Programming\SmtpServer\SmtpServerPassword.txt");

            return new X509Certificate2(certificate, password);
        }
    }
}
