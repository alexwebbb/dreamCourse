﻿using UnityEngine;
using System.Collections.Generic;

public class BounceController : MonoBehaviour, IBallComponent {

    public float power;
    public int bounceLimit = 3;
    public int bounceCount;

    Rigidbody rb;
    LaunchController lc;
    Character character;
    Vector3[] bouncePositions = new Vector3[2];

    // this is for when the ball interacts with colliders that have a game role
    public Character GetCharacter {
        get {
            if (character == null) character = GetComponentInParent<Character>();
            return character;
        }
    }

    public void Initialize(Rigidbody _rb, LaunchController _lc) {
        rb = _rb;
        lc = _lc;
    }

    void Start () {
        // initialize the bounce positions array
        bouncePositions = new Vector3[2];
	}
    
    void OnCollisionEnter(Collision collision) {

        // explosive force when two player objects collide
        if(collision.gameObject.tag == "Player") {
            // may want to give this a public value at some point
            rb.AddExplosionForce(300f, collision.transform.position, 0);
        }

        // caputures the current bounce position and the very last one. used to determine the bounce direction
        if (rb != null) {
            bouncePositions[1] = bouncePositions[0];
            bouncePositions[0] = rb.position;
        }
    }

    void OnCollisionExit(Collision collision) {

        // make sure not to consider the very first collision, which is with the ground, as well as any bounces after the second
        if (bounceCount > 0 && bounceCount < bounceLimit) {
            
            // calculate the direction spin force will be applied in using the bounce positions array
            Vector3 spinDirection = bouncePositions[0] - bouncePositions[1];
            Vector3.Normalize(spinDirection);


            // this is adds to the vertical force of the bounce. SHOULD work even when hitting things from underneath
            rb.AddForce(Vector3.up * rb.velocity.y * lc.bouncePercent, ForceMode.VelocityChange);

            // because unity angular velocity is very finicky and doesn't tell you direction of spin, the power of the spin correction is determined by the original spin power that is entered into the launch controller
            if (lc.spinForce.x > 0 || lc.spinForce.x < 0) {

                // force is applied. make sure to always use velociy for the force mode!
                rb.AddForce(spinDirection * lc.spinForce.x * lc.spinForceFactor, ForceMode.VelocityChange);

                // torque is applied. keeps the object spinning in the correct direction visibly.
                rb.AddRelativeTorque(lc.spinForce, ForceMode.VelocityChange);
                
            } 
        }

        // increment the bounce count. used to establish the range that spin adjustments occur in
        if(rb != null) bounceCount++;

    }
}
