using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public string levelName;
    public string fileName;

    public Vector3 levelOrigin { get { return transform.position; } }

}
