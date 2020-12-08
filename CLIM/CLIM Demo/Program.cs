using CLIM.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIM_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> values = new List<string>();

            Engine engine = new Engine("CLIM .net demo");

            Menu mainMenu = engine.BuildMenu("Main menu");
            mainMenu.Description = "List of strings example";
            mainMenu.AddEntry(new Entry("List values") {
                OnAction = ()=>{
                    if (values.Count != 0)
                    {
                        engine.Print("Values:");
                        int v = 0;
                        foreach (string s in values) engine.Print(v++ + ": " + s);
                        engine.Print("--end of list--");
                    }
                    else {
                        engine.Print("--list empty--");
                    }
                }
            });


            mainMenu.AddEntry(new Entry("Add value") {
                OnAction = () => {
                    engine.Print("Type new value:");
                    string newVal = engine.ForceRead();

                    //build a nested menu
                    Menu addMenu = engine.BuildMenu("'Add' options", "back");
                    addMenu.Description = "Choose insert position";
                    values.Add(newVal);
                }           
            });

            mainMenu.AddEntry(new Entry("Remove value")
            {
                OnAction = () => {
                    engine.Print("Remove index: ");
                    int index = -1;
                    int.TryParse(engine.ForceRead(), out index);
                    if (index >= 0) values.RemoveAt(index);
                }
            });

            Menu secondMenu = engine.BuildMenu("Second menu", "cancel");

            secondMenu.AddEntry(new Entry("Pop this menu") { 
                OnAction= () => {
                    engine.PopMenu();
                }
            });

            secondMenu.AddEntry(new Entry("Another action")
            {
                OnAction = () => {
                    //do nothing
                }
            });

            //adding secondMenu to mainMenu as Entry
            mainMenu.AddSubMenu(secondMenu);

            //adding main menu on top of engine
            engine.AddOnTop(mainMenu);
            engine.Start();

            //just blocking...
             while (engine.GetRunning()) { }


        }
    }
}
