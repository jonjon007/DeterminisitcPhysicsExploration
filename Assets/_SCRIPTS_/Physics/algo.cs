using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics.FixedPoint;
using SepM.Utils;
using SepM.Physics;

namespace SepM.Physics{
    public static class algo{
        public static CollisionPoints FindSphereSphereCollisionPoints(
            SphereCollider a, PhysTransform ta,
            SphereCollider b, PhysTransform tb)
        {
            fp3 A = a.Center + ta.WorldPosition();
            fp3 B = b.Center + tb.WorldPosition();

            fp Ar = a.Radius * ta.WorldScale().major();
            fp Br = b.Radius * tb.WorldScale().major();

            fp3 AtoB = B - A;
            fp3 BtoA = A - B;

            if (AtoB.lengthSqrd() > (Ar + Br).sqrd()) {
                return CollisionPoints.noCollision;
            }

            A += AtoB.normalized() * Ar;
            B += BtoA.normalized() * Br;

            AtoB = B - A;

            return new CollisionPoints{ 
                A = A,
                B = B,
                Normal = AtoB.normalized(),
                DepthSqrd = AtoB.lengthSqrd(),
                HasCollision = true
            };
        }

        // Transforms dont work for plane
        // TODO: Assumes plane is infinite; add bounds calculations
        public static CollisionPoints FindSpherePlaneCollisionPoints(
            SphereCollider a, PhysTransform ta,
            PlaneCollider  b, PhysTransform tb)
        {
            fp3 A  = a.Center + ta.WorldPosition();
            fp Ar = a.Radius * ta.WorldScale().major();

            fp3 N = b.Normal.multiply(tb.WorldRotation());
            N.normalize();
            
            fp3 P = N * b.Distance + tb.WorldPosition();

            fp d = (A - P).dot(N); // distance from center of sphere to plane surface

            if (d > Ar) {
                return CollisionPoints.noCollision;
            }
            
            fp3 B = A - N * d;
                    A = A - N * Ar;

            fp3 AtoB = B - A;

            return new CollisionPoints{ 
                A = A,
                B = B, 
                Normal = AtoB.normalized(), 
                DepthSqrd = AtoB.lengthSqrd(),
                HasCollision = true
            };
        }

        public static CollisionPoints FindSphereCapsuleCollisionPoints(
        SphereCollider  a, PhysTransform ta,
        CapsuleCollider b, PhysTransform tb)
        {
            fp Bhs = 1.0m;
            fp Brs = 1.0m;

            fp3 s = tb.WorldScale();
            // TODO: Will need to verify this condition
            // Right
            if (b.Direction.Equals(new fp3(1,0,0))) {
                Bhs = s.x;
                Brs = new fp2(s.y, s.z).major();
            }
            // Up
            else if (b.Direction.Equals(new fp3(0,1,0))) {
                Bhs = s.y;
                Brs = new fp2(s.x, s.z).major();
            }
            // Forward
            else if (b.Direction.Equals(new fp3(0,0,1))) {
                Bhs = s.z;
                Brs = new fp2(s.x, s.y).major();
            }

            fp3 offset = b.Direction.multiply(tb.WorldRotation()) * (b.Height * Bhs / 2 - b.Radius * Brs);

            fp3 A = a.Center          + ta.WorldPosition();
            fp3 B = b.Center - offset + tb.WorldPosition();
            fp3 C = b.Center + offset + tb.WorldPosition(); // might not be correct
            
            fp Ar = a.Radius * ta.WorldScale().major();
            fp Br = b.Radius * Brs;

            fp3 BtoA = A - B;
            fp3 BtoC = C - B;

            fp d = BtoC.normalized().dot(BtoA).clamp(0, BtoC.lengthSqrd().sqrt());
            fp3 D = B + BtoC.normalized() * d;

            fp3 AtoD = D - A;
            fp3 DtoA = A - D;

            if (AtoD.lengthSqrd() > (Ar + Br).sqrd()) {
                return CollisionPoints.noCollision;
            }

            A += AtoD.normalized() * Ar;
            D += DtoA.normalized() * Br;

            AtoD = D - A;

            return new CollisionPoints(){
                A = A,
                B = D,
                Normal = AtoD.normalized(),
                DepthSqrd = AtoD.lengthSqrd(),
                HasCollision = true
            };
        }
    }
}
/*
        // Swaps

        void SwapPoints(
            ManifoldPoints& points)
        {
            iw::fp3 T = points.A;
            points.A = points.B;
            points.B = T;

            points.Normal = -points.Normal;
        }

        ManifoldPoints FindPlaneSphereMaifoldPoints(
            PlaneCollider*  a, PhysTransform* ta, 
            SphereCollider* b, PhysTransform* tb)
        {
            ManifoldPoints points = FindSpherePlaneMaifoldPoints(b, tb, a, ta);
            SwapPoints(points);

            return points;
        }

        ManifoldPoints FindCapsuleSphereMaifoldPoints(
            CapsuleCollider* a, PhysTransform* ta,
            SphereCollider*  b, PhysTransform* tb)
        {
            ManifoldPoints points = FindSphereCapsuleMaifoldPoints(b, tb, a, ta);
            SwapPoints(points);

            return points;
        }
    }
}
*/