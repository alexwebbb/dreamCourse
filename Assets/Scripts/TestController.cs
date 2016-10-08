using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

    public GameObject launchedObject;
    public float force;
    public float posit;
    public float angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L)) {
            GameObject testBall = (GameObject)Instantiate(launchedObject, new Vector3(posit, 0, 0), Quaternion.Euler(angle, 0, 0));
            testBall.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 1, 0) * force, ForceMode.Impulse);
        }
	}
}
