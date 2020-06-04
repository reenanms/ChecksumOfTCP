# ChecksumOfTCP
Implementação do Checksum utilizado pelo protocolo TCP.

## Funcionanmento
O sistema simula a geração de uma mensagem de 16bits para envio, junto com a montagem do valor do checksum (em 16bits), validação do seu recebimento e exibição do dado. 

- O sistema pegunta pra o usuário a mensagem que ele deseja enviar;
- Pegunta se deseja simular um erro de envio/recebimento;
- Valida o dado recebido;
- Exibe o dados recebido ou o erro gerado;


##### Conversão da mensagem em dados de 16bits
``` csharp
private Message(string data, Encoding encoding)
{
    //Converte a mensagem de string para um array de 16bits
    Data16Bits[] dataInInData16Bits = convertStringToData16Bits(data, encoding);
    //Faz o checksum da mensagem
    Data16Bits checksumData = checksum(dataInInData16Bits);

    //Junta a mensagem com o valor do checksum
    Data16Bits[] datawithChecksum = dataInInData16Bits.Join(new[] { checksumData });

    this.data = datawithChecksum;
}
```


##### Validação do dado
``` csharp
public void Validate()
{
    //Cria um dado com 16bits 1
    var checksumExpectedResult = new Data16Bits(true);
    // Soma todo a dado da mensagem (inclusive com o checksum)
    var checksumResult = sumOfAll(data);
    //Valida se o resultado da soma é um conjunto de 16bist com valor 1 em cada bit
    if (!checksumResult.Equals(checksumExpectedResult))
        throw new Exception("Invalid message, checksum does not match");
}
```


##### Conversão dos dados de 16bits recebidos
``` csharp
private string unpack(Encoding encoding)
{
    //Remove o dado de checksum da mensagem
    var receivedDataWithOutChecksum = data.SubArray(0, data.Length - 1);
    //Converte os dados em 16bits em string
    string messageReceived = convert16BitArrayInString(receivedDataWithOutChecksum, encoding);
    return messageReceived;
}
```


## Referencias usadas para a implementação
- https://en.wikipedia.org/wiki/Transmission_Control_Protocol#TCP_checksum_for_IPv4
- https://tools.ietf.org/html/rfc793
- https://www.slashroot.in/how-is-tcp-and-udp-checksum-calculated
- https://stackoverflow.com/questions/11204666/converting-c-sharp-byte-to-bitarray
- https://stackoverflow.com/questions/560123/convert-from-bitarray-to-byte
- https://www.electronics-tutorials.ws/combination/comb_7.html
- https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
- https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading