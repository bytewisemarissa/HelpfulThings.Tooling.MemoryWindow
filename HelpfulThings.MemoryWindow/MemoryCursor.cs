using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulThings.MemoryWindow
{
    public class MemoryCursor
    {
        private readonly byte[] _memoryBytes;
        private long _currentLocation;
        private readonly long _startIndex;
        private readonly long _endIndex;

        public MemoryCursor(ref byte[] memoryBytes, long startingPosition, long? endIndex = null)
        {
            _memoryBytes = memoryBytes;
            _currentLocation = startingPosition;
            _startIndex = startingPosition;
            // ReSharper disable once PossibleInvalidOperationException
            _endIndex = endIndex.HasValue ? _memoryBytes.LongLength : endIndex.Value;
        }

        public bool Read(out byte value)
        {
            if (_currentLocation >= _endIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(_currentLocation));
            }

            value = _memoryBytes[_currentLocation];

            _currentLocation++;

            return _currentLocation >= _endIndex;
        }

        public void MoveToBeginning()
        {
            _currentLocation = _startIndex;
        }
    }
}