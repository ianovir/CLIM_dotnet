using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
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
