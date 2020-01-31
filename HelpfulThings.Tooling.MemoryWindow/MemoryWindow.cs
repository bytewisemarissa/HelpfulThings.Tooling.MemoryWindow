using System;

namespace HelpfulThings.Tooling.MemoryWindow
{
    public class MemoryWindow
    {
        private byte[] _bytes;
        public byte[] Bytes => _bytes;

        public long LastPosition => Bytes.Length;

        public MemoryWindow(byte[] dataBytes)
        {
            _bytes = dataBytes;
        }

        public MemorySlice GetSlice(long offset, long length)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset >= Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (length > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new MemorySlice(ref _bytes, offset, length);
        }

        public MemoryCursor GetCursor(long offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset > Bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            return new MemoryCursor(ref _bytes, offset, LastPosition);
        }
    }
}
