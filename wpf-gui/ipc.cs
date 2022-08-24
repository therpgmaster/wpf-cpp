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
        class StreamSrvState 
        {
            private NamedPipeServerStream s = null!;
            private StreamReader r = null!;
            private StreamWriter w = null!;
            public StreamSrvState() { Task.Factory.StartNew(() => { init(); }); } // ctor, runs the init method in its own thread
            ~StreamSrvState() { s.Dispose(); } // dtor
            private volatile bool initDone = false;
            private void init()
            {
                s = new NamedPipeServerStream("rpgpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
                r = new StreamReader(s);
                s.WaitForConnection();
                w = new StreamWriter(s);
                initDone = true;
            }
            private void waitForInit() 
            {
                if (initDone) return;
                System.Threading.Thread.Sleep(3000);
                if (!initDone) { throw new InvalidOperationException("ipc init timed out"); }
            }

            // these may block, run as async tasks
            public void send(string d)
            {
                waitForInit(); // prevent data loss
                w.Write(d);
                w.Write((char)0);
                w.Flush();
                s.WaitForPipeDrain();
            }
            public string receive()
            {
                if (!initDone) { return ""; }
                var d = r.ReadLine();
                if (!string.IsNullOrEmpty(d)) { return d; }
                else { return ""; }
            }
        }

        private StreamSrvState srv = new StreamSrvState();

        // use await for send/receive
        public async Task send(string data) 
        {
            await Task.Run(() => srv.send(data)); 
        }
        public async Task<string> receive() 
        {
            return await Task.Run(() => srv.receive()); 
        }

    }
}
