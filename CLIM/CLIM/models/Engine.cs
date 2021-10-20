using CLIM.models.streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CLIM.models
{
    /// <summary>
    /// The choreographer of the menus' stack.
    /// By default, input and output streams are the ScannerInputStream and
    /// SystemOutputStream respectively
    /// </summary>
    public class Engine {

        public string Name { get; protected set; }
        public Menu CurrentMenu { get; private set; }
        public InputStream InStream { get; set; }
        public IOutputStream OutStream { get; set; }
        public int MenusCount => menus.Count;

        private Stack<Menu> menus;

        private bool running;

        private readonly object syncLock;

        public Engine(string name)
        {
            this.Name = name;
            menus = new Stack<Menu>();
            InStream = new ScannerInputStream(this);
            OutStream = new SystemOutputStream();
            syncLock = new object();
        }

        /// <summary>
        /// Adds a menu on the top of the stack
        /// </summary>
        /// <param name="subMenu">the new menu to be added</param>
        public void AddOnTop(Menu subMenu)
        {
            menus.Push(subMenu);
            CurrentMenu = menus.Peek();
        }

        /// <summary>
        /// Removes the menu on top
        /// </summary>
        public void PopMenu() {
            if (menus.Count > 1) {
                menus.Pop();
            }
            CurrentMenu = menus.Peek();
        }

        /// <summary>
        /// Forces the call to the action corresponding to the chosen entry
        /// </summary>
        /// <param name="entry">the index of the entry in the current menu</param>
        public void OnChoice(int entry) {
            CurrentMenu.OnChoice(entry);
            if (CurrentMenu.IsRemoved) {
                menus.Pop();
                CurrentMenu = menus.Count == 0 ? null : menus.Peek();
            }

            //engine stops if no more menus
            if (CurrentMenu == null)
            {
                Stop();
            }
            else {
                printHUT();
            }
        }

        /// <summary>
        /// Creates a new menu referencing the current engine;
        /// Note that the menu won't be added to the stack automatically.
        /// </summary>
        /// <param name="name">the name of the new menu</param>
        /// <returns>the new built menu</returns>
        [Obsolete]
        public Menu BuildMenu(string name) {
            return new Menu(name, this);
        }

        /// <summary>
        /// Creates a new menu referencing the current engine; 
        /// Note that the menu won't be added to the stack automatically.
        /// </summary>
        /// <param name="name">the name of the new menu</param>
        /// <param name="exitText">the text for the exit entry</param>
        /// <returns>the new built menu</returns>
        [Obsolete]
        public Menu BuildMenu(string name, string exitText) {
            return new Menu(name, exitText, this);
        }

        /// <summary>
        /// Creates a new menu referencing the current engine;
        /// The menu will be added at the top of stack automatically.
        /// </summary>
        /// <param name="name">the name of the new menu</param>
        /// <returns>the new built menu</returns>
        public Menu BuildMenuOnTop(string name)
        {
            Menu m = new Menu(name, this);
            AddOnTop(m);
            return m;
        }

        /// <summary>
        /// Creates a new menu referencing the current engine; 
        /// The menu will be added at the top of stack automatically.
        /// </summary>
        /// <param name="name">the name of the new menu</param>
        /// <param name="exitText">the text for the exit entry</param>
        /// <returns>the new built menu</returns>
        public Menu BuildMenuOnTop(string name, string exitText)
        {
            Menu m = new Menu(name, exitText, this);
            AddOnTop(m);
            return m;
        }

        /// <summary>
        /// Starts the current engine
        /// </summary>
        public void Start()
        {
            lock (syncLock) {
                running = true;
                InStream.Open();
                OutStream.Open();
                present();
                printHUT();            
            }
        }

        /// <summary>
        /// Stops the current engine
        /// </summary>
        public void Stop() {
            lock (syncLock) {
                running= false;
                InStream.Close();
                OutStream.Close();            
            }
        }

        public bool IsRunning()
        {
            lock (syncLock) { 
                return running;
            }
        }

        /// <summary>
        /// Forces the inputstream to be read
        /// </summary>
        /// <returns>the read line</returns>
        public string ForceRead() {
            return InStream.ForceRead();
        }

        /// <summary>
        /// Sends a message to the outputstream
        /// </summary>
        /// <param name="msg">the content to show</param>
        public void Print(string msg) {
            OutStream.OnOutput(msg);
        }

        private void present() {
            OutStream.OnOutput(Name);
        }

        private void printHUT()
        {
            if (CurrentMenu != null) { 
                OutStream.OnOutput(CurrentMenu.GetHUT());
            }
        }
               

    }
}
