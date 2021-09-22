using System;
using System.Collections;

namespace BloomFilterDirty
{
    public class BallOfMud
    {
        private readonly HashFunction _getHashSecondary;
        private readonly BitArray _hashBits;

        public BallOfMud()
        {
            _getHashSecondary = HashString;
            _hashBits = new BitArray(1000000);
        }
        
        public void Add(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this._getHashSecondary(item);
            int h1 = primaryHash % this._hashBits.Count;
            int h2 = (primaryHash +  secondaryHash) % this._hashBits.Count;
            int h3 = (primaryHash + (2 * secondaryHash)) % this._hashBits.Count;
            _hashBits[Math.Abs((int)h1)] = true;
            _hashBits[Math.Abs((int)h2)] = true;
            _hashBits[Math.Abs((int)h3)] = true;
        }

        public bool Contains(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this._getHashSecondary(item);
            int h1 = primaryHash % this._hashBits.Count;
            int h2 = (primaryHash +  secondaryHash) % this._hashBits.Count;
            int h3 = (primaryHash + (2 * secondaryHash)) % this._hashBits.Count;

            if (!_hashBits[Math.Abs((int)h1)]) return false;
            if (!_hashBits[Math.Abs((int)h2)]) return false;
            if (!_hashBits[Math.Abs((int)h3)]) return false;
            
            return true;
        }
        
        public delegate int HashFunction(string input);        
        
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
        
    }
}