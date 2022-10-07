using EasySolution.NetCore.Storage.Document;
using EasySolution.NetCore.Storage.StorageProviders;

namespace EasySolution.NetCore.Storage.Test.DocumentStorages
{
    [TestClass]
    public class TestDocumentStorageNotSupportDynamic
    {
        DocumentStorage<dynamic> storage;
        [TestMethod]
        public void TestNotSupportDynamic()
        {
            Assert.ThrowsException<NotSupportedException>(() => new InMemoryQueryableDocumentStorageProvider<dynamic>());
        }
    }
}