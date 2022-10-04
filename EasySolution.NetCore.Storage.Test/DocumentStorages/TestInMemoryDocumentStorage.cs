using EasySolution.NetCore.Storage.Document;
using EasySolution.NetCore.Storage.StorageProviders;

namespace EasySolution.NetCore.Storage.Test.DocumentStorages
{
    [TestClass]
    public class TestInMemoryDocumentStorage
    {
        DocumentStorage<dynamic> storage;
        [TestInitialize]
        public void Initialize()
        {
            InMemoryDocumentStorageProvider<dynamic> provider = new StorageProviders.InMemoryDocumentStorageProvider<dynamic>();
            storage = new DocumentStorage<dynamic>(provider);
        }

        [TestMethod]
        public void serializeOneDocument()
        {
            var book = new { title="Awsome Book", author="Mr.Awesome" };
            AddDocumentResult result = storage.Add(book);
            Assert.IsNotNull(result.DocumentId);

            DocumentRecord<dynamic> bookOnStorage = storage.Get(result.DocumentId);
            Assert.IsNotNull(bookOnStorage);
            Assert.IsNotNull(result.DocumentId, "" + bookOnStorage._id);
            Assert.IsNotNull("Awesome Book", "" + bookOnStorage.source.title);
            Assert.IsNotNull("Mr.Awesome", "" + bookOnStorage.source.author);
        }

        [TestMethod]
        public void serializeMassiveDocuments()
        {
            List<AddDocumentResult> resultList = new List<AddDocumentResult>();
            int nDocs = 10000000;

            Console.WriteLine($"Add {nDocs} documents...");
            DateTime start = DateTime.Now;
            for (int i = 0; i < nDocs; i++)
            {
                var book = new { title = $"Awsome Book {i}", author = $"Mr.Awesome {i}" };
                AddDocumentResult result = storage.Add(book);
                Assert.IsNotNull(result.DocumentId);
                resultList.Add(result);
            }
            TimeSpan duration = DateTime.Now - start;
            Console.WriteLine($"Add {nDocs} documents take {duration.TotalSeconds} seconds");

            Console.WriteLine($"Load and Validate {nDocs} documents...");
            start = DateTime.Now;
            for (int i = 0; i < nDocs; i++)
            {
                DocumentRecord<dynamic> bookOnStorage = storage.Get(resultList[i].DocumentId);
                Assert.IsNotNull(bookOnStorage);
                Assert.IsNotNull(resultList[i].DocumentId, "" + bookOnStorage._id);
                Assert.IsNotNull($"Awesome Book {i}", "" + bookOnStorage.source.title);
                Assert.IsNotNull($"Mr.Awesome {i}", "" + bookOnStorage.source.author);
            }
            duration = DateTime.Now - start;
            Console.WriteLine($"Load and validate {nDocs} documents take {duration.TotalSeconds} seconds");
        }
    }
}