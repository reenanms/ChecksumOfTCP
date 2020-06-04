using System;
using System.Text;

namespace TCP.Checksum
{
    public class Message
    {
        private Data16Bits[] data;

        private Message(string data, Encoding encoding)
        {
            Data16Bits[] dataInInData16Bits = convertStringToData16Bits(data, encoding);
            Data16Bits checksumData = checksum(dataInInData16Bits);

            Data16Bits[] datawithChecksum = dataInInData16Bits.Join(new[] { checksumData });

            this.data = datawithChecksum;
        }
        private Data16Bits[] convertStringToData16Bits(string data, Encoding encoding)
        {
            byte[] bytesOfData = encoding.GetBytes(data);

            var lenghtOfBitArray = bytesOfData.Length / 2;
            var addBitArray = bytesOfData.Length % 2 != 0 ? 1 : 0;
            var bitArrayOfData = new Data16Bits[lenghtOfBitArray + addBitArray];

            for (int i = 0; i < bitArrayOfData.Length - 1; i++)
                bitArrayOfData[i] = new Data16Bits(bytesOfData[i * 2], bytesOfData[(i * 2) + 1]);

            if (addBitArray == 0)
                bitArrayOfData[bitArrayOfData.Length - 1] = new Data16Bits(bytesOfData[(bitArrayOfData.Length - 1) * 2], bytesOfData[((bitArrayOfData.Length - 1) * 2) + 1]);
            else
                bitArrayOfData[bitArrayOfData.Length - 1] = new Data16Bits(bytesOfData[(bitArrayOfData.Length - 1) * 2], new byte());

            return bitArrayOfData;
        }

        private Data16Bits checksum(Data16Bits[] data)
        {
            var sum = sumOfAll(data);
            return sum.Not();
        }

        private Data16Bits sumOfAll(Data16Bits[] data)
        {
            var sumOfAllBitArrays = new Data16Bits();
            for (int i = 0; i < data.Length; i++)
                sumOfAllBitArrays = sumOfAllBitArrays + data[i];
            return sumOfAllBitArrays;
        }

        public void Validate()
        {
            var checksumExpectedResult = new Data16Bits(true);
            var checksumResult = sumOfAll(data);
            if (!checksumResult.Equals(checksumExpectedResult))
                throw new Exception("Invalid message, checksum does not match");
        }

        public static Message Pack(string message)
        {
            return new Message(message, Encoding.Unicode);
        }

        public void SimulateError()
        {
            if (data.Length > 0)
                data[0] = data[0].Not();
        }

        public static string Unpack(Message data)
        {
            return data.unpack(Encoding.Unicode);
        }

        private string unpack(Encoding encoding)
        {
            var receivedDataWithOutChecksum = data.SubArray(0, data.Length - 1);
            string messageReceived = convert16BitArrayInString(receivedDataWithOutChecksum, encoding);
            return messageReceived;
        }

        private string convert16BitArrayInString(Data16Bits[] data, Encoding encoding)
        {
            byte[] bytesOfData = new byte[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                var data16BitsInBytes = data[i].ToByteArray();
                bytesOfData[(i * 2)] = data16BitsInBytes[0];
                bytesOfData[(i * 2) + 1] = data16BitsInBytes[1];
            }

            return encoding.GetString(bytesOfData);
        }
    }
}
