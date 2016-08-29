using System.Drawing;
using System.Reflection;

namespace GisHomeWork
{
    /// <summary>
    /// Простая реализация класса поля Id для BinaryKey
    /// </summary>
    public sealed class IntId
    {
        #region Static
        public static bool Equals(IntId x, IntId y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y,null))
            {
                return false;
            }

            return x.GetHashCode() == y.GetHashCode();
        }
        #endregion

        public int Value { get; private set; }

        public IntId(int value)
        {
            Value = value;
        }

        public bool Equals(IntId obj)
        {
            return Equals(this, obj);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is IntId && Equals((IntId)obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(IntId x, IntId y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(IntId x, IntId y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
