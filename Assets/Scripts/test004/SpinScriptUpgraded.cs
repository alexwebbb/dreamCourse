using UnityEngine;
using System.Collections;

public class SpinScriptUpgraded : MonoBehaviour {

    public float power;
    public int bounceCount;

    Rigidbody rb;
    GameObject launcher;
    LaunchControllerWRotator launchCon;
    Vector3[] bouncePositions;

	void Start () {

        // get the rigidbody of the spinning object
        rb = GetComponent<Rigidbody>();

        // find the launcher for the object and capture the controller. this would need to change in a multiplayer context 
        launcher = GameObject.FindGameObjectWithTag("Launcher");
        launchCon = launcher.GetComponent<LaunchControllerWRotator>();

        // initialize the bounce positions array
        bouncePositions = new Vector3[2];

	}
    
    void OnCollisionEnter(Collision collision) {


        // caputures the current bounce position and the very last one. used to determine the bounce direction
        bouncePositions[1] = bouncePositions[0];
        bouncePositions[0] = rb.position;

    }

    void OnCollisionExit(Collision collision) {

        // make sure not to consider the very first collision, which is with the ground, as well as any bounces after the second
        if (bounceCount > 0 && bounceCount < 2) {

            // calculate the direction spin force will be applied in using the bounce positions array
            Vector3 spinDirection = bouncePositions[0] - bouncePositions[1];
            Vector3.Normalize(spinDirection);
            
            // because unity angular velocity is very finicky and doesn't tell you direction of spin, the power of the spin correction is determined by the original spin power that is entered into the launch controller
            if (launchCon.spinForce.x > 0) {

                // force is applied. make sure to always use impulse for the force mode!
                rb.AddForce(spinDirection * launchCon.spinForce.x * launchCon.spinForceFactor, ForceMode.Impulse);
                
            } else if(launchCon.spinForce.x < 0) {
                
                // force is applied in the reverse direction
                rb.AddForce(spinDirection * launchCon.spinForce.x * launchCon.spinForceFactor, ForceMode.Impulse);
                
            }
        }

        // increment the bounce count. used to establish the range that spin adjustments occur in
        bounceCount++;

    }
}
