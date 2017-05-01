using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPad : MonoBehaviour {

    public ParticleSystem splashEffect;

    public float power = 5f;
    public float resistance = 2f;
    public float skipThreshhold = 0.9f;
    public float skipPower = 1.2f;

    public float horizontalBounceScale = 1.2f;

    Material material;

    private void Start() {
        material = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {

        //// surface bounce

        // check with a raycast to make sure the bouncing object is above the water
        RaycastHit hit;
        Ray ray = new Ray(other.transform.position, transform.TransformDirection(Vector3.down));
        if (Physics.Raycast(ray, out hit, 2f) && hit.collider.gameObject.layer == 4) {
            
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            // check the dot product of the velocity vs the horizontal plane
            if (Mathf.Abs(Vector3.Dot(new Vector3(otherRB.velocity.x, 0, otherRB.velocity.z).normalized, otherRB.velocity.normalized)) > skipThreshhold) {
                // compare the magnitude of horizontal movement vs vertical, if horizontal is greater, bounce can occur
                if (Vector3.SqrMagnitude(new Vector3(otherRB.velocity.x, 0, otherRB.velocity.z)) / 2 > Vector3.SqrMagnitude(new Vector3(0, otherRB.velocity.y, 0))) {
                    // splash effect
                    GameObject splash = Instantiate<GameObject>(splashEffect.gameObject, other.transform.position - new Vector3(0, 1f, 0), splashEffect.transform.rotation);
                    splash.GetComponent<ParticleSystemRenderer>().material = material; 
                    Destroy(splash, 1f);
                    // adjust velocity, slowing horizontal movement, adding vertical velocity
                    otherRB.velocity = new Vector3(otherRB.velocity.x / horizontalBounceScale, skipPower * -otherRB.velocity.y, otherRB.velocity.z / horizontalBounceScale);
                }
            }

        }


        BounceController bc = other.GetComponent<BounceController>();

        bc.AddDrag = resistance;

        bc.AddConstantForce = new Vector3(0, power, 0);
    }

    private void OnTriggerExit(Collider other) {

        BounceController bc = other.GetComponent<BounceController>();

        bc.ResetDrag();

        bc.ResetConstantForce();

    }
}
