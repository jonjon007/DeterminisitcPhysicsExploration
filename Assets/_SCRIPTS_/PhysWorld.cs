using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics.FixedPoint;
using SepM.Physics;
using SepM.Utils;

public class PhysWorld{
	private List<PhysObject> m_objects = new List<PhysObject>();
	private List<Solver> m_solvers = new List<Solver>();
 
    public void AddObject (PhysObject obj) {
        GameObject u_obj;
        if(obj.coll is SepM.Physics.SphereCollider){
            u_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float sphRadius = (float)((SepM.Physics.SphereCollider)obj.coll).Radius*2;
            u_obj.transform.localScale = Vector3.one*sphRadius;
        }
        else if(obj.coll is SepM.Physics.CapsuleCollider){
            u_obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            float capRadius = (float)((SepM.Physics.CapsuleCollider)obj.coll).Radius*2;
            float capHeight = (float)((SepM.Physics.CapsuleCollider)obj.coll).Height;
            u_obj.transform.localScale = new Vector3(capRadius, capHeight, capRadius);
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
	public void RemoveObject(PhysObject obj) { /* TODO */ }
 
    public void AddSolver(Solver solver) { m_solvers.Add(solver); }
	public void RemoveSolver(Solver solver) { /* TODO */ }

    // TODO: Work with permissions
	public void Step(fp dt){
        ResolveCollisions(dt);

		foreach(PhysObject obj in m_objects) {
            fp mass = 1/obj.InverseMass;
            fp3 oldForce = obj.Force;
            fp3 oldVelocity = obj.Velocity;
            fp3 oldPosition = obj.Transform.Position;

            // Get combined forces
            fp3 newForce =  oldForce + mass * obj.Gravity; // apply a force
            fp3 newVelocity = oldVelocity + newForce / mass * dt;
            fp3 newPosition = oldPosition + newVelocity*dt;
			 
            obj.Velocity = (newVelocity);
            obj.Transform.Position = (newPosition);
			obj.Force = (fp3.zero); // reset net force at the end
		}
	}

    void ResolveCollisions(fp dt){
		List<PhysCollision> collisions = new List<PhysCollision>();
        // TODO: Work on that efficiency
		foreach (PhysObject a in m_objects) {
			foreach (PhysObject b in m_objects) {
				if (a == b) break;

                // Check if a collider is assigned
				if (a.coll is null || b.coll is null){
					continue;
				}
 
				CollisionPoints points = a.coll.TestCollision(
					a.Transform,
					b.coll,
					b.Transform);
 
				if (points.HasCollision) {
					collisions.Add(
                        new PhysCollision{
                            ObjA = a,
                            ObjB = b,
                            Points = points
                        }
                    );
				}
			}
		}
 
		foreach(Solver solver in m_solvers) {
			solver.Solve(collisions, dt);
		}
 	}
}
