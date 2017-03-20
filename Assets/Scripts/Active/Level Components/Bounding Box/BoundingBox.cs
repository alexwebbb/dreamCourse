using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : LevelComponent {


    private void OnTriggerExit(Collider other) {
        BounceController bc = other.GetComponent<BounceController>();
        if (bc != null) {
            GetLevelController.ResetCharacterPosition(bc.GetCharacter);
        }
    }
}
