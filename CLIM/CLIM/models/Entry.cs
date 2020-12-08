using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models
{
    /// <summary>
    /// An Entry is the basic element composing a menu.
    /// </summary>
    public class Entry
    {

        public string Name { get; protected set; }
        public bool Visible { get; set; }

        public Entry(string name)
        {
            this.Name = name;
            Visible = true;
        }

        /// <summary>
        /// The action (to be implemented) for the entry
        /// </summary>
        public Action OnAction;

    }
}
