using System;
using System.Collections;
using System.Text;

namespace TCP.Checksum
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var messageToSend = askMessageToSend();
                var dataToSend = Message.Pack(messageToSend);

                if (askErroSimulation())
                    dataToSend.SimulateError();
                var receivedData = dataToSend;

                receivedData.Validate();
                var messageReceived = Message.Unpack(receivedData);

                showReceivedMessage(messageReceived);
            }
            catch (Exception e)
            {
                showError(e);
            }
        }

        private static void showError(Exception e)
        {
            Console.WriteLine("Erro ocorreu durante o processo:");
            Console.WriteLine(e.Message);
        }

        private static void showReceivedMessage(string messageReceived)
        {
            Console.WriteLine("Mensagem recebida:");
            Console.WriteLine(messageReceived);
        }

        private static bool askErroSimulation()
        {
            Console.WriteLine("Para simular mensagem enviada/entregue com erro, digite 'S':");
            string simulateError = Console.ReadLine();
            bool simulateErro = simulateError == "S";
            return simulateErro;
        }

        private static string askMessageToSend()
        {
            Console.WriteLine("Escreva uma mensagem para enviar:");
            string messageToSend = Console.ReadLine();
            return messageToSend;
        }
    }
}
