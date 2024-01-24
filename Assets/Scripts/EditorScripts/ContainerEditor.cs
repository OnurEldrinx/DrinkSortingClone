using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Container))]
public class ContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Container container = (Container)target;

        if (GUILayout.Button("Fill"))
        {
            container.Fill(SpawnManager.Instance.GetRandomDrinks());
        }

    }
}
