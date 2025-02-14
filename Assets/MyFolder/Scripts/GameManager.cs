using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RawImage display;
    
    private WebCamTexture camTexture;
    
    private AudioManager audioManager;

    private CancellationTokenSource cts;
    [SerializeField] private GameObject blackDisplay;
    [SerializeField] private SliderControl sliderControl;
    private DisableOnAlpha disableOnAlpha;
    
    private void MakeCts()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        
        Debug.Log("CreateCts");
    }

    private void CancelCts()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
        Debug.Log("DeleteCts");
    }

    //////////////////////////////////////////////////////////////
    ///
    //////////////////////////////////////////////////////////////

    private void Awake()
    {
        Debug.Log("Awake");
        instance = this;
        disableOnAlpha = GetComponent<DisableOnAlpha>();
    }

    private void Start()
    {
        Debug.Log("Start");
        audioManager = AudioManager.instance;
        camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        GoIdleState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GoIdleState();
            sliderControl.ReturnIdle();
        }
    }

    public void StartGame()
    {
        audioManager.AcceptCall();
        CancelCts();
    }

    private void GoIdleState()
    {
        MakeCts();
        IdleState(cts.Token).Forget();
    }
    
    private async UniTaskVoid IdleState(CancellationToken ct)
    {
        while (true)
        {
            disableOnAlpha.SetActiveTrue();
            StopWebcam();
            await UniTask.Delay(20000, cancellationToken:ct);
            
            disableOnAlpha.SetActiveFalse();
            LoadWebcam();
            await UniTask.Delay(15000, cancellationToken:ct);
        }
    }
    
    private void LoadWebcam()
    {
        display.texture = camTexture;
        camTexture.Play();
        audioManager.PlayCall();
    }
    private void StopWebcam()
    {
        camTexture.Stop();
        audioManager.StopCall();
    }
}
