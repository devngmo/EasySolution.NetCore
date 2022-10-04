using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.StorageProviders
{
    public class InMemoryDocumentStorageProvider<TDocument> : IDocumentStorageProvider<TDocument>
    {
        Dictionary<string, TDocument> _docMap = new Dictionary<string, TDocument>();
        public AddDocumentResult Add(TDocument doc)
        {
            string _id = Guid.NewGuid().ToString("D");
            _docMap[_id] = doc;
            return new AddDocumentResult { 
                DocumentId = _id
            };
        }

        public TDocument? Get(string id)
        {
            if (!_docMap.ContainsKey(id)) return default(TDocument);
            return _docMap[id];
        }
    }
}
