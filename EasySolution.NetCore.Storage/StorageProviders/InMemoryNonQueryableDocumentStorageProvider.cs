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
    /// Non Queryable Storage: only store object so it will be fastest
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class InMemoryNonQueryableDocumentStorageProvider<TDocument> : IDocumentStorageProvider<TDocument>
    {
        Dictionary<string, TDocument> _docMap = new Dictionary<string, TDocument>();
        public AddDocumentResult Add(TDocument doc)
        {
            string _id = Guid.NewGuid().ToString("N");
            _docMap[_id] = doc;
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
            throw new NotSupportedException("This is Non Queryable Storage => not support DeleteMany by query");
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
            
            return new DocumentRecord<TDocument> {
                source = _docMap[id],
                _id = id
            };
        }

        public UpdateDocumentResult UpdateChanges(string id, dynamic changes)
        {
            throw new NotSupportedException("This is Non Queryable Storage => not support UpdateChanges");
        }
    }
}
