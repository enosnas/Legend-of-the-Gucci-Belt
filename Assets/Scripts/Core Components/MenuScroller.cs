using UnityEngine;
using UnityEngine.UI;

public class MenuScroller : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float _x, _y;

    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(_x,_y) * Time.deltaTime, image.uvRect.size);
    }
}
