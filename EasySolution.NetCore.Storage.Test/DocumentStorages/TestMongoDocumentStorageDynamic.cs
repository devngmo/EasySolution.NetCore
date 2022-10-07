using EasySolution.NetCore.CredVault;
using EasySolution.NetCore.Storage.Document;
using EasySolution.NetCore.Storage.MongoDB;
using EasySolution.NetCore.Storage.StorageProviders;
using System.Text.Json;

namespace EasySolution.NetCore.Storage.Test.DocumentStorages
{
    [TestClass]
    public class TestMongoDocumentStorageDynamic
    {
        DocumentStorage<dynamic> storage;
        [TestInitialize]
        public void Initialize()
        {
            c
            MongoDocumentStorageProvider<dynamic> provider = new MongoDocumentStorageProvider<dynamic>(
                new CreateMongoDocumentStorageOptions { 
                    DatabaseName = "Sandbox",
                    CollectionName = "Books",
                    ConnectionString = credVault.GetString("my-mongo-cloud-connection-string")!
                });
            storage = new DocumentStorage<dynamic>(provider);
            storage.DeleteAll();
        }

        [TestMethod]
        public void serializeOneDocument()
        {
            var book = new { title="Awesome Book", author="Mr.Awesome" };
            AddDocumentResult result = storage.Add(book);
            Assert.IsNotNull(result.DocumentId);

            //JsonElement e;
            //e.GetProperty("").
            DocumentRecord<dynamic>? bookOnStorage = storage.Get(result.DocumentId);
            Assert.IsNotNull(bookOnStorage);
            Assert.AreEqual(result.DocumentId, bookOnStorage._id);
            Assert.AreEqual("Awesome Book", bookOnStorage.source.GetProperty("title").GetString());
            Assert.AreEqual("Mr.Awesome", bookOnStorage.source.GetProperty("author").GetString());
            Console.WriteLine(JsonSerializer.Serialize(bookOnStorage));
        }

        [TestMethod]
        public void serializeMassiveDocuments()
        {
            List<AddDocumentResult> resultList = new List<AddDocumentResult>();
            int nDocs = 100;

            Console.WriteLine($"Add {nDocs} documents...");
            DateTime start = DateTime.Now;
            for (int i = 0; i < nDocs; i++)
            {
                var book = new { title = $"Awesome Book {i}", author = $"Mr.Awesome {i}" };
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
                DocumentRecord<dynamic>? bookOnStorage = storage.Get(resultList[i].DocumentId);
                Assert.IsNotNull(bookOnStorage);
                Assert.AreEqual(resultList[i].DocumentId, bookOnStorage._id);
                Assert.AreEqual($"Awesome Book {i}", bookOnStorage.source.GetProperty("title").GetString());
                Assert.AreEqual($"Mr.Awesome {i}", bookOnStorage.source.GetProperty("author").GetString());
            }
            duration = DateTime.Now - start;
            Console.WriteLine($"Load and validate {nDocs} documents take {duration.TotalSeconds} seconds");
        }
    }
}