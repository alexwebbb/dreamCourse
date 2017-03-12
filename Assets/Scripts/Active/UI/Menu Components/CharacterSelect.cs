using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour, ISelectionMenu {

    GameObject elements;

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.GetChild(0).gameObject;
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
