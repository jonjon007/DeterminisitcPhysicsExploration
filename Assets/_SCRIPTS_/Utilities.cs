using Unity.Mathematics.FixedPoint;
using System;

namespace Utils{
    public static class Utilities
    {
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

        /* Squares the passed FixedPoint number */
        public static fp sqrd(this fp x){
            return x*x;
        }
    }
}