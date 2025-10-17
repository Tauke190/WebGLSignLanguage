// BlendShapeController_Refactored.cs
using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class BlendShapeController : MonoBehaviour
{
    [Header("Mesh Renderers")]
    public List<SkinnedMeshRenderer> childMeshes = new List<SkinnedMeshRenderer>();

    // We keep these public for easy debugging in the Inspector, 
    // but they are no longer needed for the animation logic.
    [Header("Blendshape Values (Debug)")]
    [Range(0, 100)] public float open_jaw;
    [Range(0, 100)] public float smile;
    [Range(0, 100)] public float E;
    [Range(0, 100)] public float TH;
    [Range(0, 100)] public float F_V;
    [Range(0, 100)] public float E_I;
    [Range(0, 100)] public float B_M_P;
    [Range(0, 100)] public float Ooo;

    // A dictionary to keep track of current weights internally.
    private Dictionary<string, float> blendShapeWeights = new Dictionary<string, float>();

    /// <summary>
    /// A fast, direct way to set a specific blend shape's weight.
    /// This replaces the need for reflection.
    /// </summary>
    public void SetBlendWeight(string name, float value)
    {
        // Update the corresponding public field for Inspector debugging
        // Note: This part is optional and uses reflection, but only ONCE per call, not per frame.
        // For pure performance, you could remove this.
        UpdateDebugField(name, value);

        // Apply the weight to all child meshes
        foreach (var mesh in childMeshes)
        {
            if (mesh == null) continue;
            int childIndex = mesh.sharedMesh.GetBlendShapeIndex(name);
            if (childIndex >= 0)
            {
                mesh.SetBlendShapeWeight(childIndex, value);
            }
        }

        // Store the value
        blendShapeWeights[name] = value;
    }

    /// <summary>
    /// Resets all known blend shapes to 0.
    /// </summary>
    public void ResetAllBlendShapes()
    {
        // Use a temporary list to avoid modifying the dictionary while iterating
        var keys = new List<string>(blendShapeWeights.Keys);
        foreach (string key in keys)
        {
            SetBlendWeight(key, 0f);
        }
    }

    // Helper to update public fields for debugging in the inspector.
    private void UpdateDebugField(string name, float value)
    {
        var field = this.GetType().GetField(name);
        if (field != null && field.FieldType == typeof(float))
        {
            field.SetValue(this, value);
        }
    }
}