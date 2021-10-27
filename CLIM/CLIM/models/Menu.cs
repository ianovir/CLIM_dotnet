using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIM.models
{
    /// <summary>
    /// A menu represents a list of entries.
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public class Menu
    {
        public string Name { get; protected set; }
        public string Description { get; set; }
        public string HeaderSeparator { get; set; }
        public bool RemoveOnAction { get; set; }
        public string ExitText { get; protected set; }
        public bool IsRemoved { get; protected set; }
        public bool RemoveOnInvalidChoice { get; set; }

        protected List<Entry> VisibleEntries {
            get {
                return mEntries.Where(e => e.Visible).ToList();
            }
        }

        /// <summary>
        /// Number of visible entries
        /// </summary>
        public int EntriesCount => VisibleEntries.Count+1;

        /// <summary>
        /// Number of all entries (visible or not)
        /// </summary>
        public int TotalEntriesCount => mEntries.Count+1;
        public Action ExitAction { get; set; }
        private Engine mEngine;
        private LinkedList<Entry> mEntries;

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
        /// Adds a new entry to the menu.
        /// </summary>
        /// <param name="entry">the new entry</param>
        public void AddEntry(Entry entry) {
            mEntries.AddLast(entry);
        }

        /// <summary>
        /// Adds a new entry to the menu
        /// </summary>
        /// <param name="eName">Entry name</param>
        /// <param name="eAction">Entry action</param>
        /// <returns>The new added Entry</returns>
        public Entry AddEntry(string eName, Action eAction) {
            Entry newEntry = new Entry(eName, eAction);
            AddEntry(newEntry);
            return newEntry;
        }

        /// <summary>
        /// Add a new menu as sub entry with a custom entry text (different from menu's name)
        /// </summary>
        /// <param name="subMenu">The new menu to be added</param>
        /// <param name="entryText">the custom entry text if different from menu's name</param>
        public void AddSubMenu(Menu subMenu, string entryText) {
            var e = new Entry(entryText, () => {
                subMenu.IsRemoved = false;
                mEngine.AddOnTop(subMenu);
                }
            );
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

            if (isInvalidEntryChoice(entry)) {
                if (IsExitChoice(entry)) ExitAction?.Invoke();
                IsRemoved = RemoveOnInvalidChoice || IsExitChoice(entry);
                return false;
            }

            Entry cEntry = mEntries.ElementAt(entry);
            if (cEntry != null) {
                mEngine.Print(cEntry.Name);
                cEntry.OnAction?.Invoke();
                IsRemoved = RemoveOnAction;
                return true;
            }

            return false;
        }

        private bool isInvalidEntryChoice(int entry)
        {
            return entry < 0 || entry >= VisibleEntries.Count;
        }

        private bool IsExitChoice(int entry)
        {
            return entry == VisibleEntries.Count;
        }

        /// <summary>
        /// Retrieves the HUT of the current menu
        /// </summary>
        /// <returns>the HUT of the current menu</returns>
        public string GetHUT() {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(HeaderSeparator)) sb.Append("\n").Append(HeaderSeparator).Append("\n");
            sb.Append(Name.ToUpper()).Append("\n");
            if (!string.IsNullOrEmpty(Description)) { 
                sb.Append(Description).Append("\n");
            }
            int men =0;
            foreach (Entry me in mEntries) { 
                sb.Append(men++).Append(". ").Append(me.Name).Append("\n");
            }
            sb.Append(men).Append(". ").Append(ExitText).Append("\n\n>>");
            return sb.ToString();
        }



    }
}
