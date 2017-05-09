using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CourseGenerator : MonoBehaviour {


    public Panel[] panels;
    public int width;
    public int height;

    Panel[,] map;
    float panelDiameter = 8f;
    float tierHeight = 2.14f;
    
    public void GenerateCourse() {

        // current level keeps track of the height of the tiles
        int currentLevel = 0;

        // create an array for storing all the tiles
        map = new Panel[height, width];

        // Set name for map holder, detroy the previous one
        string holderName = "Generated Map";
        if (transform.FindChild(holderName)) {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        // create a holder for the tiles and set the parent as the current gameobject
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        
        // primary pair of nested loops used to create the grid
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {

                // create a new selected panel and panel pick list each iteration
                PanelSet selectedPanel;
                List<PanelSet> potentialPanels = new List<PanelSet>();

                // in this loop every side of every potential tile is compared 
                // to the last tile and added to the list if it matches 
                foreach (Panel tryPanel in panels) {

                    if (j != 0) {

                        if (map[i, j - 1].right.Equals(tryPanel.left)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.left, 0));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.down)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.down, 90));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.right)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.right, 180));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.up)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.up, 270));

                        }


                    } else {
                        // this default case is here for the initial tile on each row
                        potentialPanels.Add(new PanelSet(panels[0], panels[0].left, 0));
                    }
                }

                /// going to have the other check for the side of the tile occur here
                /// maybe add some optional parameters to the panel set struct
                /// 


                // pick a random panel if it is not the first panel
                if(i == 0 && j == 0) {
                    selectedPanel = potentialPanels[0];
                } else {
                    selectedPanel = potentialPanels[Random.Range(0, potentialPanels.Count)];
                }

                // the previous tile is used to adjust the height of the current level int, 
                // which will be used to set the position of the tile that is about to be instantiated
                if (j != 0) currentLevel = map[i, j - 1].heightLevel + map[i, j - 1].right.sideHeight;
                else if (i > 0) currentLevel = map[i - 1, j].heightLevel + map[i - 1, j].up.sideHeight;

                // adjust the height based on which side of the tile got selected
                // ie if the top edge of a tile got selected, it will shift it down
                currentLevel -= selectedPanel.sideOfPanel.sideHeight;

                // instantiate the tile in the correct position on the grid based on the loops
                map[i, j] = Instantiate(selectedPanel.panelPick.gameObject, 
                    new Vector3(j * panelDiameter, currentLevel * tierHeight, i * panelDiameter), 
                    Quaternion.identity).GetComponent<Panel>();

                // use the special apply rotation function which also changes the sides
                map[i, j].ApplyRotation(selectedPanel.rotation);

                // put the tiles in the generated map gameobject
                map[i, j].transform.parent = mapHolder;
                
                // set the value on the panel for height level, 
                // which is used to set the current level variable in the next iteration
                map[i, j].heightLevel = currentLevel;
               
            }
        }
    }
}

[System.Serializable]
public struct PanelSet {

    public Panel panelPick;
    public Side sideOfPanel;
    public int rotation;

    public PanelSet(Panel _panel, Side _side, int _rotation) {
        this.panelPick = _panel;
        this.sideOfPanel = _side;
        this.rotation = _rotation;
    }

}