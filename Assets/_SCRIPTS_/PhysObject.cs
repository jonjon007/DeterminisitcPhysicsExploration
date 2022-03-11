using UnityEngine;
using FixMath.NET;
using Unity.Mathematics.FixedPoint;

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
	public fp3 Position; // struct with 3 floats for x, y, z or i + j + k
	public fp3 Velocity;
	public fp3 Force;
	public fp Mass;
    public PhysObject(int m){
        Position = new fp3();
        Velocity = new fp3();
        Force = new fp3();
        Mass = m;
    }
    public PhysObject(fp3 p, fp3 v, fp3 f, fp m){
        Position = p;
        Velocity = v;
        Force = f;
        Mass = m;
    }
    public void SetPosition(fp3 p){ Position = p; }
    public void SetVelocity(fp3 v){ Velocity = v; }
    public void SetForce(fp3 f){ Force = f; }
};