﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBox : MonoBehaviour {


    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("point scored");
        }
        
    }

}
