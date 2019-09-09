using System;
using System.Globalization;

namespace WaybillService.Core
{
    public abstract class LongBasedId<TSelf> : IEquatable<TSelf> where TSelf : LongBasedId<TSelf>
    {
        public long Value { get; }

        protected LongBasedId(long value)
        {
            if (value <= 0)
            {             
                throw new InvalidLongBasedIdException<TSelf>(value);
            }
            Value = value;
        }

        public static bool operator != (
            LongBasedId<TSelf> value1, LongBasedId<TSelf> value2) =>
            !AreEqual(value1, value2);
        
        public static bool operator == (
            LongBasedId<TSelf> value1, LongBasedId<TSelf> value2) =>
            AreEqual(value1, value2);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is LongBasedId<TSelf> other && AreEqual(this, other);
        }
        
        public bool Equals(TSelf other)
        {
            return AreEqual(this, other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() =>
            Value.ToString(CultureInfo.InvariantCulture);

        private static bool AreEqual(LongBasedId<TSelf> first, LongBasedId<TSelf> second)
        {
            if (ReferenceEquals(first, null))
            {
                return ReferenceEquals(second, null);
            }

            if (ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Value == second.Value;
        }
    }
    
    public class InvalidLongBasedIdException<TSelf> : BusinessException
    { 
        public InvalidLongBasedIdException(long value) : base(
            $"Cannot create {nameof(TSelf)} with Id = {value}. Id should be positive")
        {
        }
    }
}