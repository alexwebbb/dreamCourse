using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionMenu {

    GameObject GetElements { get; }

    // assigns the newgame property to a local variable
    void Initialize(NewGame newGame);

    void ResetMenu();

    void ConfirmSelection();
}
