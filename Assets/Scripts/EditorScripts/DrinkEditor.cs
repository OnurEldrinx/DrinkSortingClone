using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrinkGenerator))]
public class DrinkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrinkGenerator drinkGenerator = (DrinkGenerator)target;

        if (GUILayout.Button("Create"))
        {
            drinkGenerator.Create();
        }

    }
}
