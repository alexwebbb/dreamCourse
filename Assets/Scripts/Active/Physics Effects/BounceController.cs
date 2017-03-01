using UnityEngine;
using System.Collections;

public class BounceController : MonoBehaviour {

    public float power;
    public int bounceLimit = 3;
    public int bounceCount;

    Rigidbody rb;
    LaunchController launchCon;
    Vector3[] bouncePositions;

	void Start () {

        // get the rigidbody of the spinning object
        rb = GetComponent<Rigidbody>();

        // find the launcher for the object and capture the controller. this would need to change in a multiplayer context 
        launchCon = GameObject.FindGameObjectWithTag("Launcher").GetComponent<LaunchController>();

        // initialize the bounce positions array
        bouncePositions = new Vector3[2];

	}
    
    void OnCollisionEnter(Collision collision) {

        // There needs to be an event here that reports the position of the collision to the level controller
        // something like if(bounceCount > 0) 

        // caputures the current bounce position and the very last one. used to determine the bounce direction
        bouncePositions[1] = bouncePositions[0];
        bouncePositions[0] = rb.position;

    }

    void OnCollisionExit(Collision collision) {

        // make sure not to consider the very first collision, which is with the ground, as well as any bounces after the second
        if (bounceCount > 0 && bounceCount < bounceLimit) {
            
            // calculate the direction spin force will be applied in using the bounce positions array
            Vector3 spinDirection = bouncePositions[0] - bouncePositions[1];
            Vector3.Normalize(spinDirection);

            // this is adds to the vertical force of the bounce. SHOULD work even when hitting things from underneath
            rb.AddForce(Vector3.up * rb.velocity.y * launchCon.bouncePercent, ForceMode.VelocityChange);

            // because unity angular velocity is very finicky and doesn't tell you direction of spin, the power of the spin correction is determined by the original spin power that is entered into the launch controller
            if (launchCon.spinForce.x > 0 || launchCon.spinForce.x < 0) {

                // force is applied. make sure to always use velociy for the force mode!
                rb.AddForce(spinDirection * launchCon.spinForce.x * launchCon.spinForceFactor, ForceMode.VelocityChange);

                // torque is applied. keeps the object spinning in the correct direction visibly.
                rb.AddRelativeTorque(launchCon.spinForce, ForceMode.VelocityChange);
                
            } 
        }

        // increment the bounce count. used to establish the range that spin adjustments occur in
        bounceCount++;

    }
}
