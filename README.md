
# ChecksumOfTCP

Implementation of the TCP protocol checksum in C#.


## How It Works
This project simulates the generation of a 16-bit message for transmission, calculates the checksum (16 bits), validates the received message, and displays the result.

- The system asks the user for the message to send.
- It asks if you want to simulate a transmission/reception error.
- It validates the received data.
- It displays the received data or the generated error.



### Converting the Message to 16-bit Data
```csharp
private Message(string data, Encoding encoding)
{
    // Converts the string message to an array of 16-bit data
    Data16Bits[] dataIn16Bits = convertStringToData16Bits(data, encoding);
    // Calculates the checksum of the message
    Data16Bits checksumData = checksum(dataIn16Bits);

    // Combines the message with the checksum value
    Data16Bits[] dataWithChecksum = dataIn16Bits.Join(new[] { checksumData });

    this.data = dataWithChecksum;
}
```



### Data Validation
```csharp
public void Validate()
{
    // Creates a 16-bit data with all bits set to 1
    var checksumExpectedResult = new Data16Bits(true);
    // Sums all the data in the message (including the checksum)
    var checksumResult = sumOfAll(data);
    // Validates if the sum result is a 16-bit set with all bits as 1
    if (!checksumResult.Equals(checksumExpectedResult))
        throw new Exception("Invalid message, checksum does not match");
}
```



### Unpacking Received 16-bit Data
```csharp
private string unpack(Encoding encoding)
{
    // Removes the checksum data from the message
    var receivedDataWithoutChecksum = data.SubArray(0, data.Length - 1);
    // Converts the 16-bit data array back to a string
    string messageReceived = convert16BitArrayInString(receivedDataWithoutChecksum, encoding);
    return messageReceived;
}
```



## References
- https://en.wikipedia.org/wiki/Transmission_Control_Protocol#TCP_checksum_for_IPv4
- https://tools.ietf.org/html/rfc793
- https://www.slashroot.in/how-is-tcp-and-udp-checksum-calculated
- https://stackoverflow.com/questions/11204666/converting-c-sharp-byte-to-bitarray
- https://stackoverflow.com/questions/560123/convert-from-bitarray-to-byte
- https://www.electronics-tutorials.ws/combination/comb_7.html
- https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
- https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading