using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public class SystemOutputStream : IOutputStream
    {
        public void Put(string msg)
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
