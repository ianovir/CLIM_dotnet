using CLIM.models.streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CLIM.models
{
    public class Engine {

        public string Name { get; protected set; }
        public Menu CurrentMenu { get; private set; }

        public InputStream InStream { get; set; }
        public IOutputStream OutStream { get; set; }

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

        public void AddOnTop(Menu subMenu)
        {
            menus.Push(subMenu);
            CurrentMenu = menus.Peek();
        }

        public void PopMenu() {
            if (menus.Count > 1) {
                menus.Pop();
            }
            CurrentMenu = menus.Peek();
        }


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

        public Menu BuildMenu(string name) {
            return new Menu(name, this);
        }

        public Menu BuildMenu(string name, string exitText) {
            return new Menu(name, exitText, this);
        }

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

        public void Stop() {
            lock (syncLock) {
                running= false;
                InStream.Close();
                OutStream.Close();            
            }
        }

        public bool GetRunning()
        {
            lock (syncLock) { 
                return running;
            }
        }

        public string ForceRead() {
            return InStream.ForceRead();
        }

        public void Print(string msg) {
            OutStream.OnOutput(msg);
        }

        private void present() {
            OutStream.OnOutput(Name);
        }

        private void printHUT()
        {
            Thread.Sleep(500);
            OutStream.OnOutput(CurrentMenu.GetHUT());
        }

    }
}
