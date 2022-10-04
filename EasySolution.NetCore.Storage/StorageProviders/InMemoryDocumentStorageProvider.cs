using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.StorageProviders
{
    public class InMemoryDocumentStorageProvider<TDocument> : IDocumentStorageProvider<TDocument>
    {
        Dictionary<string, DocumentRecord<TDocument>> _docMap = new Dictionary<string, DocumentRecord<TDocument>>();
        public AddDocumentResult Add(TDocument doc)
        {
            string _id = Guid.NewGuid().ToString("D");
            DocumentRecord<TDocument> storageDoc = new DocumentRecord<TDocument> {
                _id = _id,
                source = doc
            };
            _docMap[_id] = storageDoc;
            return new AddDocumentResult { 
                DocumentId = _id
            };
        }

        public DocumentRecord<TDocument>? Get(string id)
        {
            if (!_docMap.ContainsKey(id)) return null;
            return _docMap[id];
        }
    }
}
