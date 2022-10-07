using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.MongoDB
{
    public class CreateMongoDocumentStorageOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public bool UseObjectID { get; set; } = true;
    }
}
