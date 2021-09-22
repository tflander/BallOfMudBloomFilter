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
            _hashBits = new BitArray(2_000_000_000);
        }
        
        public void Add(string item)
        {
            var value = 0;

            foreach (var t in item)
            {
                value += t;
                value += (value << 10);
                value ^= (value >> 6);
            }

            value += (value << 3);
            value ^= (value >> 11);
            value += (value << 15);

            _hashBits[Math.Abs((int)(item.GetHashCode() % this._hashBits.Count))] = true;
            _hashBits[Math.Abs((int)((item.GetHashCode() +  value) % this._hashBits.Count))] = true;
            _hashBits[Math.Abs((int)((item.GetHashCode() + (2 * value)) % this._hashBits.Count))] = true;
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