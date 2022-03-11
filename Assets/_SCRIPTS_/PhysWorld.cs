using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath.NET;
using Unity.Mathematics.FixedPoint;

public class PhysWorld{
	private List<PhysObject> m_objects = new List<PhysObject>();
	fp3 m_gravity = new fp3(0, -9.81m, 0);
 
    public void AddObject (PhysObject obj) {
        GameObject u_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
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
            fp3 oldPosition = obj.Position;

            // Get combined forces
            fp3 newForce =  oldForce + mass * m_gravity; // apply a force
            fp3 newVelocity = oldVelocity + newForce / mass * dt;
            fp3 newPosition = oldPosition + newVelocity*dt;
			 
            obj.SetVelocity(newVelocity);
            obj.SetPosition(newPosition);
			obj.SetForce(fp3.zero); // reset net force at the end
            Debug.Log(obj.Position);
		}
	}
}
