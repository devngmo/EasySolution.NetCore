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
    public interface IDocumentStorageProvider<TDocument>
    {
        AddDocumentResult Add(TDocument doc);
        TDocument? Get(string id);
    }
}
