using CLIM.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
