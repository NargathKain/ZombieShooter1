using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// Utility to optimize Casino scene lights by disabling shadows on most lights.
/// The Casino scene has 30+ shadow-casting lights which can crash the GPU.
/// This script keeps shadows only on the most important lights.
/// </summary>
public class CasinoLightOptimizer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Optimize Casino Lights")]
    public static void OptimizeCasinoLights()
    {
        int disabledCount = 0;
        int keptCount = 0;

        // Find all lights in the scene
        Light[] allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in allLights)
        {
            // Keep shadows on directional lights (sun/moon) - they're cheap
            if (light.type == LightType.Directional)
            {
                keptCount++;
                continue;
            }

            // Keep shadows on very bright/important lights
            if (light.intensity > 5f && light.shadows != LightShadows.None)
            {
                keptCount++;
                continue;
            }

            // Disable shadows on all other lights
            if (light.shadows != LightShadows.None)
            {
                Undo.RecordObject(light, "Disable Light Shadow");
                light.shadows = LightShadows.None;
                EditorUtility.SetDirty(light);
                disabledCount++;
            }
        }

        Debug.Log($"[CasinoLightOptimizer] Disabled shadows on {disabledCount} lights, kept {keptCount} lights with shadows.");

        if (disabledCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }

    [MenuItem("Tools/Restore Casino Light Shadows (Soft)")]
    public static void RestoreSoftShadows()
    {
        int restoredCount = 0;
        Light[] allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in allLights)
        {
            if (light.type == LightType.Spot || light.type == LightType.Point)
            {
                if (light.shadows == LightShadows.None && light.intensity > 1f)
                {
                    Undo.RecordObject(light, "Enable Light Shadow");
                    light.shadows = LightShadows.Soft;
                    EditorUtility.SetDirty(light);
                    restoredCount++;
                }
            }
        }

        Debug.Log($"[CasinoLightOptimizer] Restored soft shadows on {restoredCount} lights.");

        if (restoredCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
#endif
}
