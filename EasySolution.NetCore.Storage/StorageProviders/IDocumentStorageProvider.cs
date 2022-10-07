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

    public class UpdateDocumentResult
    {
        public int matchedCount { get; set; }
        public int modifiedCount { get; set; }
    }

    public class DeleteDocumentResult
    {
        public int matchedCount { get; set; }
        public int deletedCount { get; set; }
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
        UpdateDocumentResult UpdateChanges(string id, dynamic changes);
        DeleteDocumentResult DeleteMany(dynamic query);
        DeleteDocumentResult DeleteAll();
        bool DeleteOne(string id);
    }
}
