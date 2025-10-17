using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class VisemeData
{
    public int visemeId;
    public int time; // in ms
}

// VisemeReceiver_Refactored.cs

public class VisemeReceiver : MonoBehaviour
{
    [Header("References")]
    public BlendShapeController blendController; // Assign in inspector

    [Header("Animation Settings")]
    [Tooltip("Controls the overall strength of the viseme movements. 1 is full intensity, 0.5 is half.")]
    [Range(0f, 1f)]
    public float visemeIntensity = 0.8f;

    [Tooltip("Controls the playback speed. 1 is normal, 0.5 is half-speed (slower), 2 is double-speed (faster).")]
    [Range(0.25f, 2f)]
    public float playbackSpeed = 1.0f; // ** NEW: Speed control slider **

    [Header("Viseme → Blendshape Mapping")]
    public string PP = "B_M_P";
    public string FF = "F_V";
    public string TH_Viseme = "TH";
    public string DD = "open_jaw";
    public string kk = "open_jaw";
    public string aa = "E";
    public string ih = "E_I";
    public string oh = "Ooo";

    [Header("Debug")]
    public bool debugLogs = true;

    private Dictionary<int, string> visemeMap;
    private List<string> controlledShapes;

    void Awake()
    {
        InitializeVisemeMap();
    }

    public void ReceiveViseme(string json)
    {
        VisemeData[] visemes = JsonHelper.FromJson<VisemeData>(json);
        if (visemes == null || visemes.Length == 0)
        {
            Debug.LogWarning("No viseme data found!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(PlayVisemesSmooth(visemes));
    }

    private IEnumerator PlayVisemesSmooth(VisemeData[] visemes)
    {
        Dictionary<string, float> currentWeights = new Dictionary<string, float>();
        foreach (string shape in controlledShapes)
        {
            currentWeights[shape] = 0f;
        }
        blendController.ResetAllBlendShapes();

        float maxWeight = 100f * visemeIntensity;

        for (int i = 0; i < visemes.Length; i++)
        {
            VisemeData v = visemes[i];

            if (!visemeMap.TryGetValue(v.visemeId, out string targetShape))
            {
                if (debugLogs) Debug.LogWarning($"Viseme ID {v.visemeId} not found in map. Skipping.");
                continue;
            }

            // ** MODIFIED: Duration is now divided by playbackSpeed **
            float baseDuration = (i < visemes.Length - 1)
                ? (visemes[i + 1].time - v.time) / 1000f
                : 0.1f;
            float duration = baseDuration / playbackSpeed;


            if (debugLogs)
                Debug.Log($"Viseme {v.visemeId} → {targetShape} over {duration:F3}s");

            var previousWeights = new Dictionary<string, float>(currentWeights);
            float elapsed = 0f;

            // To ensure animation speed is correct, we handle the case where duration might be zero or negative
            if (duration <= 0)
            {
                // If duration is invalid, just snap to the end state and continue
                elapsed = duration;
            }

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                foreach (string shape in controlledShapes)
                {
                    float start = previousWeights[shape];
                    float end = (shape == targetShape) ? maxWeight : 0f;
                    float weight = Mathf.Lerp(start, end, t);

                    blendController.SetBlendWeight(shape, weight);
                    currentWeights[shape] = weight;
                }
                yield return null;
            }

            // Snap to final values to ensure accuracy
            foreach (string shape in controlledShapes)
            {
                float finalWeight = (shape == targetShape) ? maxWeight : 0f;
                blendController.SetBlendWeight(shape, finalWeight);
                currentWeights[shape] = finalWeight;
            }
        }

        if (debugLogs) Debug.Log("Animation finished. Resetting shapes.");
        blendController.ResetAllBlendShapes();
    }

    private void InitializeVisemeMap()
    {
        visemeMap = new Dictionary<int, string>
        {
            { 13, PP }, { 17, FF }, { 16, TH_Viseme }, { 21, TH_Viseme },
            { 15, DD }, { 19, DD }, { 20, DD }, { 14, kk }, { 18, kk },
            { 1, aa }, { 2, aa }, { 3, aa },
            { 0, ih }, { 5, ih }, { 6, ih }, { 7, ih }, { 8, ih }, { 22, ih }, { 23, ih },
            { 4, oh }, { 10, oh }, { 11, oh }, { 12, oh }
        };

        controlledShapes = new List<string>();
        foreach (var shapeName in visemeMap.Values)
        {
            if (!controlledShapes.Contains(shapeName))
            {
                controlledShapes.Add(shapeName);
            }
        }
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        if (json.TrimStart().StartsWith("{"))
            return JsonUtility.FromJson<Wrapper<T>>(json).data;

        string wrappedJson = "{\"data\":" + json + "}";
        return JsonUtility.FromJson<Wrapper<T>>(wrappedJson).data;
    }

    [System.Serializable]
    private class Wrapper<T> { public T[] data; }
}
