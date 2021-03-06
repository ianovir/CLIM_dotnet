﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIM.models
{
    /// <summary>
    /// A menu represents a list of entries.
    /// </summary>
    public class Menu
    {
        public string Name { get; protected set; }
        public string Description { get; set; }
        public string HeaderSeparator { get; set; }
        public bool RemoveOnAction { get; set; }
        public string ExitText { get; protected set; }
        public bool IsRemoved { get; protected set; }

        private Engine mEngine;
        private LinkedList<Entry> mEntries;
        private bool removeOnInvalidChoice;


        public Menu(string name, Engine engine)
        {
            this.Name = name;
            mEntries = new LinkedList<Entry>();
            this.mEngine = engine;
            ExitText = "exit";
            HeaderSeparator = "-------------";
        }

        public Menu(String name, String exitText, Engine engine):this(name, engine)
        {
            this.ExitText = exitText;
        }

        /// <summary>
        /// Add a new entry to menu.
        /// </summary>
        /// <param name="entry">the new entry</param>
        public void AddEntry(Entry entry) {
            mEntries.AddLast(entry);
        }


        /// <summary>
        /// Add a new menu as sub entry with a custom entry text (different from menu's name)
        /// </summary>
        /// <param name="subMenu">The new menu to be added</param>
        /// <param name="entryText">the custom entry text if different from menu's name</param>
        public void AddSubMenu(Menu subMenu, string entryText) {
            var e = new Entry(entryText){
                OnAction = () => {
                    mEngine.AddOnTop(subMenu);
                }
            };
            mEntries.AddLast(e);
        }

        /// <summary>
        /// Add a new menu as sub entry
        /// </summary>
        /// <param name="subMenu">The new menu to be added</param>
        public void AddSubMenu(Menu subMenu) {
            AddSubMenu(subMenu, subMenu.Name);
        }

        /// <summary>
        /// Forces the call to the action corresponding to the chosen entry
        /// </summary>
        /// <param name="entry">the index of the entry in the menu</param>
        /// <returns>True if an action has been properly triggered, False otherwise</returns>
        public bool OnChoice(int entry) {
            if (entry == mEntries.Count) return !(IsRemoved = true);
            if (entry < 0 || entry > mEntries.Count) return false;

            Entry cEntry = mEntries.ElementAt(entry);
            if (cEntry != null) {
                cEntry.OnAction.Invoke();
                IsRemoved = RemoveOnAction;
                return true;
            }
            if (removeOnInvalidChoice) IsRemoved = true;
            return false;
        }

        /// <summary>
        /// Retrieves the HUT of the current menu
        /// </summary>
        /// <returns>the HUT of the current menu</returns>
        public string GetHUT() {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(HeaderSeparator)) sb.Append("\n" + HeaderSeparator + "\n");
            sb.Append(Name.ToUpper() + "\n");
            if (!String.IsNullOrEmpty(Description)) sb.Append(Description + "\n");
            int men =0;
            foreach (Entry me in mEntries)
                sb.Append(men++ + ". " + me.Name + "\n");

            sb.Append(men++ + ". "+ ExitText + "\n\n>>");
            return sb.ToString();
        }

    }
}
