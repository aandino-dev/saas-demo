using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Azure.Management.Sql.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using saasdemo.data;
using saasdemo.data.Data;
using saasdemo.data.Data.Repositories;

namespace saasdemo.webjob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static async Task ProcessQueueMessage([QueueTrigger("tenant-queue")] Tenant tenant)
        {
            TenantRepository tenantRepo = new TenantRepository(tenant.TenantID);
            data.ITenant t = await tenantRepo.GetAsync();

            if (t == null || string.IsNullOrEmpty(t.Server) == false || string.IsNullOrEmpty(t.Database) == false)
            {
                Console.WriteLine("[{0}] Tenant already configured or something when wrong.", tenant.TenantID);
                return;
            }


            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(CloudConfigurationManager.GetSetting("az:clientId"), //clientId,
                CloudConfigurationManager.GetSetting("az:clientSecret"),//clientSecret,
                CloudConfigurationManager.GetSetting("az:tenantId"),//tenantId,
                AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithDefaultSubscription();

            string startAddress = "0.0.0.0";
            string endAddress = "255.255.255.255";

            var servers = await azure.SqlServers.ListAsync();
            var avaibleServers = servers.Where(x => x.Databases.List().Count < 150 && x.ResourceGroupName == "ssas-demo");

            ISqlServer sqlServer;
            if (avaibleServers.Any())
            {
                sqlServer = avaibleServers.FirstOrDefault();
            }
            else
            {
                string sqlServerName = SdkContext.RandomResourceName("saas-", 8);
                // Create the SQL server instance
                sqlServer = azure.SqlServers.Define(sqlServerName)
                    .WithRegion(Region.USEast)
                    .WithExistingResourceGroup("ssas-demo")
                    .WithAdministratorLogin(CloudConfigurationManager.GetSetting("sqlserver:username"))
                    .WithAdministratorPassword(CloudConfigurationManager.GetSetting("sqlserver:password"))
                    .WithNewFirewallRule(startAddress, endAddress)
                    .Create();
            }

            string dbName = SdkContext.RandomResourceName("saas-", 8);

            // Create the database
            ISqlDatabase sqlDb = sqlServer.Databases.Define(dbName)
                    .WithEdition(DatabaseEditions.Standard)
                    .WithServiceObjective(ServiceObjectiveName.S0)
                    .Create();


            Console.WriteLine(sqlServer.FullyQualifiedDomainName);
            Console.WriteLine(sqlDb.Name);

            await tenantRepo.UpdateAsync(sqlDb.SqlServerName, sqlDb.Name);

            if (string.IsNullOrEmpty(tenant.Email) == false)
                await SendEmailNotification(tenant.Email, tenant.Organization);
        }

        private static async Task SendEmailNotification(string email, string organization)
        {
            var smtp = new SmtpClient
            {
                Host = CloudConfigurationManager.GetSetting("mail:host"),
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(CloudConfigurationManager.GetSetting("mail:username"), CloudConfigurationManager.GetSetting("mail:password"))
            };

            smtp.SendCompleted += (s, e) =>
            {
                Console.WriteLine("E-Mail has been sent to {0}", email);
            };

            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("no-reply@sassdemo.com");
            MailMessage mail = new MailMessage(from, to);

            mail.Subject = $"Your workspace {organization} has been created successfully";

            mail.Body = $"Hi,\n\nYour workpace {organization} is ready to use";


            Console.WriteLine("Sending email to {0}...", email);
            await smtp.SendMailAsync(mail);
        }
    }
}
