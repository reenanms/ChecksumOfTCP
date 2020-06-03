using System;
using System.Collections;
using System.Text;

namespace TCP.Checksum
{
    class Program
    {
        /*
         * References used to implementation:
            [1] https://en.wikipedia.org/wiki/Transmission_Control_Protocol#TCP_checksum_for_IPv4
            [2] https://tools.ietf.org/html/rfc793
            [3] https://www.slashroot.in/how-is-tcp-and-udp-checksum-calculated
            [4] https://stackoverflow.com/questions/11204666/converting-c-sharp-byte-to-bitarray
            [5] https://stackoverflow.com/questions/560123/convert-from-bitarray-to-byte
            [6] https://www.electronics-tutorials.ws/combination/comb_7.html
            [7] https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
        */
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Escreva uma mensagem para enviar:");
                string messageToSend = Console.ReadLine();

                BitArray[] dataToSend = genereteMessageDataToSend(messageToSend);
                BitArray[] receivedData = dataToSend;
                string messageReceived = buildAndCheckReceivedMessage(receivedData);

                Console.WriteLine("Mensagem recebida:");
                Console.WriteLine(messageReceived);
            }
            catch(Exception e)
            {
                Console.WriteLine("Erro ocorreu durante o processo:");
                Console.WriteLine(e.Message);
            }
        }

        private static string buildAndCheckReceivedMessage(BitArray[] receivedData)
        {
            BitArray checksumToValidate = sumOfAll16BitArrays(receivedData);
            validateChecksum(checksumToValidate);
            BitArray[] receivedDataWithOutChecksum = gerPartOfArrat(receivedData, 0, receivedData.Length - 1);
            string messageReceived = convert16BitArrayInString(receivedDataWithOutChecksum);
            return messageReceived;
        }

        private static BitArray[] genereteMessageDataToSend(string messageToSend)
        {
            BitArray[] dataInIn16BitArray = convertStringIn16BitArray(messageToSend);
            BitArray checksumToSend = generateChecksumOf16BitArray(dataInIn16BitArray);
            BitArray[] dataToSend = joinArrays(dataInIn16BitArray, new[] { checksumToSend });
            return dataToSend;
        }

        private static void validateChecksum(BitArray checksumToValidate)
        {
            var checksumExpectedResult = new BitArray(16, true);
            if (!isBitArrayEquals(checksumToValidate, checksumExpectedResult))
                throw new Exception("Invalid message, checksum does not match");
        }

        private static bool isBitArrayEquals(BitArray checksumToValidate, BitArray checksumExpectedResult)
        {
            if (checksumToValidate.Length != checksumExpectedResult.Length)
                return false;

            for (int i = 0; i < checksumToValidate.Length; i++)
            {
                if (checksumToValidate[i] != checksumExpectedResult[i])
                    return false;
            }

            return true;
        }

        private static BitArray[] gerPartOfArrat(BitArray[] array, int startIndex, int size)
        {
            var result = new BitArray[size];
            for (int i = 0; i < size; i++)
                result[i] = array[startIndex + i];

            return result;
        }

        private static BitArray[] joinArrays(BitArray[] arrayA, BitArray[] arrayB)
        {
            var result = new BitArray[arrayA.Length + arrayB.Length];
            for (int i = 0; i < arrayA.Length; i++)
                result[i] = arrayA[i];

            for (int i = 0; i < arrayB.Length; i++)
                result[arrayA.Length + i] = arrayB[i];

            return result;
        }

        private static BitArray generateChecksumOf16BitArray(BitArray[] data)
        {
            var sumOfAll = sumOfAll16BitArrays(data);
            return sumOfAll.Not();
        }

        private static BitArray sumOfAll16BitArrays(BitArray[] data)
        {
            var sumOfAllBitArrays = new BitArray(16, false);
            for (int i = 0; i < data.Length; i++)
            {
                sumOfAllBitArrays = adderOfBitArrays(sumOfAllBitArrays, data[i]);
            }

            return sumOfAllBitArrays;
        }

        private static BitArray adderOfBitArrays(BitArray bitArrayA, BitArray bitArrayB)
        {
            var resultOfBitArray = new BitArray(bitArrayA.Length);
            var carryIn = false;
            for (int i = bitArrayA.Length-1; i > -1; i--)
            {
                var result = adderOfBits(bitArrayA[i], bitArrayB[i], carryIn);
                resultOfBitArray[i] = result.result;
                carryIn = result.carryOut;
            }

            return resultOfBitArray;
        }

        private static (bool result, bool carryOut) adderOfBits(bool bitA, bool bitB, bool carryIn)
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


        private static string convert16BitArrayInString(BitArray[] data)
        {
            byte[] bytesOfData = new byte[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                bytesOfData[(i * 2)] = bitArrayToByte(data[i], 0);
                bytesOfData[(i * 2) + 1] = bitArrayToByte(data[i], 1);
            }

            return MessageEncoding.GetString(bytesOfData);
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

        private static Encoding MessageEncoding => Encoding.Unicode;

        private static BitArray[] convertStringIn16BitArray(string data)
        {
            byte[] bytesOfData = MessageEncoding.GetBytes(data);

            var lenghtOfBitArray = bytesOfData.Length / 2;
            var addBitArray = bytesOfData.Length % 2 != 0 ? 1 : 0;
            var bitArrayOfData = new BitArray[lenghtOfBitArray + addBitArray];

            for (int i = 0; i < bitArrayOfData.Length-1; i++)
                bitArrayOfData[i] = new BitArray(new[] { bytesOfData[i*2], bytesOfData[(i * 2) + 1] });

            if (addBitArray == 0)
                bitArrayOfData[bitArrayOfData.Length - 1] = new BitArray(new[] { bytesOfData[(bitArrayOfData.Length - 1) * 2], bytesOfData[((bitArrayOfData.Length - 1) * 2) + 1] });
            else
                bitArrayOfData[bitArrayOfData.Length - 1] = new BitArray(new[] { bytesOfData[(bitArrayOfData.Length - 1) * 2], new byte() });

            return bitArrayOfData;
        }
    }
}
