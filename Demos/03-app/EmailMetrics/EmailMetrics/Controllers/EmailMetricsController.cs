using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EmailMetrics.Controllers
{
  public class EmailMetricsController : Controller
  {
    CloudStorageAccount _storageAccount;
    CloudBlobClient _storageClient;
    CloudBlobContainer _storageContainer;

    // GET: EmailMetrics
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost, ActionName("ShowMetrics")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ShowMetrics()
    {
      var emailMetrics = ProcessEmails();

      return View(emailMetrics);
    }

    private List<Models.EmailMetric> ProcessEmails()
    {
      var emailMetrics = new List<Models.EmailMetric>();

      // connect to the storage account
      _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString-1"]);
      _storageClient = _storageAccount.CreateCloudBlobClient();

      // connect to the container
      _storageContainer = _storageClient.GetContainerReference("maildump");

      // get a list of all emails
      var blobResults = _storageContainer.ListBlobs();

      // process each email
      foreach (IListBlobItem blob in blobResults)
      {
        if (blob.GetType() == typeof(CloudBlockBlob))
        {
          var cloudBlob = (CloudBlockBlob)blob;
          var blockBlob = _storageContainer.GetBlobReference(cloudBlob.Name);

          var emailMetric = ProcessEmail(blockBlob);

          // if already have this sender... 
          var existingMetric = emailMetrics.FirstOrDefault(metric => metric.Email == emailMetric.Email);
          if (existingMetric != null)
          {
            existingMetric.RecipientsToEmail += emailMetric.RecipientsToEmail;
          }
          else
          {
            emailMetrics.Add(emailMetric);
          }

        }
      }

      return emailMetrics;
    }

    private Models.EmailMetric ProcessEmail(CloudBlob emailBlob)
    {
      var emailMetric = new Models.EmailMetric();

      using (var reader = new StreamReader(emailBlob.OpenRead()))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          var jsonObj = JObject.Parse(line);

          // extract sender
          var sender = jsonObj.SelectToken("Sender.EmailAddress.Address")?.ToString();

          // extract and count up recipients
          var totalRecipients = 0;
          totalRecipients += jsonObj.SelectToken("ToRecipients").Children().Count();
          totalRecipients += jsonObj.SelectToken("CcRecipients").Children().Count();
          totalRecipients += jsonObj.SelectToken("BccRecipients").Children().Count();

          emailMetric.Email = sender;
          emailMetric.RecipientsToEmail = totalRecipients;
        }
      }

      return emailMetric;
    }

  }
}