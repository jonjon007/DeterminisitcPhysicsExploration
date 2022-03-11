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
        PhysObject newObj = new PhysObject(5);
        physWorld.AddObject(newObj);
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
