using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBallComponent {

    void Initialize(Rigidbody rb, LaunchController lc);
}
