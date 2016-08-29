using System.Drawing;
using Dictionarys.Interface;

namespace GisHomeWork
{
    public sealed class BinaryKey : IBinaryKey<IntId, int>
    {
        #region Static
        public static bool Equals(BinaryKey x, BinaryKey y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.GetHashCode() == y.GetHashCode();
        }
        #endregion

        public IntId Id { get; private set; }
        public int Name { get; private set; }

        public BinaryKey(int id, int name)
        {
            Id = new IntId(id);
            Name = name;
        }

        public bool Equals(BinaryKey obj)
        {
            return Equals(this, obj);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is BinaryKey && Equals((BinaryKey)obj);
        }

        public override int GetHashCode()
        {
            Point p = new Point(Id.Value, Name);
            return p.GetHashCode();
        }

        public static bool operator ==(BinaryKey x, BinaryKey y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BinaryKey x, BinaryKey y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return string.Format("Id = {0}, Name = {1}", Id, Name);
        }
    }
}
