using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridInstance))]
public class GridEditorScript : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridInstance grid = (GridInstance)target;

        if (GUILayout.Button("Fill"))
        {
            //grid.FillAllShelves();
        }

    }
}
