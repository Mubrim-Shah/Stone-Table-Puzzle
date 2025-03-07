using UnityEngine;

public class TriangleSector : MonoBehaviour
{
    public int sectorIndex; 
    public LightDisc lightDisc1; 
    public LightDisc lightDisc2; 
    public LightDisc centerPiece; 
    public GameObject highlightSprite; 

    public void UpdateReferences()
    {
        if (!PuzzleManager.Instance.mirrorDiscMappings.ContainsKey(sectorIndex))
            return; 

        int[] affectedDiscs = PuzzleManager.Instance.mirrorDiscMappings[sectorIndex];

        lightDisc1 = PuzzleManager.Instance.lightDiscs[affectedDiscs[0]];
        lightDisc2 = PuzzleManager.Instance.lightDiscs[affectedDiscs[1]];
        centerPiece = PuzzleManager.Instance.lightDiscs[8]; 

        //Ensure beams update after swap
        lightDisc1.UpdateBeamAppearance();
        lightDisc2.UpdateBeamAppearance();
        centerPiece.UpdateBeamAppearance();
    }
    public void HighlightSector(bool state)
    {
        if (highlightSprite != null)
            highlightSprite.SetActive(state);
    }
}
