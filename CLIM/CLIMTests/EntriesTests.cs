using CLIM.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CLIMTests
{
    [TestClass]
    public class EntriesTests
    {
        [TestMethod]
        public void TestAddSubtract()
        {
            int[] sum = { 0 };
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("sum menu");
            sMenu.AddEntry("add 4", ()=>sum[0] += 4);
            sMenu.AddEntry("sub 2", ()=>sum[0] -= 2);

            engine.Start();
            engine.OnChoice(0);
            engine.OnChoice(1);
            engine.Stop();

            Assert.AreEqual(2, sum[0]);
        }

        [TestMethod]
        public void TestNullActionEntry()
        {
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("menu");
            sMenu.AddEntry("entry1", null);

            engine.Start();
            try
            {
                engine.OnChoice(0);
                Assert.IsTrue(true);
            }
            catch (NullReferenceException ex)
            {
                Assert.Fail(ex.Message);
            }

            engine.Stop();
        }

        [TestMethod]
        public void TestCancelActionAfterHidingEntry()
        {
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("menu");
            Entry hiding = sMenu.AddEntry("Entry1", null);
            sMenu.AddEntry("Hide Entry1", ()=>hiding.Visible=false);

            engine.Start();
            try
            {
                engine.OnChoice(1);//hiding entry1
                engine.OnChoice(1);//exiting
                Assert.IsTrue(true);
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.Fail(ex.Message);
            }

            engine.Stop();
        }

        [TestMethod]
        public void TestCheckMenuWithHiddenEntry()
        {
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("menu");

            Entry hiddenEntry = new Entry("Hidden entry", ()=>{ });
            hiddenEntry.Visible = false;
            sMenu.AddEntry(hiddenEntry);

            sMenu.AddEntry("Another entry", ()=> { });

            engine.Start();
            int noe = sMenu.EntriesCount;
            engine.Stop();

            Assert.AreEqual(2, noe);//another entry and exit one
        }


        [TestMethod]
        public void TestCheckHideEntryWithAnotherAction()
        {
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("menu");

            Entry hiddenEntry = new Entry("Hidden entry", ()=>{ });
            Entry guiltyEntry = new Entry("Guilty entry", ()=>hiddenEntry.Visible=false);

            sMenu.AddEntry(guiltyEntry);
            sMenu.AddEntry(hiddenEntry);

            engine.Start();
            int noe_before = sMenu.EntriesCount;
            engine.OnChoice(0);
            int noe_after = sMenu.EntriesCount;
            engine.Stop();

            Assert.AreEqual(3, noe_before);
            Assert.AreEqual(2, noe_after);
        }

        [TestMethod]
        public void TestCheckShowAndExecuteEntryWithAnotherAction()
        {
            Engine engine = new Engine("engine name");
            Menu sMenu = engine.BuildMenuOnTop("menu");

            int flag = -1;
            Entry hiddenEntry = new Entry("Hidden entry", ()=> flag=1 );
            hiddenEntry.Visible = false;
            Entry guiltyEntry = new Entry("Guilty entry", ()=>hiddenEntry.Visible = true);

            sMenu.AddEntry(hiddenEntry);
            sMenu.AddEntry(guiltyEntry);

            engine.Start();
            engine.OnChoice(0);//show the hiddenEntry (first)
            engine.OnChoice(0);//execute the hiddenEntry
            engine.Stop();

            Assert.AreEqual(1, flag);
        }

    }


}
