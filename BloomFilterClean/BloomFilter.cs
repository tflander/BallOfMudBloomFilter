using System;
using System.Collections;

namespace BloomFilterClean
{
    public class BloomFilter
    {
        private readonly HashFunction _getHashSecondary;
        private readonly BitArray _hashBits;
        private readonly int _hashFunctionCount;

        public BloomFilter()
        {
            _getHashSecondary = HashString;
            _hashBits = new BitArray(1000000);
            _hashFunctionCount = 3;
        }
        
        public void Add(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this._getHashSecondary(item);
            for (int i = 0; i < this._hashFunctionCount; i++)
            {
                int hash = this.ComputeHash(primaryHash, secondaryHash, i);
                this._hashBits[hash] = true;
            }
        }
        
        public bool Contains(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this._getHashSecondary(item);
            for (int i = 0; i < this._hashFunctionCount; i++)
            {
                int hash = this.ComputeHash(primaryHash, secondaryHash, i);
                if (this._hashBits[hash] == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Performs Dillinger and Manolios double hashing. 
        /// </summary>
        private int ComputeHash(int primaryHash, int secondaryHash, int i)
        {
            int resultingHash = (primaryHash + (i * secondaryHash)) % this._hashBits.Count;
            return Math.Abs((int)resultingHash);
        }
        
        /// <summary>
        /// Hashes a string using Bob Jenkin's "One At A Time" method from Dr. Dobbs (http://burtleburtle.net/bob/hash/doobs.html).
        /// Runtime is suggested to be 9x+9, where x = input.Length. 
        private static int HashString(string s)
        {
            int hash = 0;

            for (int i = 0; i < s.Length; i++)
            {
                hash += s[i];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return hash;
        }
        
        public delegate int HashFunction(string input);        
        
    }
    
    
}