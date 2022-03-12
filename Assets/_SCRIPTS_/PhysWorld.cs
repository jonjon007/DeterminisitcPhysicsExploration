using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics.FixedPoint;
using SepM.Physics;
using SepM.Utils;

public class PhysWorld{
	private List<PhysObject> m_objects = new List<PhysObject>();
	fp3 m_gravity = new fp3(0, -9.81m, 0);
 
    public void AddObject (PhysObject obj) {
        GameObject u_obj;
        if(obj.coll is SepM.Physics.SphereCollider){
            u_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float sphRadius = (float)((SepM.Physics.SphereCollider)obj.coll).Radius;
            u_obj.transform.localScale = Vector3.one*sphRadius;
        }
        else{
            u_obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            PlaneCollider c = (PlaneCollider)obj.coll;
            Vector3 planeDir = c.Normal.toVector3();
            float scale = (float)c.Distance/10;
            u_obj.transform.Rotate(planeDir);
            u_obj.transform.localScale = Vector3.one*scale;
        }
        PhysObjController u_objCont = u_obj.AddComponent<PhysObjController>();
        u_objCont.setPhysObject(obj);
        
        m_objects.Add(obj);
    }
	public void RemoveObject(PhysObject obj) { /* ... */ }
 
    // TODO: Work with permissions
	public void Step(fp dt){
		foreach(PhysObject obj in m_objects) {
            fp mass = obj.Mass;
            fp3 oldForce = obj.Force;
            fp3 oldVelocity = obj.Velocity;
            fp3 oldPosition = obj.Transform.Position;

            // Get combined forces
            fp3 newForce =  oldForce + mass * m_gravity; // apply a force
            fp3 newVelocity = oldVelocity + newForce / mass * dt;
            fp3 newPosition = oldPosition + newVelocity*dt;
			 
            obj.Velocity = (newVelocity);
            obj.Transform.Position = (newPosition);
			obj.Force = (fp3.zero); // reset net force at the end
		}
	}
}
