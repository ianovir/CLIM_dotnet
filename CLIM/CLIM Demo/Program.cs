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

            Engine engine = new Engine("CLIM .net core demo");

            Menu mainMenu = new Menu("Main menu", engine);
            mainMenu.Description = "List of strings example";
            mainMenu.AddEntry(new Entry("List values", ()=>{
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
                })
            );


            mainMenu.AddEntry("Add value", () => {
                    engine.Print("Type new value:");
                    string newVal = engine.ForceRead();
                    values.Add(newVal);
                }           
            );

            mainMenu.AddEntry("Remove value", () => {
                    engine.Print("Remove index: ");
                    int index = -1;
                    int.TryParse(engine.ForceRead(), out index);
                    if (index >= 0) values.RemoveAt(index);
                }
            );

            Menu secondMenu = new Menu("Second menu", "cancel", engine);
            secondMenu.AddEntry("Pop this menu", () =>  engine.PopMenu());
            secondMenu.AddEntry("Another action", () => { });

            //adding secondMenu to mainMenu as Entry
            mainMenu.AddSubMenu(secondMenu);

            //adding main menu on top of engine
            engine.AddOnTop(mainMenu);
            engine.Start();

            //just blocking...
            while (engine.IsRunning()) ;


        }
    }
}
