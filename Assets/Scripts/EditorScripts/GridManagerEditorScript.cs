using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditorScript : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridManager gridManager = (GridManager)target;

        if (GUILayout.Button("Generate Grid"))
        {
            gridManager.GenerateGrid();
        }

    }

}
