using EasySolution.NetCore.CredVault;
using EasySolution.NetCore.Storage.Document;
using EasySolution.NetCore.Storage.MongoDB;
using EasySolution.NetCore.Storage.StorageProviders;
using EasySolution.NetCore.Storage.Test.Models;
using System.Text.Json;

namespace EasySolution.NetCore.Storage.Test.DocumentStorages
{
    [TestClass]
    public class TestMongoDocumentStorageTypedDocument
    {
        DocumentStorage<BookInfo> storage;
        [TestInitialize]
        public void Initialize()
        {
            var credVault = RedisCredVault.Localhost;
            MongoDocumentStorageProvider<BookInfo> provider = new MongoDocumentStorageProvider<BookInfo>(
                new CreateMongoDocumentStorageOptions { 
                    DatabaseName = "Sandbox",
                    CollectionName = "Books",
                    ConnectionString = credVault.GetString("my-mongo-cloud-connection-string")!
                });
            storage = new DocumentStorage<BookInfo>(provider);
            storage.DeleteAll();
        }

        [TestMethod]
        public void serializeOneDocument()
        {
            BookInfo book = new BookInfo  { title="Awesome Book", author="Mr.Awesome" };
            AddDocumentResult result = storage.Add(book);
            Assert.IsNotNull(result.DocumentId);

            //JsonElement e;
            //e.GetProperty("").
            DocumentRecord<BookInfo>? bookOnStorage = storage.Get(result.DocumentId);
            Assert.IsNotNull(bookOnStorage);
            Assert.AreEqual(result.DocumentId, bookOnStorage._id);
            Assert.AreEqual("Awesome Book", bookOnStorage.source.title);
            Assert.AreEqual("Mr.Awesome", bookOnStorage.source.author);
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
                BookInfo book = new BookInfo { title = $"Awesome Book {i}", author = $"Mr.Awesome {i}" };
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
                DocumentRecord<BookInfo>? bookOnStorage = storage.Get(resultList[i].DocumentId);
                Assert.IsNotNull(bookOnStorage);
                Assert.AreEqual(resultList[i].DocumentId, bookOnStorage._id);
                Assert.AreEqual($"Awesome Book {i}", bookOnStorage.source.title);
                Assert.AreEqual($"Mr.Awesome {i}", bookOnStorage.source.author);
            }
            duration = DateTime.Now - start;
            Console.WriteLine($"Load and validate {nDocs} documents take {duration.TotalSeconds} seconds");
        }
    }
}