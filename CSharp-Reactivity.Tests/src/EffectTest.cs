using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reactivity;

namespace CSharp_Reactivity.Tests;

class EffectScopeTest : Effect
{
    public int count = 0;
    public override void Trigger()
    {
        count += 1;
    }
}

[TestClass]
public class EffectTest
{
    [TestMethod]
    public void EffectManager()
    {
        var ref1 = new Ref<int>(1);
        var computed1 = new Computed<int>(() => ref1.Value);
        var a = computed1.Value;
        Assert.AreEqual(1, ref1.EffectManager.Count);
        
        computed1.Dispose();
        Assert.AreEqual(0, ref1.EffectManager.Count);
        
        ref1.Value = 2;
        Assert.AreEqual(1, computed1.Value);
    }

    [TestMethod]
    public void EffectScope()
    {
        var ref1 = new Ref<int>(1);
        var et = new EffectScopeTest();
        using (new EffectScope(et))
        {
            var a = ref1.Value;
        }
        ref1.Value = 2;
        Assert.AreEqual(1, et.count);
    }

    [TestMethod]
    public void NestEffectScope()
    {
        var ref1 = new Ref<int>(1);
        var et = new EffectScopeTest();
        var ref2 = new Ref<int>(1);
        var et2 = new EffectScopeTest();
        using (new EffectScope(et))
        {
            var a = ref1.Value;
            using (new EffectScope(et2))
            {
                var b = ref2.Value;
            }
        }
        
        ref2.Value = 2;
        Assert.AreEqual(0, et.count);
        Assert.AreEqual(1, et2.count);
        
        ref1.Value = 2;
        Assert.AreEqual(1, et.count);
        Assert.AreEqual(1, et2.count);
    }
}