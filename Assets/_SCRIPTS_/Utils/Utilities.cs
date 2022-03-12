using UnityEngine;
using Unity.Mathematics;
using Unity.Mathematics.FixedPoint;
using System;

namespace SepM.Utils{
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

        public static bool equalsWError(this fp x, fp other, fp errorPercent){
            int sign = x < 0 ? -1 : 1;
            fp maxVal = x*(1+errorPercent*sign);
            fp minVal = x*(1-errorPercent*sign);
            return other >= minVal && other <= maxVal;
        }

        public static fp InvSqrft(this fp x){
            (fp, int) tmp = (0,0);
            tmp.Item1 = x;
            tmp.Item2 = 0x5f3759df - (tmp.Item2 >> 1);
            fp y = tmp.Item1;
            return y * (1.5m - 0.5m * x * y * y);
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
            fp lengthSqrd = vec.lengthSqrd();

            fp3 result = vec/lengthSqrd.Sqrt();

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

        /* Squares the passed FixedPoint vector */
        public static fp3 sqrd(this fp3 v){
            return new fp3(v.x.sqrd(), v.y.sqrd(), v.z.sqrd());
        }
        public static Vector3 toVector3(this fp3 v){
            Vector3 result = new Vector3(
                (float)v.x,
                (float)v.y,
                (float)v.z
            );
            return result;
        }

        // Referenced from: https://github.com/asik/FixedMath.Net/blob/b2adac7713eda01fdd31578dd5a1d15f8f7ba067/src/Fix64.cs#L575-L645
        public static fp Sqrt(this fp x)
        {
            var xl = x.RawValue;
            if (xl < 0)
            {
                // We cannot represent infinities like Single and Double, and Sqrt is
                // mathematically undefined for x < 0. So we just throw an exception.
                throw new ArgumentOutOfRangeException("Negative value passed to Sqrt", "x");
            }

            var num = (ulong)xl;
            var result = 0UL;

            // second-to-top bit
            // var bit = 1UL << (NUM_BITS - 2);
            var bit = 1UL << (64 - 2);

            while (bit > num)
            {
                bit >>= 2;
            }

            // The main part is executed twice, in order to avoid
            // using 128 bit values in computations.
            for (var i = 0; i < 2; ++i)
            {
                // First we get the top 48 bits of the answer.
                while (bit != 0)
                {
                    if (num >= result + bit)
                    {
                        num -= result + bit;
                        result = (result >> 1) + bit;
                    }
                    else
                    {
                        result = result >> 1;
                    }
                    bit >>= 2;
                }

                if (i == 0)
                {
                    // Then process it again to get the lowest 16 bits.
                    // if (num > (1UL << (NUM_BITS / 2)) - 1)
                    if (num > (1UL << (64 / 2)) - 1)
                    {
                        // The remainder 'num' is too large to be shifted left
                        // by 32, so we have to add 1 to result manually and
                        // adjust 'num' accordingly.
                        // num = a - (result + 0.5)^2
                        //       = num + result^2 - (result + 0.5)^2
                        //       = num - result - 0.5
                        num -= result;
                        num = (num << (64 / 2)) - 0x80000000UL;
                        // num = (num << (NUM_BITS / 2)) - 0x80000000UL;
                        result = (result << (64 / 2)) + 0x80000000UL;
                        // result = (result << (NUM_BITS / 2)) + 0x80000000UL;
                    }
                    else
                    {
                        // num <<= (NUM_BITS / 2);
                        num <<= (64 / 2);
                        // result <<= (NUM_BITS / 2);
                        result <<= (64 / 2);
                    }

                    // bit = 1UL << (NUM_BITS / 2 - 2);
                    bit = 1UL << (64 / 2 - 2);
                }
            }
            // Finally, if next bit would have been 1, round the result upwards.
            if (num > result)
            {
                ++result;
            }
            return fp.FromRaw((long)result);
        }
    }
}