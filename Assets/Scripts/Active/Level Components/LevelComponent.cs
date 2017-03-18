using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelComponent : MonoBehaviour {

    LevelController levelController;
    GameObject element;

    public GameObject GetElement {
        get {
            if (element == null) element = transform.GetChild(0).gameObject;
            return element;
        }
    }

    void Awake() {
        levelController = FindObjectOfType<LevelController>();
    }

}
