using UnityEngine;
using UnityEditor;
using System.Collections;

public class SphereBuilder : EditorWindow
{
    [MenuItem("Tim/SphereBuilder")]
    public static void Init()
    {
        var globeData = BuildGlobe.GetGlobeData();
        var mesh = BuildGlobe.CreateMesh(globeData);
        AssetDatabase.CreateAsset(mesh, "Assets/GlobeMesh.asset");
        AssetDatabase.Refresh();
    }

}