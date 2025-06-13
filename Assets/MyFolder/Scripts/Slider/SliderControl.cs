using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class SliderControl : MonoBehaviour, IPointerUpHandler
{
    private Slider slider;
    private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private GameObject bkg;
    
    private static readonly int LEFTTOP = Animator.StringToHash("LeftTop");
    private static readonly int LEFTBOTTOM = Animator.StringToHash("LeftBottom");
    private static readonly int RIGHTTOP = Animator.StringToHash("RightTop");
    private static readonly int RIGHTBOTTOM = Animator.StringToHash("RightBottom");

    private string horizontal = "right";
    private string vertical = "top";
    
    private static readonly int IDLE = Animator.StringToHash("Reset");
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        
        horizontal = JsonSaver.instance.Settings.horizontal;
        vertical = JsonSaver.instance.Settings.vertical;
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
                vp.Play();
                bkg.SetActive(false);
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
        int x = (horizontal, vertical) switch
        {
            ("left", "top") => LEFTTOP,
            ("left", "bottom") => LEFTBOTTOM,
            ("right", "top") => RIGHTTOP,
            ("right", "bottom") => RIGHTBOTTOM,
            _ => LEFTTOP
        };
        
        animator.SetTrigger(x);
    }

    public void ReturnIdle()
    {
        animator.SetTrigger(IDLE);
        slider.value = 0;
        slider.interactable = true;
        slider.gameObject.SetActive(true);
    }
}
