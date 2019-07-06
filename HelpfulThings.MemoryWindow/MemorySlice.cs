using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HelpfulThings.MemoryWindow
{
    [Serializable]
    public struct MemorySlice
    {
        private byte[] _array;
        private readonly long _offset;

        public long Count { get; }

        public byte this[long index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if(index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _array[_offset + index];
            }
            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                _array[_offset + index] = value;
            }
        }

        public MemorySlice(ref byte[] array)
        {
            _array = array ?? throw new ArgumentNullException(nameof(array));
            _offset = 0;
            Count = array.Length;
        }

        public MemorySlice(ref byte[] array, long offset, long count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (array.Length - offset < count)
                throw new ArgumentException("The array segment will end after the end of the array.");

            _array = array;
            _offset = offset;
            Count = count;
        }

        public MemoryCursor GetCursorAtBeginning()
        {
            return GetCursor(0);
        }

        public MemoryCursor GetCursor(long offset)
        {
            if(offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if(offset > Count)
                throw new ArgumentOutOfRangeException(nameof(offset));

            return new MemoryCursor(ref _array, _offset + offset, _offset + Count);
        }

        public MemorySlice GetSlice(long offset, long length)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset >= Count)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (length > Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new MemorySlice(ref _array, _offset+offset, length);
        }

        public byte[] Dump()
        {
            var dumpBytes = new byte[Count];

            for (long i = _offset; i < (_offset + Count); i++)
            {
                dumpBytes[(i - _offset)] = _array[i];
            }

            return dumpBytes;
        }
        
        public override bool Equals(Object obj)
        {
            if (obj is MemorySlice slice)
                return Equals(slice);

            return false;
        }

        public override int GetHashCode()
        {
            //Ask Resharper, I'm assuming this is safe.
            unchecked
            {
                var hashCode = (_array != null ? _array.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _offset.GetHashCode();
                hashCode = (hashCode * 397) ^ Count.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(MemorySlice obj)
        {
            return obj._array == _array && obj._offset == _offset && obj.Count == Count;
        }
        
        public static bool operator == (MemorySlice a, MemorySlice b)
        {
            return a.Equals(b);
        }

        public static bool operator != (MemorySlice a, MemorySlice b)
        {
            return !(a == b);
        }
    }
}
