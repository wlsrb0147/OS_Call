using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject blackDisplay;
    [SerializeField] private SliderControl sliderControl;
    public static GameManager instance;
    
    private AudioManager audioManager;

    private CancellationTokenSource cts;
    private DisableOnAlpha disableOnAlpha;
    
    #region webcam
    
    public RawImage display;
    private WebCamTexture camTexture;
    
    private Texture2D frameTex;
    private CamSettings camSettings;
    private float frameTimer;
    private float frameInterval;
    
    #endregion

    private int enableTime;
    private int disableTime;
    
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

        
        /*camTexture = new WebCamTexture(
            WebCamTexture.devices[0].name,
            1080, 1920, 30
        );

        frameTex = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
        display.texture = frameTex;*/
    }

    private void Start()
    {
        Debug.Log("Start");
        audioManager = AudioManager.instance;
        
        camSettings = JsonSaver.instance.Settings.camSettings;
        
        camTexture = new WebCamTexture(
            WebCamTexture.devices[0].name,
            camSettings.width,
            camSettings.height,
            camSettings.frameRate
        );
        
        frameTex = new Texture2D(camTexture.requestedWidth, camTexture.requestedHeight);
        frameInterval = 1f / camTexture.requestedFPS;
        enableTime = Convert.ToInt32(JsonSaver.instance.Settings.enableTime * 1000) ;
        disableTime = Convert.ToInt32(JsonSaver.instance.Settings.disableTime * 1000);
        
        GoIdleState();
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GoIdleState();
            sliderControl.ReturnIdle();
        }
        
        if (camTexture.isPlaying && camTexture.width > 16)
        {
            // WebCamTexture에서 픽셀 가져오기
            Color[] pixels = camTexture.GetPixels();
            
            // Texture2D에 덮어쓰기
            frameTex.SetPixels(pixels);
            frameTex.Apply(); // 적용 필수
        }
    }
    
    public void StartGame()
    {
        audioManager.AcceptCall();
        CancelCts();
    }

    public void GoIdleState()
    {
        MakeCts();
        IdleState(cts.Token).Forget();
    }
    
    private async UniTaskVoid IdleState(CancellationToken ct)
    {
        while (true)
        {
            disableOnAlpha.SetActiveFalse();
            LoadWebcam();
            await UniTask.Delay(enableTime, cancellationToken:ct);
            
            disableOnAlpha.SetActiveTrue();
            StopWebcam();
            await UniTask.Delay(disableTime, cancellationToken:ct);
        }
    }
    
    private void LoadWebcam()
    {
        display.color = Color.white;
        if (!camTexture.isPlaying)
        {
            camTexture.Play();
            Debug.Log("Webcam Started");
        }
        display.texture = frameTex;
        audioManager.PlayCall();
    }

    public void StopWebcam()
    {
        if (camTexture.isPlaying)
        {
            camTexture.Stop();
            Debug.Log("Webcam Stopped");
        }

        display.texture = null;
        audioManager.StopCall();
        display.color = Color.clear;
    }
}
