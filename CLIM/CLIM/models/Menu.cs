using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIM.models
{
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

        public void AddEntry(Entry entry) {
            mEntries.AddLast(entry);
        }

        public void AddSubMenu(Menu subMenu, string entryText) {
            var e = new Entry(entryText){
                OnAction = () => {
                    mEngine.AddOnTop(subMenu);
                }
            };
            mEntries.AddLast(e);
        }

        public void AddSubMenu(Menu subMenu) {
            AddSubMenu(subMenu, subMenu.Name);
        }

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
