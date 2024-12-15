using UnityEngine;

[ExecuteInEditMode]
public class PixelationEffect : MonoBehaviour
{
    public Material pixelationMaterial;
    [Range(1, 500)] public float pixelDensity = 20f;
    public float maxDistance = 20f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelationMaterial != null)
        {
            pixelationMaterial.SetFloat("_PixelDensity", pixelDensity);
            pixelationMaterial.SetFloat("_MaxDistance", maxDistance);
            Graphics.Blit(src, dest, pixelationMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
