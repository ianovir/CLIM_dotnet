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
        private readonly string HEADER_SEPARATOR = "-------------";
        private readonly string DEFAULT_EXIT = "exit";

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
            ExitText = DEFAULT_EXIT;
            HeaderSeparator = HEADER_SEPARATOR;
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
        /// <param name="entryIndex">the index of the entry in the menu</param>
        public void OnChoice(int entryIndex) {

            if (isInvalidEntryChoice(entryIndex))
            {
                TryExecuteExitAction(entryIndex);
            }
            else
            {
                ExecuteEntry(entryIndex);
            }
        }

        private void TryExecuteExitAction(int entryIndex)
        {
            if (IsExitChoice(entryIndex)) ExitAction?.Invoke();
            IsRemoved = RemoveOnInvalidChoice || IsExitChoice(entryIndex);
        }

        private void ExecuteEntry(int entry)
        {
            Entry cEntry = VisibleEntries.ElementAt(entry);
            if (cEntry != null)
            {
                mEngine.Print(cEntry.Name);
                cEntry.OnAction?.Invoke();
                IsRemoved = RemoveOnAction;
            }
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
        public string GetHUT()
        {
            var sb = new StringBuilder();
            PutHutTitle(sb);
            PutHutDescription(sb);
            int lastIndex = PutHUTEntries(sb);
            PutHutTrailer(sb, lastIndex);
            return sb.ToString();
        }

        private void PutHutTrailer(StringBuilder sb, int lastIndex)
        {
            sb.Append(" ").Append(lastIndex).Append(". ").Append(ExitText).Append("\n\n>>");
        }

        private int PutHUTEntries(StringBuilder sb, int startingIndex = 0)
        {
            foreach (Entry me in VisibleEntries)
            {
                sb.Append(" ").Append(startingIndex++).Append(". ").Append(me.Name).Append("\n");
            }

            return startingIndex;
        }

        private void PutHutDescription(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                sb.Append(Description).Append("\n");
            }
        }

        private void PutHutTitle(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(HeaderSeparator)) sb.Append("\n").Append(HeaderSeparator).Append("\n");
            sb.Append(Name.ToUpper()).Append("\n");
        }


    }
}
