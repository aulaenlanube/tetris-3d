using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextosTetris))] // Asegúrate de cambiar 'EjemploInspector' por el nombre de tu script
public class EjemploInspectorEditor : Editor
{
    private GUIStyle lightGrayBoxStyle;
    private GUIStyle darkGrayBoxStyle;

    private void OnEnable()
    {
        // Estilo para la caja de tono gris claro
        lightGrayBoxStyle = new GUIStyle(GUI.skin.box);
        lightGrayBoxStyle.normal.background = MakeTex(2, 2, new Color(0.9f, 0.9f, 0.9f));

        // Estilo para la caja de tono gris oscuro
        darkGrayBoxStyle = new GUIStyle(GUI.skin.box);
        darkGrayBoxStyle.normal.background = MakeTex(2, 2, new Color(0.6f, 0.6f, 0.6f));
    }
    public override void OnInspectorGUI()
    {
        TextosTetris script = (TextosTetris)target;

        // Comienza la personalización del Inspector
        DrawSection("TEXTOS ESTILABLES", lightGrayBoxStyle, () =>
        {
            SerializedProperty nombresEnemigosProp = serializedObject.FindProperty("textosUI");
            EditorGUILayout.PropertyField(nombresEnemigosProp, new GUIContent("Textos de la UI"), true);
            serializedObject.ApplyModifiedProperties();
        });

        DrawSection("MATERIALES PARA LOS TEXTOS", darkGrayBoxStyle, () =>
        {
            SerializedProperty nombresEnemigosProp = serializedObject.FindProperty("materiales");
            EditorGUILayout.PropertyField(nombresEnemigosProp, new GUIContent("Materiales Textos"), true);
            serializedObject.ApplyModifiedProperties();
        });

        // Agrega aquí más secciones según sea necesario
    }

    private void DrawSection(string title, GUIStyle style, System.Action drawFields)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(style);
        GUILayout.Label(title, EditorStyles.boldLabel);
        drawFields();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    // Función auxiliar para crear una textura de un color específico
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

