using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using R3;
using Reactivity;

namespace CSharp_Reactivity.Tests;

[TestClass]
public class RefTest
{

    [TestMethod]
    public void Normal()
    {
        var ref1 = new Ref<int>(1);
        ref1.Value = 2;
        Assert.AreEqual(2, ref1.Value);
    }

    [TestMethod]
    public void Event()
    {
        var ref1 = new Ref<int>(1);
        ReactiveChangeEvent<int> result = new();
        ref1.OnChange.Subscribe((e) =>
        {
            result = e;
        });
        ref1.Value = 2;
        Assert.AreEqual(2, result.Value);
        Assert.AreEqual(1, result.OldValue);
    }
}