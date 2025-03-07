using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro; 

/// <summary>
/// Manages the Light Disc puzzle, handling swaps, light beam updates, and level completion.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; // Singleton

    [Header("Puzzle Elements")]
    public LightDisc[] lightDiscs;   
    public MirrorDisc[] mirrorDiscs; 
    public TriangleSector[] triangleSectors; 

    [Header("UI Elements")]
    public GameObject backgroundFade; 
    public TextMeshProUGUI completionText; 

    private bool isSwapping = false; // Ensures only one swap happens at a time
    public Dictionary<int, int[]> mirrorDiscMappings; // Stores triangle mappings for swaps

    private void Awake()
    {
        Instance = this;
        InitializeMirrorMappings();

        if (completionText) completionText.gameObject.SetActive(false);
        if (backgroundFade) backgroundFade.SetActive(false);
    }

    /// <summary>
    /// Defines the Light Disc swap logic for each Mirror Disc.
    /// </summary>
    private void InitializeMirrorMappings()
    {
        mirrorDiscMappings = new Dictionary<int, int[]>()
        {
            { 0, new int[] { 0, 1, 8 } },
            { 1, new int[] { 1, 2, 8 } },
            { 2, new int[] { 2, 3, 8 } },
            { 3, new int[] { 3, 4, 8 } },
            { 4, new int[] { 4, 5, 8 } },
            { 5, new int[] { 5, 6, 8 } },
            { 6, new int[] { 6, 7, 8 } },
            { 7, new int[] { 7, 0, 8 } }
        };
    }

    /// <summary>
    /// Handles Mirror Disc rotation and Light Disc swapping in an anticlockwise pattern.
    /// </summary>
    public void RotateMirrorDisc(int mirrorIndex)
    {
        if (isSwapping || !mirrorDiscMappings.ContainsKey(mirrorIndex))
            return;

        isSwapping = true; 

        int[] affectedDiscs = mirrorDiscMappings[mirrorIndex];

        LightDisc disc1 = lightDiscs[affectedDiscs[0]];
        LightDisc disc2 = lightDiscs[affectedDiscs[1]];
        LightDisc disc3 = lightDiscs[affectedDiscs[2]]; 

        Vector3 pos1 = disc1.transform.position;
        Vector3 pos2 = disc2.transform.position;
        Vector3 pos3 = disc3.transform.position;

        triangleSectors[mirrorIndex].HighlightSector(true);

        //Perform the swap in an anticlockwise order
        disc1.transform.DOMove(pos2, 1.5f);
        disc2.transform.DOMove(pos3, 1.5f);
        disc3.transform.DOMove(pos1, 1.5f).OnComplete(() =>
        {
            //Update references after swap
            lightDiscs[affectedDiscs[0]] = disc3;
            lightDiscs[affectedDiscs[1]] = disc1;
            lightDiscs[affectedDiscs[2]] = disc2;

            ForceUpdateTriangleSectors();

            triangleSectors[mirrorIndex].HighlightSector(false);

            CheckPuzzleCompletion();

            isSwapping = false; 
        });
    }

    /// <summary>
    /// Updates Triangle Sector references and refreshes beam appearances.
    /// </summary>
    private void ForceUpdateTriangleSectors()
    {
        foreach (TriangleSector sector in triangleSectors)
        {
            sector.UpdateReferences();
        }

        foreach (LightDisc disc in lightDiscs)
        {
            disc.UpdateBeamAppearance();
        }
    }

    /// <summary>
    /// Checks if all Sharp Beams are enabled, indicating puzzle completion.
    /// If all beams are correctly aligned, it triggers the completion UI.
    /// </summary>
    private void CheckPuzzleCompletion()
    {
        foreach (LightDisc disc in lightDiscs)
        {
            foreach (LightTriangle triangle in disc.lightTriangles)
            {
                if (!triangle.IsSharpBeamEnabled()) 
                {
                    return;
                }
            }
        }

        Debug.Log("🎉 Puzzle Finished Successfully!");

        Invoke(nameof(ShowCompletionUI), 0.5f);
    }

    /// <summary>
    /// Enables the background fade and completion text after the delay.
    /// </summary>
    private void ShowCompletionUI()
    {
        backgroundFade.SetActive(true);
        completionText.gameObject.SetActive(true);
    }


    /// <summary>
    /// Displays the puzzle completion text after the delay.
    /// </summary>
    private void ShowCompletionText()
    {
        completionText.gameObject.SetActive(true);
    }


    /// <summary>
    /// Returns whether a swap is currently happening.
    /// This prevents multiple swaps at the same time.
    /// </summary>
    public bool IsSwapping()
    {
        return isSwapping;
    }
}
