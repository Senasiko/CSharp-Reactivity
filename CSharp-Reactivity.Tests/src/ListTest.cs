using Microsoft.VisualStudio.TestTools.UnitTesting;
using R3;
using Reactivity;

namespace CSharp_Reactivity.Tests;

[TestClass]
public class ListTest
{

    [TestMethod]
    public void Normal()
    {
        var list = new ReactiveList<int>();
        var computed1 = new Computed<int>(() => list.Count);
        Assert.AreEqual(0, computed1.Value);
        list.Add(1);
        Assert.AreEqual(1, computed1.Value);
        Assert.AreEqual(1, list[0]);
    }

    [TestMethod]
    public void Operator()
    {
        var list = new ReactiveList<int>();
        var computed1 = new Computed<int>(() => list.Count);
        list.Add(1);
        Assert.AreEqual(1, computed1.Value);
        
        list.Remove(1);
        Assert.AreEqual(0, computed1.Value);
        
        list.Add(1);
        list.Add(2);
        list.Clear();
        Assert.AreEqual(0, computed1.Value);
        
    }
    
    [TestMethod]
    public void Event()
    {
        var list = new ReactiveList<int>();
        ReactiveListChangeEvent<int> result = new();
        list.OnChange.Subscribe((e) =>
        {
            result = e;
        });
        
        list.Add(1);
        Assert.AreEqual(1, result.Item);
        Assert.AreEqual(ReactiveListChangeType.Add, result.Type);
        
        list.Remove(1);
        Assert.AreEqual(1, result.Item);
        Assert.AreEqual(ReactiveListChangeType.Remove, result.Type);
        
        list.Clear();
        Assert.AreEqual(ReactiveListChangeType.Clear, result.Type);
    }
}