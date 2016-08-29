using Dictionarys;
using GisHomeWork;
using NUnit.Framework;

namespace BinaryKeyDictionaryTest
{
    [TestFixture]
    public class TestBinaryDictionary
    {
        [Test]
        public void TestAddData()
        {
            BinaryKeyDictionary<BinaryKeyItem, IntId, int> dictionary =
               new BinaryKeyDictionary<BinaryKeyItem, IntId, int>();

            int id = 1;
            int name = 2;
            string value = "Test";
            dictionary.Add(new BinaryKeyItem(id,name,value ));

            Assert.AreEqual(true, dictionary.ContainsKey(new BinaryKey(id,name)));
            Assert.AreEqual(true, dictionary.ContainsKeyId(new IntId(id)));
            Assert.AreEqual(true, dictionary.ContainsKeyName(name));
            Assert.AreEqual(value, dictionary.Get(new BinaryKey(id,name)).Value);
        }

        [Test]
        public void TestRemoveData()
        {
            BinaryKeyDictionary<BinaryKeyItem, IntId, int> dictionary =
               new BinaryKeyDictionary<BinaryKeyItem, IntId, int>();

            int id = 1;
            int name = 2;
            string value = "Test";
            dictionary.Add(new BinaryKeyItem(id,name,value));

            var key = new BinaryKey(1,2);
            Assert.AreEqual(true, dictionary.ContainsKey(key));

            dictionary.Remove(key);

            Assert.AreEqual(false, dictionary.ContainsKey(key));
            Assert.AreEqual(false, dictionary.ContainsKeyId(key.Id));
            Assert.AreEqual(false, dictionary.ContainsKeyName(key.Name));
        }
    }
}
