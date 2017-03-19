using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelComponent {

    Renderer renderComponent;
    Color defaultColor;

    Character currentOwner;
    Character lastOwner = null;

    public Renderer GetRenderer {
        get {
            if (renderComponent == null) renderComponent = GetComponent<Renderer>();
            return renderComponent;
        }
    }

    public Color SetColor { set { GetRenderer.material.color = value; } }
    public void ResetColor() { GetRenderer.material.color = defaultColor; } 

    public Character GetLastOwner { get { return lastOwner; } }

    private void Start() {
        defaultColor = GetRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other) {
        BounceController bc = other.GetComponent<BounceController>();
        if(bc != null) {
            lastOwner = currentOwner;
            currentOwner = bc.GetCharacter;

            GetLevelController.AddPoint(bc.GetCharacter, this);
        }
    }
}
