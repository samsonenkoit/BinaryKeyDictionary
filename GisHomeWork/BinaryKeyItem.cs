using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dictionarys.Interface;

namespace GisHomeWork
{
    public class BinaryKeyItem: IBinaryKeyItem<IntId,int>
    {
        public IBinaryKey<IntId, int> Key { get; private set; }

        public string Value { get; set; }

        public BinaryKeyItem(int id, int name, string value)
        {
            Key = new BinaryKey(id,name );
            Value = value;
        }

        public override string ToString()
        {
            string value = Value ?? "";
            return string.Format("Id = {0} Name = {1} Value = {2}", Key.Id.Value, Key.Name, value);
        }
    }
}
