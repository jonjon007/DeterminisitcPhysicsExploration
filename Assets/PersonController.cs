using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath.NET;
using Unity.Mathematics.FixedPoint;

public class PersonController : MonoBehaviour
{
    [Header("Set in Inspector")]
    Rigidbody rb;
    public int jumpPower = 400;
    public int moveSpeed = 500;
    public PhysWorld physWorld;
    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody>();

        physWorld = new PhysWorld();

        // PhysObject newObj = new PhysObject(new fp3(0,0,10));
        // newObj.coll = new SphereCollider(5);
        // physWorld.AddObject(newObj);

        PhysObject newObj2 = new PhysObject(new fp3(0,0,10));
        newObj2.coll = new SphereCollider(2);
        physWorld.AddObject(newObj2);

        PhysObject newObj3 = new PhysObject(new fp3(0,2.1m,10));
        newObj3.coll = new PlaneCollider(new fp3(0,1,0), 2);
        physWorld.AddObject(newObj3);

        CollisionPoints collData = newObj2.coll.TestCollision(newObj2.Transform, newObj3.coll, newObj3.Transform);
        bool isTouching = collData.HasCollision;
        Debug.Log(isTouching);
    }

    // Update is called once per frame
    void Update()
    {
        //Move
        rb.AddForce(transform.forward*Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime);
        //Jump
        if(Input.GetKeyDown("space"))
            Jump();
        
        physWorld.Step((fp)Time.deltaTime);
    }

    void Jump(){
        rb.AddForce(Vector3.up*jumpPower);
    }
}
