using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
    public class SystemOutputStream : IOutputStream
    {
        public void OnOutput(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Close()
        {
            //do nothing
        }


        public void Open()
        {
            //do nothing
        }
    }
}
