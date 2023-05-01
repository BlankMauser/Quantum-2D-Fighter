using Quantum;
using UnityEditor;

[CustomEditor(typeof(PlatformConfigAsset))]
public class PlatformConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlatformConfigAsset asset = target as PlatformConfigAsset;

        var settings = serializedObject.FindProperty("Settings");

        DrawMovementOptions(settings);
        EditorGUILayout.Space();
        DrawAmplitudes(settings);
        EditorGUILayout.Space();
        DrawMovementCurves(settings, asset);
        EditorGUILayout.Space();
        DrawRotationCurves(settings);

        settings.serializedObject.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMovementOptions(SerializedProperty settings)
    {
        var mAxis = settings.FindPropertyRelative("MovementAxis");
        
        EditorGUILayout.PropertyField(mAxis, true);
    }

    private void DrawAmplitudes(SerializedProperty settings)
    {
        var mAmp = settings.FindPropertyRelative("MovementAmplitude");
        var rAmp  = settings.FindPropertyRelative("RotationAmplitude");
      
        EditorGUILayout.PropertyField(mAmp, true);
        EditorGUILayout.PropertyField(rAmp, true);
    }

    private static void DrawRotationCurves(SerializedProperty settings)
    {
        var rot = settings.FindPropertyRelative("RotationCurve");
       
        EditorGUILayout.PropertyField(rot, true);
    }

    private static void DrawMovementCurves(SerializedProperty settings, PlatformConfigAsset asset)
    {
        var x = settings.FindPropertyRelative("XMovementCurve");
        var y = settings.FindPropertyRelative("YMovementCurve");
        var z = settings.FindPropertyRelative("ZMovementCurve");

        PlatformAxis axis = asset!.Settings.MovementAxis;

        if (axis.HasFlag(PlatformAxis.X))
            EditorGUILayout.PropertyField(x, true);
        if (axis.HasFlag(PlatformAxis.Y))
            EditorGUILayout.PropertyField(y, true);
        if (axis.HasFlag(PlatformAxis.Z))
            EditorGUILayout.PropertyField(z, true);
    }
}