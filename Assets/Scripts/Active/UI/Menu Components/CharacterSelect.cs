using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MenuComponent, ISelectionMenu {

    public GameObject characterListButton;
    public Color deselectedColor = Color.white;
    public Color selectedColor = Color.green;

    Transform characterList;
    NewGame newGame;
    List<Button> characterButtons = new List<Button>();

    public int SetNumberOfPlayersButton {
        set {
            ClearSelectedCharacters();
            newGame.SetNumberOfPlayers = value;
            characterList.GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public override void Start() {
        base.Start();
        characterList = transform.Find("Elements/Character List Mask/Character List");

        InitializeCharacterList();
    }


    // Called when loading and unloading the menu, required by the interface

    public void Initialize(NewGame _newGame) {
        // set reference to new game object
        newGame = _newGame;
    }

    public void ResetMenu() {
        ClearSelectedCharacters();
    }

    void InitializeCharacterList() {
        foreach (GameObject characterGameObject in assetManager.character) {
            if (characterGameObject != null) {
                Character character = characterGameObject.GetComponent<Character>();

                GameObject characterButton = Instantiate<GameObject>(characterListButton);
                characterButton.transform.SetParent(characterList, false);
                characterButton.GetComponentInChildren<Text>().text = character.characterName;
                Button characterButtonComponent = characterButton.GetComponent<Button>();
                // add click listener
                characterButtonComponent.onClick.AddListener(() => CharacterSelected(character, characterButtonComponent));
                // add to the list of buttons so we can clear the list later
                characterButtons.Add(characterButtonComponent);
            }
        }
    }

    void CharacterSelected(Character _selectedCharacter, Button button) {
        if(newGame.GetCharacterSelection.Contains(_selectedCharacter)) {
            newGame.GetCharacterSelection.Remove(_selectedCharacter);
            button.GetComponent<Image>().color = deselectedColor;
        } else if(newGame.GetCharacterSelection.Count < newGame.GetNumberOfPlayers) {
            newGame.GetCharacterSelection.Add(_selectedCharacter);
            button.GetComponent<Image>().color = selectedColor;
        }
    }

    void ClearSelectedCharacters() {
        // clear list of characters
        newGame.GetCharacterSelection.Clear();
        // return each button to white
        foreach(Button button in characterButtons) {
            button.GetComponent<Image>().color = deselectedColor;
        }
    }

    public void ConfirmSelection() {
        if (newGame.GetCharacterSelection.Count == newGame.GetNumberOfPlayers) {
            // load the game!
            mainMenu.Next(newGame);
        }
    }
}
