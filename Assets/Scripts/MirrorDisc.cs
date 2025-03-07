using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles the behavior of a Mirror Disc, allowing it to rotate and trigger a sector swap.
/// </summary>
public class MirrorDisc : MonoBehaviour
{
    public int mirrorIndex;

    private void OnMouseDown()
    {
        if (!PuzzleManager.Instance.IsSwapping()) // Prevents interaction during an ongoing swap
        {
            RotateMirror();
            PuzzleManager.Instance.RotateMirrorDisc(mirrorIndex);
        }
    }

    /// <summary>
    /// Rotates the Mirror Disc 60 degrees counterclockwise using DoTween.
    /// </summary>
    public void RotateMirror()
    {
        transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + 60f), 0.5f);
    }
}
