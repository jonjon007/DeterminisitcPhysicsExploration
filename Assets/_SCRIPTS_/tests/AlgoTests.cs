using NUnit.Framework;
using Unity.Mathematics.FixedPoint;
using SepM.Physics;

public class AlgoTests
{
    [Test]
    public void SphereSphereCollision_SamePos(){
        SphereCollider a = new SphereCollider(1);
        SphereCollider b = new SphereCollider(2);
        PhysTransform ta = new PhysTransform(new fp3(0,0,0));
        PhysTransform tb = new PhysTransform(new fp3(0,0,0));
        bool expected = true;

        CollisionPoints cp = a.TestCollision(ta, b, tb);
        Assert.AreEqual(expected, cp.HasCollision);
    }

    [Test]
    public void SphereSphereCollision_Overlap(){
        SphereCollider a = new SphereCollider(1);
        SphereCollider b = new SphereCollider(2);
        PhysTransform ta = new PhysTransform(new fp3(-1,1,0));
        PhysTransform tb = new PhysTransform(new fp3(1,0,0));
        bool expected = true;

        CollisionPoints cp = a.TestCollision(ta, b, tb);
        Assert.AreEqual(expected, cp.HasCollision);
    }

    [Test]
    public void SphereSphereCollision_Edge(){
        SphereCollider a = new SphereCollider(1);
        SphereCollider b = new SphereCollider(1);
        PhysTransform ta = new PhysTransform(new fp3(-1,0,0));
        PhysTransform tb = new PhysTransform(new fp3(1,0,0));
        bool expected = true;

        CollisionPoints cp = a.TestCollision(ta, b, tb);
        Assert.AreEqual(expected, cp.HasCollision);
    }

    [Test]
    public void SphereSphereCollision_NotTouching(){
        SphereCollider a = new SphereCollider(1);
        SphereCollider b = new SphereCollider(1);
        PhysTransform ta = new PhysTransform(new fp3(-1.1m,0,0));
        PhysTransform tb = new PhysTransform(new fp3(1,0,0));
        bool expected = false;

        CollisionPoints cp = a.TestCollision(ta, b, tb);
        Assert.AreEqual(expected, cp.HasCollision);
    }
}
