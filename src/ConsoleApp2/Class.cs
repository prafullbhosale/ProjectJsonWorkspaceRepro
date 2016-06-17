using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Class
    {
        public class Cloud
        {
            public int ID { get; set; }
            public string Name { get; set; }

            private CloudBlobContainer _container;

            public Task<CloudBlobContainer> GetCloudBlobContainer()
            {
                return Task.FromResult(_container);
            }

            public Cloud()
            {
                _container = new CloudBlobContainer(new Uri(""));
            }

            public async Task ThisMakesItBreak()
            {
                var container = await GetCloudBlobContainer();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    //PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
        }
    }
}
