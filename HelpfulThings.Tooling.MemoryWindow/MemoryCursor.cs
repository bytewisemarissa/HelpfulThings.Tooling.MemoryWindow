using System;
using System.Linq;

namespace HelpfulThings.Tooling.MemoryWindow
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
        
        public bool Read(out byte[] value, long bytesToRead)
        {
            if (_currentLocation + bytesToRead >= _endIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(bytesToRead));
            }

            value = new byte[bytesToRead];

            long iterator = 0;
            while (iterator < bytesToRead)
            {
                value[iterator] = _memoryBytes[_currentLocation];
                
                iterator++;
                _currentLocation++;
            }
            
            return _currentLocation >= _endIndex;
        }

        public byte[] ReadToEnd()
        {
            var arrayLength = _memoryBytes.Length - _currentLocation;
            
            var returnValue = new byte[arrayLength];
            
            long iterator = 0;
            while (iterator < arrayLength)
            {
                returnValue[iterator] = _memoryBytes[_currentLocation];
                
                iterator++;
                _currentLocation++;
            }

            return returnValue;
        }


        public void MoveToBeginning()
        {
            _currentLocation = _startIndex;
        }
    }
}