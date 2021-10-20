using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CLIM.models.streams
{
    public abstract class InputStream : IStream
    {
        protected Engine engine;
        bool run = false;
        Thread thread;        

        public InputStream(Engine subscriber)
        {
            this.engine = subscriber;
            thread = new Thread(DoWork);
            thread.IsBackground = true;

        }

        private void DoWork(object obj)
        {
            while (run) {
                OnInput(ForceRead());
            }
        }

        public void OnInput(string entryIndex) {
            if (String.IsNullOrEmpty(entryIndex)) return;

            int ch = -1;
            int.TryParse(entryIndex, out ch);
            OnInput(ch);
        }

        public void OnInput(int entryindex) {
            engine.OnChoice(entryindex);
        }

        public abstract string ForceRead();

        public void Open()
        {
            if (thread.IsAlive) return;
            run = true;
            thread.Start();
        }

        public void Close()
        {
            run = false;
        }
    }
}
