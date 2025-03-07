using UnityEngine;
using DG.Tweening;

public class LightTriangle : MonoBehaviour
{
    public GameObject glowBeam;   
    public GameObject sharpBeam; 

    private bool isSharp = false; 
    /// <summary>
    /// Casts a ray in the direction of the beam to check if it hits a mirror.
    /// If it does, the Sharp Beam is enabled; otherwise, it reverts to the Glow Beam.
    /// </summary>
    public void CheckBeamCollision()
    {
        Vector2 beamDirection = (glowBeam.transform.position - transform.position).normalized; // Get exact beam direction
        float rayLength = 5f; //to check nearby mirror disc

        RaycastHit2D hit = Physics2D.Raycast(transform.position, beamDirection, rayLength, LayerMask.GetMask("MirrorDisc"));

        //Debug.DrawRay(transform.position, beamDirection * rayLength, hit.collider != null ? Color.green : Color.red, 1f);

        bool hitMirror = hit.collider != null;

        if (hitMirror)
            EnableSharpBeam();
        else
            DisableSharpBeam();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MirrorDisc"))
        {
            Debug.Log($"✅ {gameObject.name} HIT Mirror: {other.gameObject.name}");
        }
    }

    private void EnableSharpBeam()
    {
        if (!isSharp)
        {
            isSharp = true;
            glowBeam.SetActive(false);
            sharpBeam.SetActive(true);

            sharpBeam.transform.localScale = Vector3.zero;
            sharpBeam.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);
        }
    }

    private void DisableSharpBeam()
    {
        if (isSharp)
        {
            isSharp = false;

            sharpBeam.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                sharpBeam.SetActive(false);
                glowBeam.SetActive(true);

                glowBeam.transform.localScale = Vector3.zero;
                glowBeam.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);
            });
        }
    }

    public bool IsHittingMirror()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, LayerMask.GetMask("MirrorDisc"));
        return hit.collider != null;
    }

    public bool IsSharpBeamEnabled()
    {
        return sharpBeam.activeSelf;
    }
}
