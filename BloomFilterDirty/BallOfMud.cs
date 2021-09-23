using System;
using System.Collections;

namespace BloomFilterDirty
{
    public class BallOfMud
    {
        private readonly HashFunction _getHashSecondary;
        public BitArray HashBits { get; set; }

        public BallOfMud()
        {
            _getHashSecondary = HashString;
            HashBits = new BitArray(2_000_000_000);
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

            HashBits[Math.Abs((int)(item.GetHashCode() % this.HashBits.Count))] = true;
            HashBits[Math.Abs((int)((item.GetHashCode() +  value) % this.HashBits.Count))] = true;
            HashBits[Math.Abs((int)((item.GetHashCode() + (2 * value)) % this.HashBits.Count))] = true;
        }

        public bool Contains(string item)
        {
            var primaryHash = item.GetHashCode();
            var secondaryHash = _getHashSecondary(item);

            const int numberOfHashesPerItem = 3;
            for (var i = 0; i < numberOfHashesPerItem; ++i)
            {
                var hash = (primaryHash + (i * secondaryHash)) % HashBits.Count;
                if (!HashBits[Math.Abs(hash)]) return false;
            }

            return true;
        }
        
        private delegate int HashFunction(string input);        
        
        // TODO: made public for testing
        public static int HashString(string s)
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