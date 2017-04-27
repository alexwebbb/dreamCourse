using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {

    public enum Mode { Explode, Nudge }

    public Mode mode = Mode.Explode;
    public float range = 0;

    // only reason I am using a list is because a queue is not visible in the editor
    public List<Transform> goal;

    int active = 0;
    NavMeshAgent agent;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal[0].position;
    }

    void Update() {

        if (agent.enabled) {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0) {
                if (active < goal.Count - 1) {
                    active++;
                } else {
                    active = 0;
                }
                agent.destination = goal[active].position;
            } 
        }
    }

    void OnCollisionEnter(Collision collision) {

        Rigidbody rb = GetComponent<Rigidbody>();

        switch(mode) {
            case Mode.Explode:
                // explosive force when two player objects collide
                if (collision.gameObject.tag == "Player") {
                    agent.enabled = false;
                    rb.isKinematic = false;
                    // may want to give this a public value at some point
                    rb.AddExplosionForce(800f, collision.transform.position, 0);
                }
                break;
        }

        
    }
}
