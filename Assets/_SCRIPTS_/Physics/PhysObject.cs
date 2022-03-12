using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Mathematics.FixedPoint;
using SepM.Utils;

namespace SepM.Physics{
    public struct CollisionPoints {
        public fp3 A; // Furthest point of A into B
        public fp3 B; // Furthest point of B into A
        public fp3 Normal; // B – A normalized
        public fp DepthSqrd;    // Length of B – A
        public bool HasCollision;
    };
    
    public class PhysTransform { // Describes an objects location
        public fp3 Position;
        public fp3 Scale;
        public quaternion Rotation;
        private PhysTransform m_parent;
        private List<PhysTransform> m_children;
        private PhysTransform t_parent;
        /* TODO: Comment */
        public fp3 WorldPosition(){
            fp3 parentPos = fp3.zero;

            if (!(m_parent is null)) {
                parentPos = m_parent.WorldPosition();
            }

            return Position + parentPos;
        }
        public quaternion WorldRotation(){
            
            // TODO: Check if we shoudl be using (1,0,0,0) or (0,0,0,1)
            quaternion parentRot = quaternion.identity;

            if (!(m_parent is null)) {
                parentRot = m_parent.WorldRotation();
            }

            return Rotation.multiply(parentRot);
        }
        /* TODO: Comment */
        public fp3 WorldScale(){
            fp3 parentScale = new fp3(1,1,1);

            if (!(m_parent is null)) {
                parentScale = m_parent.WorldScale();
            }

            return Scale * parentScale;
        }
    };

    public abstract class Collider {
        public abstract CollisionPoints TestCollision(
            PhysTransform transform,
            Collider collider,
            PhysTransform colliderTransform
        );
        public abstract CollisionPoints TestCollision(
            PhysTransform transform,
            SphereCollider sphere,
            PhysTransform sphereTransform
        ); 
        public abstract CollisionPoints TestCollision(
            PhysTransform transform,
            PlaneCollider plane,
            PhysTransform planeTransform
        );
    };

    public class SphereCollider : Collider{
        public fp3 Center;
        public fp Radius;
    
        public SphereCollider(int r){
            Center = fp3.zero;
            Radius = r;
        }
        public override CollisionPoints TestCollision(
            PhysTransform transform,
            Collider collider,
            PhysTransform colliderTransform)
        {
            return collider.TestCollision(colliderTransform, this, transform);
        }
    
        public override CollisionPoints TestCollision(
            PhysTransform transform,
            SphereCollider sphere,
            PhysTransform sphereTransform)
        {
            return algo.FindSphereSphereCollisionPoints(
                this, transform, sphere, sphereTransform);
        }
    
        public override CollisionPoints TestCollision(
            PhysTransform transform,
            PlaneCollider plane,
            PhysTransform planeTransform){
            return algo.FindSpherePlaneCollisionPoints(
                this, transform, plane, planeTransform
            );
        }
    };

    public class PlaneCollider : Collider{
        public fp3 Normal;
        public fp Distance;
    
        public PlaneCollider(fp3 n, fp d){
            Normal = n;
            Distance = d;
        }

        public override CollisionPoints TestCollision(
            PhysTransform transform,
            Collider collider,
            PhysTransform colliderTransform)
        {
            return collider.TestCollision(colliderTransform, this, transform);
        }
    
        public override CollisionPoints TestCollision(
            PhysTransform transform,
            SphereCollider sphere,
            PhysTransform sphereTransform)
        {
            return algo.FindSpherePlaneCollisionPoints(
                sphere, transform, this, sphereTransform);
        }
    
        public override CollisionPoints TestCollision(
            PhysTransform transform,
            PlaneCollider plane,
            PhysTransform planeTransform){
            // return algo.FindPlanePlaneCollisionPoints(
            // 	this, transform, plane, planeTransform
            // );

            // TODO
            return new CollisionPoints();
        }
    };

    /*
    public struct Vec3{
        public Fix64 x;
        public Fix64 y;
        public Fix64 z;

        public Vec3(Fix64 xPos, Fix64 yPos, Fix64 zPos){
            x = xPos;
            y = yPos;
            z = zPos;
        }
        
        public Vec3(double xPos, double yPos, double zPos){
            x = (Fix64)xPos;
            y = (Fix64)yPos;
            z = (Fix64)zPos;
        }

        public Vec3(int xPos, int yPos, int zPos){
            x = (Fix64)xPos;
            y = (Fix64)yPos;
            z = (Fix64)zPos;
        }

        public static Vec3 operator +(Vec3 left, Vec3 right)
            => new Vec3(left.x + right.x, left.y + right.y, left.z + right.z);
        
        public static Vec3 operator *(Fix64 f, Vec3 v)
            => new Vec3(v.x * f, v.y * f, v.z * f);
        public static Vec3 operator *(Vec3 v, Fix64 f)
            => v*f;
        
        public static Vec3 operator /(Vec3 v, Fix64 f)
            => new Vec3(v.x / f, v.y / f, v.z / f);


        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})",
                x.ToString(), y.ToString(), z.ToString());
        }
        // TODO: Turn into static members
        public static Vec3 Up(){
            return new Vec3(0, 1, 0);
        }
        public static Vec3 Right(){
            return new Vec3(1, 0, 0);
        }
        public static Vec3 Forward(){
            return new Vec3(0, 0, 1);
        }
        public static Vec3 Zero = new Vec3(0, 0, 0);
    }
    */
    /* Using as a class since it represents a combination of values and will be mutated often. */
    public class PhysObject {
        public PhysTransform Transform; // struct with 3 floats for x, y, z or i + j + k
        public fp3 Velocity;
        public fp3 Force;
        public fp Mass;
        public Collider coll;
        public PhysObject(int m){
            Transform = new PhysTransform();
            Velocity = new fp3();
            Force = new fp3();
            Mass = m;
        }
        public PhysObject(fp3 pos){
            PhysTransform newTransform = new PhysTransform();
            newTransform.Position = pos;
            Transform = newTransform;

            Velocity = new fp3();
            Force = new fp3();
            Mass = 5;
        }
        public PhysObject(PhysTransform t, fp3 v, fp3 f, fp m){
            Transform = t;
            Velocity = v;
            Force = f;
            Mass = m;
        }
    };
}