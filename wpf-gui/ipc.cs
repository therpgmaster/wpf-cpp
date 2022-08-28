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
        public ipc() { Task.Factory.StartNew(() => { ipcConnect(); }); } // ctor, runs the connect method in its own thread
        ~ipc() { s.Dispose(); } // dtor

        private NamedPipeServerStream s = null!;
        private StreamReader r = null!;
        private StreamWriter w = null!;
        private volatile bool connected = false;
        private void ipcConnect()
        {
            s = new NamedPipeServerStream("rpgpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
            r = new StreamReader(s);
            s.WaitForConnection();
            w = new StreamWriter(s);
            connected = true; // connection established
        }

        // these may block, run as async tasks
        private void ipcSnd(string d)
        {
            if (!connected) { throw new InvalidOperationException("ipc not established"); } // prevent data loss
            w.Write(d);
            w.Write((char)0);
            w.Flush();
            s.WaitForPipeDrain();
        }
        private string ipcRecv()
        {
            if (!connected) { return ""; }
            var d = r.ReadLine();
            if (!string.IsNullOrEmpty(d)) { return d; }
            else { return ""; }
        }

        public bool isConnected() { return connected; } // should be checked before first send over ipc

        // use await for send/receive
        public async Task send(string data) 
        {
            await Task.Run(() => ipcSnd(data)); 
        }
        public async Task<string> receiveipc() 
        {
            return await Task.Run(() => ipcRecv()); 
        }

    }
}
