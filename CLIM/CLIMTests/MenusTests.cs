using CLIM.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLIMTests
{
    [TestClass]
    public class MenusTests
    {
        [TestMethod]
        public void TestAddMenuByEntry()
        {
            Engine engine = new Engine("engine name");
            Menu menu1 = new Menu("menu1", engine);
            Menu menu2 = new Menu("menu2", engine);

            menu1.AddEntry("add menu2 on top", ()=>engine.AddOnTop(menu2));
            engine.AddOnTop(menu1);

            engine.Start();
            engine.OnChoice(0);
            engine.Stop();

            Assert.AreEqual(2, engine.MenusCount);
        }

        [TestMethod]
        public void testRemoveMenuByEntry()
        {
            Engine engine = new Engine("engine name");
            Menu menu1 = new Menu("menu1", engine);
            Menu menu2 = new Menu("menu2", engine);

            menu1.AddEntry("pop menu on top", ()=> engine.PopMenu());

            engine.AddOnTop(menu1);
            engine.AddOnTop(menu2);

            engine.Start();
            engine.OnChoice(0);
            engine.Stop();

            Assert.AreEqual(1, engine.MenusCount);
        }

        [TestMethod]
        public void testPersistenceMenuOnAction()
        {
            Engine engine = new Engine("engine name");
            Menu menu1 = new Menu("menu1", engine);
            Menu subMenu = new Menu("sub menu", engine);

            subMenu.AddEntry("placeholder action", ()=> { });

            menu1.AddSubMenu(subMenu);

            engine.AddOnTop(menu1);

            engine.Start();
            engine.OnChoice(0);//choose sub menu (auto-entry)
            engine.OnChoice(0);//choose sub menu's entry 0
            engine.Stop();

            //submenu won't be removed
            Assert.AreEqual(2, engine.MenusCount);
        }

        [TestMethod]
        public void testRemoveMenuOnAction()
        {
            Engine engine = new Engine("engine name");
            Menu menu1 = new Menu("menu1", engine);
            Menu subMenu = new Menu("sub menu", engine);

            subMenu.RemoveOnAction = true;
            subMenu.AddEntry("placeholder action", ()=> { });

            menu1.AddSubMenu(subMenu);

            engine.AddOnTop(menu1);

            engine.Start();
            engine.OnChoice(0);//choose sub menu (auto-entry)
            engine.OnChoice(0);//choose sub menu's entry 0
            engine.Stop();

            Assert.AreEqual(1, engine.MenusCount);
        }

    }
}
