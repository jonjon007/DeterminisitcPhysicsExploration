using UnityEngine;
using Unity.Mathematics;
using Unity.Mathematics.FixedPoint;
using System;

namespace Utils{
    public static class Utilities
    {
        public static fp3 cross(this fp3 va, fp3 vb){
            return new fp3(
                va.y * vb.z - va.z * vb.y,
                va.z * vb.x - va.x * vb.z,
                va.x * vb.y - va.y * vb.x);
        }

        public static fp dot(this fp3 va, fp3 vb){
            return va.x * vb.x + va.y * vb.y + va.z * vb.z;
        }
        
        /* Returns the vector length squared, avoiding the slow operation */
        public static fp lengthSqrd(this fp3 vec){
            return vec.x * vec.x + vec.y * vec.y + vec.z * vec.z;
        }

        public static fp major(this fp3 vec){
            fp major = vec.x;
            if (Math.Abs(vec.y) > Math.Abs(major)) major = vec.y;
            if (Math.Abs(vec.z) > Math.Abs(major)) major = vec.z;

            return major;
        }
        
        /* Returns the vector length squared, avoiding the slow operation */
        public static void normalize(ref this fp3 vec){
            fp3 normalized = vec.normalized();
            vec = normalized;
        }

        /* Returns the vector length squared, avoiding the slow operation */
        public static fp3 normalized(this fp3 vec){
            fp major = vec.major();
            
            // Check for zero value
            if(major == 0)
                return vec;

            fp3 result = new fp3(
                vec.x/major,
                vec.y/major,
                vec.z/major
            );
            return result;
        }

        public static fp3 multiply(this fp3 v, quaternion q){
            // TODO: Be careful of this cast. It may not be deterministic!
            fp3 u = new fp3((fp)q.value.x, (fp)q.value.y, (fp)q.value.z);
            fp s = (fp)q.value.w;

            return v + ((u.cross(v) * s) + u.cross(u.cross(v))) * 2.0m;
        }

        public static quaternion multiply(this quaternion qa, quaternion qb){
            quaternion result = new quaternion();
            result.value.w = qa.value.w * qb.value.w - qa.value.x * qb.value.x - qa.value.y * qb.value.y - qa.value.z * qb.value.z;
            result.value.x = qa.value.w * qb.value.x + qa.value.x * qb.value.w + qa.value.y * qb.value.z - qa.value.z * qb.value.y;
            result.value.y = qa.value.w * qb.value.y + qa.value.y * qb.value.w + qa.value.z * qb.value.x - qa.value.x * qb.value.z;
            result.value.z = qa.value.w * qb.value.z + qa.value.z * qb.value.w + qa.value.x * qb.value.y - qa.value.y * qb.value.x;
            return result;
        }

        /* Squares the passed FixedPoint number */
        public static fp sqrd(this fp x){
            return x*x;
        }

        public static Vector3 toVector3(this fp3 v){
            Vector3 result = new Vector3(
                (float)v.x,
                (float)v.y,
                (float)v.z
            );
            return result;
        }
    }
}