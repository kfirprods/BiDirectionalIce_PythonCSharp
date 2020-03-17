import traceback
import sys

import Ice

import Demo

PYTHON_PORT = 10000


class PrinterI(Demo.PrinterPython):
    def __init__(self, ice_communicator):
        self.__ice_communicator = ice_communicator
        self.__csharp_printer = None

    def PrintString(self, s, current=None):
        print
        print "Csharp says: {}".format(s)
        print

    def ConnectBack(self, port, current=None):
        # The csharp client tells us which port to connect to. We use that to create a proxy
        csharp_printer_base = self.__ice_communicator.stringToProxy(
            "SimpleCsharpPrinter:default -p {}".format(port))
        
        # Cast the proxy to an ice object
        self.__csharp_printer = Demo.PrinterCsharpPrx.checkedCast(
            csharp_printer_base)

    # Note: This is NOT related to the Ice interface
    def send_message_to_csharp(self, message):
        self.__csharp_printer.PrintString(message)


def create_server(port):
    ice_communicator = None

    try:
        ice_communicator = Ice.initialize()
        adapter = ice_communicator.createObjectAdapterWithEndpoints(
            "SimplePythonPrinterAdapter", "default -p {}".format(port))
        python_printer = PrinterI(ice_communicator)
        adapter.add(python_printer, ice_communicator.stringToIdentity("SimplePythonPrinter"))
        adapter.activate()

    except:
        traceback.print_exc()

    return ice_communicator, python_printer


def main():
    ice_communicator, python_printer = create_server(PYTHON_PORT)
    print "Ice is now listening on port {}".format(PYTHON_PORT)

    # This allows endless communication from this Python app to the C# server
    user_input = raw_input("Insert text to send to csharp: ")
    while user_input:
        python_printer.send_message_to_csharp(user_input)
        user_input = raw_input("Insert text to send to csharp: ")

    ice_communicator.waitForShutdown()


if __name__ == "__main__":
    main()
