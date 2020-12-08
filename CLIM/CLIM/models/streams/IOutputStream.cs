using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
    public interface IOutputStream : IStream
    {
        void OnOutput(string msg);
    }
}
