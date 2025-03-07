using UnityEngine;

public class LightDisc : MonoBehaviour
{
    public LightTriangle[] lightTriangles; // Array of LightTriangles 

    private void Start()
    {
        UpdateBeamAppearance();
    }
    /// <summary>
    /// Updates the beam appearance for all LightTriangles attached to this LightDisc.
    /// This triggers a Raycast check to determine whether to activate the Sharp Beam.
    /// </summary>
    public void UpdateBeamAppearance()
    {
        foreach (LightTriangle triangle in lightTriangles)
        {
            triangle.CheckBeamCollision(); // ✅ Perform Raycast check for each beam
        }
    }
}
