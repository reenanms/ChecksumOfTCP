using System;
using System.Collections;

namespace TCP.Checksum
{
    public class Data16Bits : IEquatable<Data16Bits>
    {
        private const int NUMBER_OF_BITS = 16;
        private BitArray bitArray;

        public Data16Bits() : this(false) { }

        public Data16Bits(bool defautOfBits)
        {
            bitArray = new BitArray(NUMBER_OF_BITS, defautOfBits);
        }

        public Data16Bits(byte byteA, byte byteB)
        {
            bitArray = new BitArray(new[] { byteA, byteB });
        }

        public Data16Bits(Data16Bits data)
        {
            bitArray = data.bitArray;
        }

        public byte[] ToByteArray()
        {
            var resut = new byte[2];
            resut[0] = bitArrayToByte(bitArray, 0);
            resut[1] = bitArrayToByte(bitArray, 1);
            return resut;
        }

        public Data16Bits Not()
        {
            var result = new Data16Bits();
            result.bitArray = bitArray.Not();
            return result;
        }

        public Data16Bits Sum(Data16Bits data)
        {
            var result = new Data16Bits();
            result.bitArray = addTwoBitArray(bitArray, data.bitArray);
            return result;
        }

        public bool Equals(Data16Bits other)
        {
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i] != other.bitArray[i])
                    return false;
            }

            return true;
        }

        public static Data16Bits operator +(Data16Bits a, Data16Bits b)
        {
            return a.Sum(b);
        }

        private static byte bitArrayToByte(BitArray bitArray, int byteIndex)
        {
            var partOfByte = byteIndex * 8;
            var newByte = new byte();
            for (int i = 0; i < 8; i++)
            {
                if (bitArray[partOfByte + i])
                    newByte |= (byte)(1 << i);
            }

            return newByte;
        }

        private static BitArray addTwoBitArray(BitArray bitArrayA, BitArray bitArrayB)
        {
            var resultOfBitArray = new BitArray(bitArrayA.Length);
            var carryIn = false;
            for (int i = bitArrayA.Length - 1; i > -1; i--)
            {
                var result = addTwofBits(bitArrayA[i], bitArrayB[i], carryIn);
                resultOfBitArray[i] = result.result;
                carryIn = result.carryOut;
            }

            return resultOfBitArray;
        }

        private static (bool result, bool carryOut) addTwofBits(bool bitA, bool bitB, bool carryIn)
        {
            return
               (bitA, bitB, carryIn) switch
               {
                   (true, true, true) => (true, true),
                   (true, true, false) => (false, true),
                   (true, false, true) => (false, true),
                   (true, false, false) => (true, false),
                   (false, true, true) => (false, true),
                   (false, true, false) => (true, false),
                   (false, false, true) => (true, false),
                   (false, false, false) => (false, false)
               };
        }
    }
}
