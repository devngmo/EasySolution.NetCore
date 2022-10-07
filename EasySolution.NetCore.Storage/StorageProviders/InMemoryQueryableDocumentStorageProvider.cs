using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Storage.StorageProviders
{
    /// <summary>
    /// Queryable Storage: implement Query will cost more operations, so it will be slower than Non Queryable Storage
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class InMemoryQueryableDocumentStorageProvider<TDocument> : IDocumentStorageProvider<TDocument>
    {
        Dictionary<string, Dictionary<string, object>> _docMap = new Dictionary<string, Dictionary<string, object>>();

        public InMemoryQueryableDocumentStorageProvider()
        {
            if (typeof(TDocument) == typeof(Object))
            {
                throw new NotSupportedException("Dynamic Type not supported");
            }
        }
        public AddDocumentResult Add(TDocument doc)
        {
            string _id = Guid.NewGuid().ToString("N");
            DocumentRecord<TDocument> storageDoc = new DocumentRecord<TDocument> {
                _id = _id,
                source = doc
            };
            string jsonStr = JsonSerializer.Serialize(doc);
            _docMap[_id] = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
            return new AddDocumentResult { 
                DocumentId = _id
            };
        }

        public DeleteDocumentResult DeleteAll()
        {
            var result = new DeleteDocumentResult
            {
                matchedCount = _docMap.Count,
                deletedCount = _docMap.Count
            };
            _docMap.Clear();
            return result;
        }

        public DeleteDocumentResult DeleteMany(dynamic query)
        {
            throw new Exception("not implemented");
        }

        public bool DeleteOne(string id)
        {
            if (_docMap.ContainsKey(id))
            {
                _docMap.Remove(id);
                return true;
            }
            return false;
        }

        public DocumentRecord<TDocument>? Get(string id)
        {
            if (!_docMap.ContainsKey(id)) return null;
            Dictionary<string, object> entry = _docMap[id];
            
            string jsonStr = JsonSerializer.Serialize(entry);
            return new DocumentRecord<TDocument> {
                source = JsonSerializer.Deserialize<TDocument>(jsonStr),
                _id = id
            };
        }

        public UpdateDocumentResult UpdateChanges(string id, dynamic changes)
        {
            if (_docMap.ContainsKey(id))
            {
                string jsonStr = JsonSerializer.Serialize(changes);
                _docMap[id] = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
            }
            return new UpdateDocumentResult { 
                matchedCount = 0 , modifiedCount = 0 
            };
        }
    }
}
