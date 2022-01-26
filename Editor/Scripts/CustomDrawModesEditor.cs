using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CustomDrawModesEditor
{
    private const string CATALOG_NAME = "CustomDrawMode/";

    private static Shader[] shaders;
    private static int lastIndex = -1;

    static CustomDrawModesEditor()
    {
        shaders = new[]
        {
            Shader.Find(CATALOG_NAME + "Vertex Color"),
            Shader.Find(CATALOG_NAME + "Vertex Color - Red"),
            Shader.Find(CATALOG_NAME + "Vertex Color - Green"),
            Shader.Find(CATALOG_NAME + "Vertex Color - Blue"),
            Shader.Find(CATALOG_NAME + "Vertex Color - Alpha"),
            Shader.Find(CATALOG_NAME + "UV0")
        };

        SceneView.ClearUserDefinedCameraModes();

        for (int i = 0; i < shaders.Length; i++)
            SceneView.AddCameraMode(GetRealName(shaders[i]), "Custom");

        SceneView.duringSceneGui += Draw;
    }

    private static string GetRealName(Shader shader)
    {
        return shader.name.Remove(0, CATALOG_NAME.Length);
    }

    private static void Draw(SceneView sceneView)
    {
        if (lastIndex != -1 && GetRealName(shaders[lastIndex]) == sceneView.cameraMode.name)
        {
            sceneView.camera.RenderWithShader(shaders[lastIndex], "");
            return;
        }

        for (int i = 0; i < shaders.Length; i++)
        {
            if (GetRealName(shaders[i]) == sceneView.cameraMode.name)
            {
                sceneView.camera.RenderWithShader(shaders[i], "");
                lastIndex = i;
                return;
            }
        }

        if (lastIndex != -1)
        {
            lastIndex = -1;
            sceneView.camera.ResetReplacementShader();
        }
    }
}