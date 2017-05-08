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

    int currentLevel { get; set; }

    public void GenerateCourse() {

        currentLevel = 0;

        map = new Panel[height, width];

        // Create map holder object
        string holderName = "Generated Map";
        if (transform.FindChild(holderName)) {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        int lastHeight = 0;

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {

                PanelSet selectedPanel;
                
                List<PanelSet> potentialPanels = new List<PanelSet>();

                foreach (Panel tryPanel in panels) {

                    if (j != 0) {

                        if (map[i, j - 1].right.Equals(tryPanel.left)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.left, tryPanel.right, 0));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.down)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.down, tryPanel.up, 90));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.right)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.right, tryPanel.left, 180));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.up)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.up, tryPanel.down, 270));

                        }


                    } else {
                        potentialPanels.Add(new PanelSet(panels[0], panels[0].left, panels[0].right, 0));
                    }

                }

                // pick a random panel
                if(i == 0 && j == 0) {
                    selectedPanel = potentialPanels[0];
                } else {
                    selectedPanel = potentialPanels[Random.Range(0, potentialPanels.Count)];
                }


                // (int)map[i, j - 1].right.sideHeight
                if (i != 0 && j != 0) currentLevel = map[i, j - 1].heightLevel + lastHeight;
                else if (i > 0) currentLevel = map[i - 1, j].heightLevel + (int)map[i - 1, j].up.sideHeight;


                currentLevel -= (int)selectedPanel.sideOfPanel.sideHeight;
                lastHeight = (int)selectedPanel.oppositeSide.sideHeight;

                map[i, j] = Instantiate(selectedPanel.panelPick.gameObject, 
                    new Vector3(j * panelDiameter, currentLevel * tierHeight, i * panelDiameter), 
                    Quaternion.identity).GetComponent<Panel>();

                bool goofyWaitBool = false;
                while(!goofyWaitBool) {
                    goofyWaitBool = map[i, j].ApplyRotation(selectedPanel.rotation);
                }
                // map[i, j].ApplyRotation(selectedPanel.rotation);


                map[i, j].transform.parent = mapHolder;

                map[i, j].heightLevel = currentLevel;
               
            }
        }
    }
}

[System.Serializable]
public struct PanelSet {

    public Panel panelPick;
    public Side sideOfPanel;
    public Side oppositeSide;
    public int rotation;

    public PanelSet(Panel _panel, Side _side, Side _oppositeSide, int _rotation) {
        this.panelPick = _panel;
        this.sideOfPanel = _side;
        // this is just here because of the dumb leveling bug
        this.oppositeSide = _oppositeSide;
        this.rotation = _rotation;
    }

}