using System.Threading;

namespace CLIM.models.streams
{
    /// <summary>
    /// Input Stream for the <see cref="Engine"/>
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public abstract class InputStream : IStream
    {
        protected Engine engine;
        protected bool run = false;
        protected Thread thread;        

        public InputStream(Engine subscriber)
        {
            engine = subscriber;
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
            if (string.IsNullOrEmpty(entryIndex)) return;

            int ch = -1;
            int.TryParse(entryIndex, out ch);
            OnInput(ch);
        }

        public void OnInput(int entryindex) {
            engine.OnChoice(entryindex);
        }

        /// <summary>
        /// Forces the read from the stream. Please, consider that this call will be called on separate thread. 
        /// This call can be supposed to be blocking.
        /// </summary>
        /// <returns></returns>
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
