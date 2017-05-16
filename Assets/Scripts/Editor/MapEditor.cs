using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CourseGenerator))]
public class MapEditor : Editor
{

    public override void OnInspectorGUI() {
        CourseGenerator course = (CourseGenerator)target;

        if (DrawDefaultInspector()) {

            //course.GenerateCourse();
        }

        if (GUILayout.Button("Generate Map")) {

            course.GenerateCourse();
        }
    }
}