using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.StorageProviders
{
    public class AddDocumentResult
    {
        public string DocumentId { get; set; }
    }
    public class DocumentRecord<TDocument>
    {
        public string _id { get; set; }
        public TDocument source { get; set; }
    }
    public interface IDocumentStorageProvider<TDocument>
    {
        AddDocumentResult Add(TDocument doc);
        DocumentRecord<TDocument>? Get(string id);
    }
}
