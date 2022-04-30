using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapController))]
public class MapControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapController mapController = (MapController)target;

        if (DrawDefaultInspector())
        {
            mapController.GenerateMap();
        }
    }
}
