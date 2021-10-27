using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public class ScannerInputStream : InputStream
    {
        public ScannerInputStream(Engine subscriber):base(subscriber)
        {
        }

        public override string ForceRead()
        {
            return Console.ReadLine();
        }
    }
}
