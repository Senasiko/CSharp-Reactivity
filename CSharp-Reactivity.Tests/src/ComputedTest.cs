using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using R3;
using Reactivity;

namespace CSharp_Reactivity.Tests;

[TestClass]
public class ComputedTest
{

    [TestMethod]
    public void Normal()
    {
        var computed1 = new Computed<int>(() => 1);
        Assert.AreEqual(1, computed1.Value);
    }

    [TestMethod]
    public void WithRef()
    {
        var ref1 = new Ref<int>(1);
        var computed1 = new Computed<int>(() => ref1.Value);
        ref1.Value = 2;
        Assert.AreEqual(2, computed1.Value);
        
        var ref2 = new Ref<int>(1);
        var computed2 = new Computed<int>(() => ref1.Value + ref2.Value);
        ref2.Value = 2;
        Assert.AreEqual(4, computed2.Value);
        ref1.Value = 3;
        Assert.AreEqual(5, computed2.Value);
    }
    
    [TestMethod]
    public void NestComputed()
    {
        var ref1 = new Ref<int>(1);
        var computed1 = new Computed<int>(() => ref1.Value);
        var computed2 = new Computed<int>(() => computed1.Value);
        ref1.Value = 2;
        Assert.AreEqual(2, computed1.Value);
        Assert.AreEqual(2, computed2.Value);
    }
    
    [TestMethod]
    public void Event()
    {
        var ref1 = new Ref<int>(1);
        var computed1 = new Computed<int>(() => ref1.Value);
        var a = computed1.Value;
        ComputedChangeEvent<int> result = new();
        computed1.OnChange.Subscribe((e) =>
        {
            result = e;
        });
        ref1.Value = 2;
        Assert.AreEqual(1, result.OldValue);
        Assert.AreEqual(2, computed1.Value);
    }
}