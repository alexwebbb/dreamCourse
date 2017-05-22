using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour {

    /*
    // cache and set the properties of rb. necessary if environmental effects change these. magnus force?
    rb.angularDrag = launcher.angleDrag;
    rb.drag = dragDefault = launcher.drag;
    */
    
    

    Dictionary<MonoBehaviour, float> cfDict = new Dictionary<MonoBehaviour, float>();

    void SetConstantForce(MonoBehaviour _source, float _force) {

        if (!cfDict.ContainsKey(_source)) cfDict.Add(_source, _force);

        float _total = 0;

        foreach(KeyValuePair<MonoBehaviour, float> entry in cfDict) {
            _total += entry.Value;
        }


    }
}

