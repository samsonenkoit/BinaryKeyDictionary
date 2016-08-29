using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionarys.Interface
{
    /// <summary>
    /// Интерфейс который должен реализовывать элемент коллекции  BinaryKeyDictionary
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TName"></typeparam>
    public interface IBinaryKeyItem<out TId, out TName>
    {
        IBinaryKey<TId, TName> Key { get;} 
    }
}
