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

            var bookOnStorage = storage.Get(result.DocumentId);
            Assert.IsNotNull(bookOnStorage);
            Assert.IsNotNull("Awesome Book", "" + bookOnStorage.title);
            Assert.IsNotNull("Mr.Awesome", "" + bookOnStorage.author);
        }
    }
}