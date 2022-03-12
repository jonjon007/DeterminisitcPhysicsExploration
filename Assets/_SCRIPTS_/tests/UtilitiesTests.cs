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
    public void TestEqualsWErrorOutOfRange()
    {
        fp answer = 100;
        fp guesss = 110;
        fp error = .08m;
        bool expected = false;

        bool actual = answer.equalsWError(guesss, error);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestEqualsWErrorInRange()
    {
        fp answer = -100;
        fp guesss = -105;
        fp error = .08m;
        bool expected = true;

        bool actual = answer.equalsWError(guesss, error);

        Assert.AreEqual(expected, actual);
    }


    [Test]
    public void TestLengthSqrd(){
        fp3 vec1 = new fp3(1,-2,3);
        fp expected = 14;

        fp actual = vec1.lengthSqrd();
        Assert.AreEqual(expected, actual);
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
    
    [Test]
    public void TestNormalizedOneDir(){
        fp3 vec1 = new fp3(123,0,0);
        fp3 expected = new fp3(1, 0, 0);

        fp3 actual = vec1.normalized();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestNormalizedMultiDir(){
        fp error = .01m;
        fp3 vec1 = new fp3(1,-2,3);
        fp3 expected = new fp3(0.2672612419124244m, -0.5345224838248488m, 0.8017837257372732m);

        fp3 actual = vec1.normalized();
        Assert.That(
            expected.x.equalsWError(actual.x, error)
            && expected.y.equalsWError(actual.y, error)
            && expected.z.equalsWError(actual.z, error)
        );
    }

    [Test]
    public void TestSqrt(){
        fp num = 16;
        fp expected = 4;

        fp actual = num.Sqrt();
        Assert.AreEqual(expected, actual);
    }
}
