using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models
{
    public class Entry
    {

        public string Name { get; protected set; }
        public bool Visible { get; set; }

        public Entry(string name)
        {
            this.Name = name;
            Visible = true;
        }

        public Action OnAction;

    }
}
