using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MenuComponent, ISelectionMenu {

    public GameObject characterListButton;

    GameObject elements;
    Transform characterList;
    NewGame newGame;

    public int SetNumberOfPlayers {
        set {
            newGame.numberOfPlayers = value;
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
        
    }


    void InitializeCharacterList() {
        foreach (GameObject characterGameObject in assetManager.character) {
            if (characterGameObject != null) {
                Character character = characterGameObject.GetComponent<Character>();

                GameObject characterButton = Instantiate<GameObject>(characterListButton);
                characterButton.transform.SetParent(characterList, false);
                characterButton.GetComponentInChildren<Text>().text = character.characterName;
                Button characterButtonComponent = characterButton.GetComponent<Button>();

                characterButtonComponent.onClick.AddListener(() => CharacterSelected(character, characterButtonComponent));
            }
        }
    }

    void CharacterSelected(Character _selectedCharacter, Button button) {
        // this may need to be an ienumerator
        if(newGame.GetCharacterSelection.Contains(_selectedCharacter)) {
            newGame.GetCharacterSelection.Remove(_selectedCharacter);
            button.GetComponent<Image>().color = Color.white;
        } else {
            newGame.GetCharacterSelection.Add(_selectedCharacter);
            button.GetComponent<Image>().color = Color.green;

            if (newGame.GetCharacterSelection.Count >= newGame.GetNumberOfPlayers) {
                Debug.Log("Load");
                mainMenu.Next(newGame);
            }
        }
    }
}
