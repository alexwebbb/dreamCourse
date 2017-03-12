using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSelect : MonoBehaviour, ISelectionMenu {

    MainMenu mainMenu;
    GameObject elements;

    public MainMenu GetMainMenu {
        get {
            if (mainMenu == null) mainMenu = transform.GetComponentInParent<MainMenu>();
            return mainMenu;
        }
    }

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
