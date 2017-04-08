using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuComponent : MonoBehaviour {

    public MainMenu mainMenu;
    public AssetManager assetManager;

    GameObject elements;

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.Find("Elements").gameObject;
            return elements;
        }
    }

    public virtual void Start () {
        mainMenu = transform.GetComponentInParent<MainMenu>();
        assetManager = transform.GetComponentInParent<AssetManager>();
    }
}
