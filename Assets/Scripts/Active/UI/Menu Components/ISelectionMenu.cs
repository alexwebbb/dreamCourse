using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionMenu {

    GameObject GetElements { get; }

    void Initialize(NewGame newGame);
    // assigns the newgame property to a local variable

    void ResetMenu();

}
