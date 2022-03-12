using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SepM.Utils;
using Unity.Mathematics.FixedPoint;

public class UtilitiesTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void UtilitiesTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [Test]
    public void TestMajorNormal(){
        // Normal condition
        fp3 yMajor = new fp3(3,11,.5m);
        fp expected = 11;

        fp actual = yMajor.major();
        Assert.That(expected == actual, "Incorrect major calculation");
    }

    [Test]
    public void TestMajorTie(){
        // Tie
        fp3 tieMajor = new fp3(0,5,5);
        fp expected = 5;

        fp actual = tieMajor.major();
        Assert.AreEqual(expected, actual, "Incorrect major calculation");
    }

    [Test]
    public void TestMajorZero(){
        // Tie
        fp3 zeroMajor = new fp3(0,0,0);
        fp expected = 0;

        fp actual = zeroMajor.major();
        Assert.AreEqual(expected, actual, "Incorrect major calculation");
    }
}
