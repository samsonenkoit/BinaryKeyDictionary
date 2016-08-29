namespace Dictionarys.Interface
{
    /// <summary>
    /// Интерфейс который должен реализовывать ключь 
    /// элемента коллекции BinaryKeyDictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TName"></typeparam>
    public interface IBinaryKey<out TKey, out TName>
    {
         TKey Id { get;  }
         TName Name { get;  }
    }
}
