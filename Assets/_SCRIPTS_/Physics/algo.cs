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
                return new CollisionPoints{ 
                    A = fp3.zero,
                    B = fp3.zero, 
                    Normal = fp3.zero, 
                    DepthSqrd = 0,
                    HasCollision = false
                };
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

            // TODO: Continue here
            fp3 N = b.Normal.multiply(tb.WorldRotation());
            N.normalize();
            
            fp3 P = N * b.Distance + tb.WorldPosition();

            fp d = (A - P).dot(N); // distance from center of sphere to plane surface

            if (d > Ar) {
                return new CollisionPoints{ 
                    A = fp3.zero,
                    B = fp3.zero, 
                    Normal = fp3.zero, 
                    DepthSqrd = 0,
                    HasCollision = false
                };
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
    }
}
/*
        ManifoldPoints FindSphereCapsuleMaifoldPoints(
            SphereCollider*  a, PhysTransform* ta,
            CapsuleCollider* b, PhysTransform* tb)
        {
            fp Bhs = 1.0f;
            fp Brs = 1.0f;

            fp3 s = tb.WorldScale();
            if (b.Direction == fp3::unit_x) {
                Bhs = s.x;
                Brs = vector2(s.y, s.z).major();
            }

            else if (b.Direction == fp3::unit_y) {
                Bhs = s.y;
                Brs = vector2(s.x, s.z).major();
            }

            else if (b.Direction == fp3::unit_z) {
                Bhs = s.z;
                Brs = vector2(s.x, s.y).major();
            }

            fp3 offset = b.Direction * tb.WorldRotation() * (b.Height * Bhs / 2 - b.Radius * Brs);

            fp3 A = a.Center          + ta.WorldPosition();
            fp3 B = b.Center - offset + tb.WorldPosition();
            fp3 C = b.Center + offset + tb.WorldPosition(); // might not be correct
            
            fp Ar = a.Radius * ta.WorldScale().major();
            fp Br = b.Radius * Brs;

            fp3 BtoA = A - B;
            fp3 BtoC = C - B;

            fp   d = iw::clamp(BtoC.normalized().dot(BtoA), 0.0f, BtoC.length());
            fp3 D = B + BtoC.normalized() * d;

            fp3 AtoD = D - A;
            fp3 DtoA = A - D;

            if (AtoD.length() > Ar + Br) {
                return {
                    0, 0,
                    0,
                    0,
                    false
                };
            }

            A += AtoD.normalized() * Ar;
            D += DtoA.normalized() * Br;

            AtoD = D - A;

            return {
                A, D,
                AtoD.normalized(),
                AtoD.length(),
                true
            };
        }

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