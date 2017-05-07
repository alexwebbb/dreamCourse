using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CourseGenerator : MonoBehaviour {


    public GameObject[] panel;
    public int width;
    public int height;

    List<List<GameObject>> map;
    float panelDiameter = 8f;
    float tierHeight = 2.14f;

    int currentLevel = 0;

    public void GenerateCourse() {

        // Create map holder object
        string holderName = "Generated Map";
        if (transform.FindChild(holderName)) {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {

                Debug.Log("i : " + i + ", j: " + j);
                GameObject temp = Instantiate(panel[0], new Vector3(j * panelDiameter, currentLevel * tierHeight, i * panelDiameter), Quaternion.identity);
                temp.transform.parent = mapHolder;
            }
        }
    }
}
