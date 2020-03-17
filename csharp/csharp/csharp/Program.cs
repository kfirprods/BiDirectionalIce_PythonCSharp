using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Demo;

namespace csharp
{
    class Program
    {
        const int PythonPort = 10000;
        const int CsharpPort = 10001;

        public class PrinterI : Demo.PrinterCsharpDisp_
        {
            public override void PrintString(string s, Ice.Current current)
            {
                Console.WriteLine();
                Console.WriteLine($"Python says: {s}");
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var pythonPrinter = ConnectToPython(PythonPort);

            // Send basic message
            pythonPrinter.PrintString("Hello World!");

            var csharpIceCommunicator = ExposeCsharpPrinter(CsharpPort);

            // Tell python to connect to us
            pythonPrinter.ConnectBack(CsharpPort);

            // This allows endless communication from this C# app to the Python server
            Console.WriteLine("Insert some text to send to the python:");
            var userInput = Console.ReadLine();

            while (userInput.Length > 0)
            {
                pythonPrinter.PrintString(userInput);

                Console.WriteLine();
                Console.WriteLine("Insert some text to send to the python:");
                userInput = Console.ReadLine();
            }

            csharpIceCommunicator.waitForShutdown();
        }

        private static Ice.Communicator ExposeCsharpPrinter(int csharpPort)
        {
            var communicator = Ice.Util.initialize();

            var adapter =
                communicator.createObjectAdapterWithEndpoints("SimpleCsharpPrinterAdapter", $"default -h localhost -p {csharpPort}");
            adapter.add(new PrinterI(), Ice.Util.stringToIdentity("SimpleCsharpPrinter"));
            adapter.activate();

            return communicator;
        }

        private static PrinterPythonPrx ConnectToPython(int pythonPort)
        {
            var ic = Ice.Util.initialize();
            Ice.ObjectPrx obj = ic.stringToProxy($"SimplePythonPrinter:default -p {pythonPort}");

            return PrinterPythonPrxHelper.checkedCast(obj);
        }
    }
}
