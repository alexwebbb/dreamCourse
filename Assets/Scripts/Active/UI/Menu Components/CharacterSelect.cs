using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour, ISelectionMenu {

    public GameObject characterListButton;

    public MainMenu GetMainMenu {
        get {
            if (mainMenu == null) mainMenu = transform.GetComponentInParent<MainMenu>();
            return mainMenu;
        }
    }

    public GameObject GetElements {
        get {
            if (elements == null) elements = transform.FindChild("Elements").gameObject;
            return elements;
        }
    }

    public AssetManager GetAssetManager {
        get {
            if (assetManager == null) assetManager = transform.GetComponentInParent<AssetManager>();
            return assetManager;
        }
    }

    public GameObject GetCharacterList {
        get {
            if (characterList == null) characterList = transform.FindChild("Character List").gameObject;
            return characterList;
        }
    }

    MainMenu mainMenu;
    GameObject elements;
    AssetManager assetManager;
    GameObject characterList;
    NewGame newGame;

    void InitializeCharacterList() {
        foreach (GameObject characterGameObject in GetAssetManager.character) {
            if (characterGameObject != null) {
                Character character = characterGameObject.GetComponent<Character>();

                GameObject characterButton = Instantiate<GameObject>(characterListButton);
                characterButton.transform.SetParent(characterList.transform, false);
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
            }
        }
    }
}
