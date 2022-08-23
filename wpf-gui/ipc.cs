using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace wpf_gui
{
    // TUTORIAL: https://dev.to/gabbersepp/ipc-between-c-and-c-by-using-named-pipes-4em9?signin=true
    class ipc
    {
        private NamedPipeServerStream srv;
        private StreamReader reader;
        private StreamWriter writer;
        private List<char> buffer = new List<char>(); // incoming stream cache
        public ipc() /* ctor */
        {
            srv = new NamedPipeServerStream("my-very-cool-pipe-example", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
            reader = new StreamReader(srv);
            srv.WaitForConnection();
            writer = new StreamWriter(srv);

            
        }
        ~ipc() { srv.Dispose(); } // dtor
        // use await for send/receive
        public async Task send(string data) { await Task.Run(() => send_s(data)); }
        public async Task<string> receive() { return await Task.Run(() => receive_s()); }
        // these may block, always run as async tasks
        private void send_s(string d)
        {
            writer.Write(d);
            writer.Write((char)0); 
            writer.Flush();
            srv.WaitForPipeDrain();
        }
        private string receive_s()
        {
            var d = reader.ReadLine();
            if (!string.IsNullOrEmpty(d)) { return d; }
            else { return ""; }
        }


    }
}
