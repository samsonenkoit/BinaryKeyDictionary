using System;
using System.Threading;
using System.Threading.Tasks;
using Dictionarys;

namespace GisHomeWork
{
    class Program
    {
        private static void Main(string[] args)
        {
            ReadDictionaryExample();
            GetByNameAndIdExample();
            ThreadSafeDictionaryExample();

            Console.ReadLine();
        }

        private static void GetByNameAndIdExample()
        {
            Console.WriteLine("Get by name and Id");

            var binaryKeyDictionary = GetTestDictionary();

            int name = 111;
            Console.WriteLine("Get by name {0}",name);

            var getByNameList = binaryKeyDictionary.GetByKeyName(name);

            foreach (var value in getByNameList )
            {
                Console.WriteLine(value);
            }

            int id = 1;
            Console.WriteLine("Get by id {0}", id);

            var getByIdList = binaryKeyDictionary.GetByKeyId(new IntId(id));

            foreach (var value in getByIdList)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine(Environment.NewLine);
        }

        private static void ReadDictionaryExample()
        {
            Console.WriteLine("Read dictionary example");

            var binaryKeyDictionary = GetTestDictionary();


            foreach (var item in binaryKeyDictionary)
            {
                Console.WriteLine(item);
            }


            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Dictionary count = {0}", binaryKeyDictionary.Count);


            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Keys");

            foreach (var key in binaryKeyDictionary.Keys)
            {
                Console.WriteLine(key);
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Id");

            foreach (var id in binaryKeyDictionary.Ids)
            {
                Console.WriteLine(id);
            }

            Console.WriteLine("Names");

            foreach (var name in binaryKeyDictionary.Names)
            {

                Console.WriteLine(name);
            }

            Console.WriteLine(Environment.NewLine);
        }



        private static BinaryKeyDictionary<BinaryKeyItem, IntId, int> GetTestDictionary()
        {
            BinaryKeyDictionary<BinaryKeyItem, IntId, int> dictionary =
                new BinaryKeyDictionary<BinaryKeyItem, IntId, int>();
            dictionary.Add(new BinaryKeyItem(1,22, "3"));
            dictionary.Add(new BinaryKeyItem(4,55, "6"));
            dictionary.Add(new BinaryKeyItem(7,88, "9"));
            dictionary.Add(new BinaryKeyItem(10,111, "12"));
            dictionary.Add(new BinaryKeyItem(13,144, "15"));
            dictionary.Add(new BinaryKeyItem(16,177, "18"));

            dictionary.Add(new BinaryKeyItem(21,111, "13"));
            dictionary.Add(new BinaryKeyItem(1,111, "15"));
            dictionary.Add(new BinaryKeyItem(22,111, "18"));

            return dictionary;
        }

        private static void ThreadSafeDictionaryExample()
        {
            Console.WriteLine("ThreadSafeDictionaryTest");

            ThreadSafeBinaryKeyDictionary<BinaryKeyItem, IntId, int> dictionary =
                new ThreadSafeBinaryKeyDictionary<BinaryKeyItem, IntId, int>();


            Random random = new Random();

            for (int i = 1; i < 100; i++)
            {
                int sleepTime = random.Next(1, 100);

                var i1 = i;
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(sleepTime);

                    int name = i1*10;
                    var item = new BinaryKeyItem(i1, name, name.ToString());
                    dictionary.Add(item);
                    return item;
                }).ContinueWith((task) =>
                {
                    Console.WriteLine("Write item {0}",task.Result);
                }, TaskScheduler.Current);
            }

            foreach (var binaryKeyItem in dictionary)
            {
                Console.WriteLine(binaryKeyItem);
            }

        }
    }
}
