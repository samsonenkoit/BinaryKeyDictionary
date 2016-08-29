using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dictionarys.Interface;

namespace Dictionarys
{

    /*
     * В качестве ключа список использует объект реализующий интерфейс IBinaryKey.
     * 
     * пары Ключь - значение хранятся в словаре _valueDictionary и большинство методов служат оберткой над методами _valueDictionary
     * 
     * словари _idDictionary,_nameDictionary используются для хранения пар: Св-во ключа (Id/Name) - соответствующий ключь, необходимо для 
     * быстрого поиска по св-вам ключа (Id/Name)
     * 
     */

    /// <summary>
    /// Класс коллекция для хранения элементов, имеющих уникальный составной ключ [Id, Name] 
    /// </summary>
    /// <typeparam name="TId">Тип поля Id ключа массива</typeparam>
    /// <typeparam name="TName">Тип поля Name ключа массива</typeparam>
    /// <typeparam name="T"></typeparam>
    public class BinaryKeyDictionary<T, TId, TName> : IEnumerable<T> where T : IBinaryKeyItem<TId, TName>
    {

        private readonly Dictionary<IBinaryKey<TId, TName>, T> _valueDictionary;

        private readonly Dictionary<TId, HashSet<IBinaryKey<TId, TName>>> _idDictionary;
        private readonly Dictionary<TName, HashSet<IBinaryKey<TId, TName>>> _nameDictionary;

        #region Property

        public int Count
        {
            get { return GetCount(); }
        }

        public IList<IBinaryKey<TId, TName>> Keys
        {
            get { return GetKeys(); }
        }

        public IList<TId> Ids
        {
            get { return GetIds(); }
        }

        public IList<TName> Names
        {
            get { return GetNames(); }
        } 

        #endregion

        public BinaryKeyDictionary()
        {
            _valueDictionary = new Dictionary<IBinaryKey<TId, TName>, T>();
            _idDictionary = new Dictionary<TId, HashSet<IBinaryKey<TId, TName>>>();
            _nameDictionary = new Dictionary<TName, HashSet<IBinaryKey<TId, TName>>>();

        }

        protected virtual int GetCount()
        {
            return _valueDictionary.Count;
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.Key == null) throw new NullReferenceException("item.Key");

            if (_valueDictionary.ContainsKey(item.Key)) throw new Exception("Элемент с таким ключем уже был добавлен");

            Set(item, true);

        }

        protected virtual void Set(T item, bool isNew = false)
        {


            if (isNew)
            {
                SaveValueKeyForBinaryKeyProperty(item.Key, item.Key.Id, _idDictionary);
                SaveValueKeyForBinaryKeyProperty(item.Key, item.Key.Name, _nameDictionary);
            }

            _valueDictionary[item.Key] = item;
        }

        public virtual bool Remove(IBinaryKey<TId, TName> key)
        {
            if (key == null) return false;

            var remove = _valueDictionary.Remove(key);

            if (!remove) return false;

            RemoveValueKeyForBinaryKeyProperty(key, key.Id, _idDictionary);
            RemoveValueKeyForBinaryKeyProperty(key, key.Name, _nameDictionary);

            return true;
        }



        public virtual T Get(IBinaryKey<TId, TName> key)
        {
            if (key == null) throw new ArgumentNullException("key");

            return _valueDictionary[key];
        }

        public virtual IList<T> GetByKeyId(TId id)
        {
            if(id == null) throw new ArgumentNullException("id");

            var valueKeys = _idDictionary[id];

            return valueKeys.Select(valueKey => _valueDictionary[valueKey]).ToList();
        }

        public virtual bool ContainsKeyId(TId id)
        {
            if(id == null) throw new ArgumentNullException("id"); 

            return _idDictionary.ContainsKey(id);
        }

        public virtual bool ContainsKeyName(TName name)
        {
            if(name == null) throw new ArgumentNullException("name");

            return _nameDictionary.ContainsKey(name);
        }

        public virtual IList<T> GetByKeyName(TName name)
        {
            if(name == null) throw new ArgumentNullException("name");

            var valueKeys = _nameDictionary[name];

            return valueKeys.Select(valueKey => _valueDictionary[valueKey]).ToList();
        }

        protected virtual IList<IBinaryKey<TId, TName>> GetKeys()
        {
            return _valueDictionary.Keys.ToList();
        }

        protected virtual IList<TId> GetIds()
        {
            return _idDictionary.Keys.ToList();
        }

        protected virtual IList<TName> GetNames()
        {
            return _nameDictionary.Keys.ToList();
        } 
 

        public virtual void Clear()
        {
            _valueDictionary.Clear();
            _idDictionary.Clear();
            _nameDictionary.Clear();
        }

        public virtual bool ContainsKey(IBinaryKey<TId, TName> key)
        {
            if(key == null) throw new ArgumentNullException("key");

            return _valueDictionary.ContainsKey(key);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return _valueDictionary.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Helper


        /// <summary>
        /// Сохраняет пару: св-во ключа (Id,Name) - ключь
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <param name="valueKey">Ключь</param>
        /// <param name="binaryKeyProperty">Св-во бинарного ключа</param>
        /// <param name="dictionary">словарь в котором хранится отношение св-во бинарного ключа - ключь</param>
        private void SaveValueKeyForBinaryKeyProperty<TP>(IBinaryKey<TId, TName> valueKey, TP binaryKeyProperty,
            Dictionary<TP, HashSet<IBinaryKey<TId, TName>>> dictionary)
        {
            if (!dictionary.ContainsKey(binaryKeyProperty))
            {
                dictionary.Add(binaryKeyProperty, new HashSet<IBinaryKey<TId, TName>>() {valueKey});
            }
            else
            {
                dictionary[binaryKeyProperty].Add(valueKey);
            }
        }


        /// <summary>
        /// Удаляет пару: св-во ключа (Id/Name) - ключь
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <param name="valueKey"></param>
        /// <param name="binaryKeyProperty"></param>
        /// <param name="dictionary"></param>
        private void RemoveValueKeyForBinaryKeyProperty<TP>(IBinaryKey<TId, TName> valueKey, TP binaryKeyProperty,
            Dictionary<TP, HashSet<IBinaryKey<TId, TName>>> dictionary)
        {
            if (!dictionary.ContainsKey(binaryKeyProperty)) return;

            var keys = dictionary[binaryKeyProperty];

            if (!keys.Remove(valueKey)) return;

            if (keys.Any())
                dictionary[binaryKeyProperty] = keys;
            else
            {
                dictionary.Remove(binaryKeyProperty);
            }
        }

        #endregion

    }
}
