using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelComponent : MonoBehaviour {

    LevelController levelController;

    public LevelController GetLevelController { get { return levelController; } }

    void Awake() {
        levelController = FindObjectOfType<LevelController>();
    }

}
