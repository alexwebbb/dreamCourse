using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {

    public List<Transform> goal;

    int active = 0;
    NavMeshAgent agent;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal[0].position;
    }

    void Update() {
        
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0) {
            if(active < goal.Count - 1) {
                active++;
            } else {
                active = 0;
            }
            agent.destination = goal[active].position;
        }
    }
}
