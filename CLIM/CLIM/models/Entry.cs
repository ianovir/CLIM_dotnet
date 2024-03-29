﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLIM.models
{
    /// <summary>
    /// An Entry is the basic element composing a menu.
    /// </summary> 
    /// <author>Sebastiano Campisi (ianovir)</author>
    public class Entry
    {
        public string Name { get; protected set; }
        public bool Visible { get; set; }

        [Obsolete]
        public Entry(string name): this(name, null){}

        public Entry(string name, Action action)
        {
            this.Name = name;
            Visible = true;
            this.OnAction = action;
        }

        /// <summary>
        /// The action (to be implemented) for the entry
        /// </summary>
        public Action OnAction;

    }
}
