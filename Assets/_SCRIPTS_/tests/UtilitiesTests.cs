using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SepM.Utils;
using Unity.Mathematics;
using Unity.Mathematics.FixedPoint;

public class UtilitiesTests
{
    [Test]
    public void TestCross(){
        fp3 v1 = new fp3(1,0,0);
        fp3 v2 = new fp3(0,1,0);
        fp3 expected = new fp3(0,0,1);
        
        fp3 actual = v1.cross(v2);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestDot(){
        fp3 v1 = new fp3(1,0,0);
        fp3 v2 = new fp3(-1,0,0);
        fp expected = -1;
        
        fp actual = v1.dot(v2);
        Assert.AreEqual(expected, actual);
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
    public void TestMultiplyQQ(){
        quaternion q1 = new quaternion(1,2,3,1);
        quaternion q2 = new quaternion(3,2,1,1);
        quaternion expected = new quaternion(0,12,0,-9);

        quaternion actual = q1.multiply(q2);
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void TestMultiplyVQ(){
        fp error = .01m;
        quaternion q = new quaternion(0.382683f,0,0,0.92388f);
        fp3 v = new fp3(0,0,1);
        fp3 expected = new fp3(0, -0.7071063400800001m, 0.7071079759110002m);

        fp3 actual = v.multiply(q);
        Assert.That(
            expected.x.equalsWError(actual.x, error)
            && expected.y.equalsWError(actual.y, error)
            && expected.z.equalsWError(actual.z, error)
        );
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
    public void TestNormalize(){
        fp error = .01m;
        fp3 vec = new fp3(1,-2,3);
        fp3 expected = new fp3(0.2672612419124244m, -0.5345224838248488m, 0.8017837257372732m);

        vec.normalize();
        Assert.That(
            expected.x.equalsWError(vec.x, error)
            && expected.y.equalsWError(vec.y, error)
            && expected.z.equalsWError(vec.z, error)
        );
    }

    [Test]
    public void TestSqrd(){
        fp num = 4;
        fp expected = 16;

        fp actual = num.sqrd();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSqrdNegative(){
        fp num = -10;
        fp expected = 100;

        fp actual = num.sqrd();
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void TestSqrt(){
        fp num = 16;
        fp expected = 4;

        fp actual = num.sqrt();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSqrtZero(){
        fp num = -1;
        Assert.Throws<ArgumentOutOfRangeException>(() => num.sqrt());
    }

    [Test]
    public void TestToVector3(){
        fp3 f3 = new fp3(1,2,3);
        Vector3 expected = new Vector3(1,2,3);

        Vector3 actual = f3.toVector3();
        Assert.AreEqual(expected, actual);
    }
}
