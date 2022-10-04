using EasySolution.NetCore.Storage.StorageProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.Document
{
    public class DocumentStorage<TDocument>
    {
        
        protected IDocumentStorageProvider<TDocument> storageProvider;
        public DocumentStorage(IDocumentStorageProvider<TDocument> storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public AddDocumentResult Add(TDocument doc)
        {
            return storageProvider.Add(doc);
        }

        public DocumentRecord<TDocument>? Get(string id)
        {
            return storageProvider.Get(id);
        }
    }
}
