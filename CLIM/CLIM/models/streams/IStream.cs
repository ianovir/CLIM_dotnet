using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models.streams
{
    public interface IStream
    {
        void Open();
        void Close();

    }
}
