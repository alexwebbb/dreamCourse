﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelect : MonoBehaviour, ISelectionMenu {

    GameObject elements;

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.FindChild("Elements").gameObject;
            return elements;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
