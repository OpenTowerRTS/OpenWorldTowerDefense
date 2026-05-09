using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game;
using UnityEditor.VersionControl;
public class CalculatorUnitTest
{
    // A Test behaves as an ordinary method
    private Calculator _calculator;
    [SetUp]
    public void Setup() => _calculator = new Calculator();
    [Test]
    public void AddTest() => Assert.AreEqual(2, _calculator.Add(1, 1));

    [Test]
    public void SubtractTest() => Assert.AreEqual(0, _calculator.Sub(1, 1));

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CalculatorUnitTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
