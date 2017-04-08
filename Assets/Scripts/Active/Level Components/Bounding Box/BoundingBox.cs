using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : LevelComponent {

    public event Action<Character> characterOutOfBoundsEvent; 

    private void Start() {
        GetLevelController.RegisterBoundingBox(this);
    }

    private void OnTriggerExit(Collider other) {
        BounceController bc = other.GetComponent<BounceController>();
        if (bc != null) {
            Character character = bc.GetCharacter;
            character.ReturnOutOfBoundsCharacterToLastPosition();
            if (characterOutOfBoundsEvent != null) characterOutOfBoundsEvent(character);
        }
    }
}
