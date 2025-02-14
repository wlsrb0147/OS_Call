using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour, IPointerUpHandler
{
    private Slider slider;
    private GameManager gameManager;
    [SerializeField] private Animator animator;
    
    private static readonly int SMALL = Animator.StringToHash("Small");
    private static readonly int IDLE = Animator.StringToHash("Reset");
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (slider.interactable)
        {
            if (slider.value >= 0.99f)
            {
                slider.interactable = false;
                Invoke(nameof(PlayAnim),0.5f);
                slider.gameObject.SetActive(false);
                gameManager.StartGame();
            }
            else
            {
                slider.value = 0f;
            }
        }
    }

    private void PlayAnim()
    {
        animator.SetTrigger(SMALL);
    }

    public void ReturnIdle()
    {
        animator.SetTrigger(IDLE);
        slider.value = 0;
        slider.interactable = true;
        slider.gameObject.SetActive(true);
    }
}
