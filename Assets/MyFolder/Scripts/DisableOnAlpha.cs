using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnAlpha : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetActiveFalse()
    {
        image.DOFade(0, 0.4f).OnComplete(() =>
        {
            image.gameObject.SetActive(false);
        });
    }
    
    public void SetActiveTrue()
    {
        image.gameObject.SetActive(true);
        image.DOFade(1, 0.4f).OnComplete(() =>
        {
            image.color = Color.black;
        });
    }
}
