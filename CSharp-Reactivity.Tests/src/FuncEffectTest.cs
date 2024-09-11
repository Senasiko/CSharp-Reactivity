using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reactivity;

namespace CSharp_Reactivity.Tests;

[TestClass]
public class FuncEffectTest
{
    
    [TestMethod]
    public void Normal()
    {        
        var ref1 = new Ref<int>(1);
        var count = 0;
        var fn = new FuncEffect<int>(() => count = ref1.Value);
        fn.Run();
        ref1.Value = 2;
        Assert.AreEqual(2, count);
    }
}