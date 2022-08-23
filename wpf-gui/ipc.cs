using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace wpf_gui
{
    // TUTORIAL: https://dev.to/gabbersepp/ipc-between-c-and-c-by-using-named-pipes-4em9?signin=true
    class ipc
    {
        private NamedPipeServerStream srv;
        private StreamReader reader;
        private StreamWriter writer;
        public ipc() /* ctor */
        {
            srv = new NamedPipeServerStream("my-very-cool-pipe-example", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
            reader = new StreamReader(srv);
            srv.WaitForConnection();
            writer = new StreamWriter(srv);
        }
        ~ipc() { srv.Dispose(); } // dtor
        public void send(string data) 
        {
            writer.Write(data);
            writer.Write((char)0);
            writer.Flush();
            srv.WaitForPipeDrain();
        }
        public string receive() { return reader.ReadLine(); }

    }
}
