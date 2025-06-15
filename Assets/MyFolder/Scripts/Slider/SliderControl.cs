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
    
    private static readonly int LEFTTOP = Animator.StringToHash("LeftTop");
    private static readonly int LEFTBOTTOM = Animator.StringToHash("LeftBottom");
    private static readonly int RIGHTTOP = Animator.StringToHash("RightTop");
    private static readonly int RIGHTBOTTOM = Animator.StringToHash("RightBottom");

    private string horizontal = "right";
    private string vertical = "top";
    
    private static readonly int IDLE = Animator.StringToHash("Reset");
    
    [SerializeField] private CanvasGroup bkg;
    [SerializeField] private CanvasGroup group;

    [SerializeField] private Animator anim;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        
        horizontal = JsonSaver.instance.Settings.horizontal;
        vertical = JsonSaver.instance.Settings.vertical;
        vp.loopPointReached += VpOnloopPointReached;
    }

    private readonly int reset = Animator.StringToHash("Reset");

    private void VpOnloopPointReached(VideoPlayer source)
    {
        gameManager.GoIdleState();
        slider.interactable = true;
        group.gameObject.SetActive(true);
        bkg.gameObject.SetActive(true);
        anim.SetTrigger(reset);
        Debug.Log("Executed");
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        vp.url = JsonSaver.instance.fixedUrl[0];
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (slider.interactable)
        {
            if (slider.value >= 0.99f)
            {
                slider.interactable = false;
                vp.Play();
                Invoke(nameof(PlayAnim),0.5f);
                bkg.DOFade(0,0.25f).OnComplete(() =>
                {
                    bkg.gameObject.SetActive(false);
                    bkg.alpha = 1;
                });
                group.DOFade(0, 0.25f).OnComplete(() =>
                {
                    group.gameObject.SetActive(false);
                    group.alpha = 1;
                    slider.value = 0;
                });
                gameManager.StartGame();
                Invoke(nameof(DisableWebcam),24f);
            }
            else
            {
                slider.value = 0f;
            }
        }
    }

    private void DisableWebcam()
    {
        gameManager.StopWebcam();
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
