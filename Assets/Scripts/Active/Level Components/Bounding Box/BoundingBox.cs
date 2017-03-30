using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : LevelComponent {

    public event Action characterOutOfBoundsEvent; 

    private void Start() {
        GetLevelController.RegisterBoundingBox(this);
    }

    private void OnTriggerExit(Collider other) {
        BounceController bc = other.GetComponent<BounceController>();
        if (bc != null) {
            bc.GetCharacter.ReturnOutOfBoundsCharacterToLastPosition();
            if (characterOutOfBoundsEvent != null) characterOutOfBoundsEvent();
        }
    }
}
