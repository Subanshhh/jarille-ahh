using UnityEngine;
using UnityEngine.UI;

public class RandomImageSelector : MonoBehaviour
{
    public RawImage rawImage; // ✅ correct type

    public Texture[] possibleTextures; // ✅ textures instead of sprites

    void Start()
    {
        SetRandomImage();
    }

    public void SetRandomImage()
    {
        if (possibleTextures == null || possibleTextures.Length == 0)
        {
            Debug.LogWarning("No textures assigned!");
            return;
        }

        Texture selected = possibleTextures[Random.Range(0, possibleTextures.Length)];
        rawImage.texture = selected;
    }
}