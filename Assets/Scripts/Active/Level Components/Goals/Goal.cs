﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelComponent {

    Renderer renderComponent;
    Color defaultColor;

    Character currentOwner;
    Character lastOwner = new Character();

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
        // cache the default color of the goal
        defaultColor = GetRenderer.material.color;

        // register this goal with the level controller
        GetLevelController.RegisterGoal();
    }

    private void OnTriggerEnter(Collider other) {
        BounceController bc = other.GetComponent<BounceController>();
        if (bc != null) {
            Character player = bc.GetCharacter;
            if (player != currentOwner) {
                lastOwner = currentOwner;
                currentOwner = player;
                // haha make sure the add point call comes AFTER the assignment of current owner
                GetLevelController.AddPoint(player, this);
            } 
        }
    }
}
