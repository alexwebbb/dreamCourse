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

    int currentLevel = 0;

    public void GenerateCourse() {

        map = new Panel[height, width];

        // Create map holder object
        string holderName = "Generated Map";
        if (transform.FindChild(holderName)) {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {

                PanelSet selectedPanel;
                
                List<PanelSet> potentialPanels = new List<PanelSet>();

                foreach(Panel tryPanel in panels) {

                    if (j != 0) {

                        if (map[i, j - 1].right.Equals(tryPanel.left)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.left, 0));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.up)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.up, 90));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.right)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.right, 180));

                        }

                        if (map[i, j - 1].right.Equals(tryPanel.down)) {

                            potentialPanels.Add(new PanelSet(tryPanel, tryPanel.down, 270));

                        }
                    } else {
                        potentialPanels.Add(new PanelSet(panels[0], panels[0].left, 0));
                    }

                }

                // pick a random panel
                selectedPanel = potentialPanels[0];
                currentLevel -= (int)selectedPanel.sideOfPanel.sideHeight;

                Debug.Log("i : " + i + ", j: " + j);
                map[i, j] = Instantiate(selectedPanel.panelPick.gameObject, 
                    new Vector3(j * panelDiameter, currentLevel * tierHeight, i * panelDiameter), 
                    Quaternion.identity).GetComponent<Panel>();
                map[i, j].ApplyRotation(selectedPanel.rotation);
                map[i, j].transform.parent = mapHolder;

                map[i, j].heightLevel = currentLevel;


            }
        }
    }
}

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