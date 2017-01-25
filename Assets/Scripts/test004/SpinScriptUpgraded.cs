using UnityEngine;
using System.Collections;

public class SpinScriptUpgraded : MonoBehaviour {

    // public float cutoff;
    public float power;
    Vector3 entranceVector;
    Vector3 exitVector;
    bool collisionActive;
    Rigidbody rb;
    GameObject launcher;
    LaunchControllerWRotator launchCon;

    GameObject player;


    //
    public int bounceCount;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        launcher = GameObject.FindGameObjectWithTag("Launcher");
        launchCon = launcher.GetComponent<LaunchControllerWRotator>();


	}

    /*
	// Update is called once per frame
	void FixedUpdate () {
        if (!collisionActive) {

        }
    }

    
    void OnCollisionEnter(Collision collision) {
        collisionActive = true;

        // Debug.Log(collision.collider.gameObject.layer);

        if (collision.collider.gameObject.layer == 11) {

            

        }
    }
    */

    void OnCollisionExit(Collision collision) {

        if (bounceCount > 0 && bounceCount < 2) {

            // exitVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Vector3 spinForce = exitVector - entranceVector;

            Vector3 spinDirection = new Vector3(launchCon.launchDirection.x, 0, launchCon.launchDirection.z);
            Vector3.Normalize(spinDirection);

            if (launchCon.spinForce.x > 0) {

                rb.AddForce(spinDirection * launchCon.spinForce.x * launchCon.spinForceFactor, ForceMode.Impulse);
                

            } else if(launchCon.spinForce.x < 0) {
               
                rb.AddForce(spinDirection * launchCon.spinForce.x * launchCon.spinForceFactor, ForceMode.Impulse);
                
            }

        }

        bounceCount++;


        /*
        else if (bounceCount == 0 && collision.collider.gameObject.layer == LayerMask.NameToLayer("ground")) {

            entranceVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Debug.Log(entranceVector + " entrance");
        }
        */


        // collisionActive = false;
    }
}
