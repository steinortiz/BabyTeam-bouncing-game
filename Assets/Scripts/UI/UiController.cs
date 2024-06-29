using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum AnimationType
{
    NONE,
    SEMI,
    COMPLETE
}

public class UiController : MonoBehaviour
{
    [SerializeField] public Canvas pauseCanvas;
    [SerializeField] private GameObject buttonsContainer;
    
    [Header("Animacion Central")]
    [SerializeField] private RectTransform centralObj;
    [SerializeField] private Vector3 initialPosCentralObj;
    [SerializeField] private Vector3 finalPosCentralObj;
    [SerializeField] private float animCentralTime;
    [SerializeField] private LeanTweenType animCentralType;

    [Header("Animacion Bordes")]
    
    [SerializeField] private RectTransform leftObj;
    [SerializeField] private Vector3 initialPosLeftObj;
    [SerializeField] private Vector3 finalPosLeftObj;
    
    [SerializeField] private RectTransform rightObj;
    [SerializeField] private Vector3 initialPosRightObj;
    [SerializeField] private Vector3 finalPosRightObj;

    [SerializeField] private float animBordesTime;
    [SerializeField] private LeanTweenType animBordesType;
    
    [Header("Animacion Cortinas")]
    
    [SerializeField] private RectTransform cortinaSemi;
    [SerializeField] private Vector3 semiOpenPosition;
    [SerializeField] private RectTransform cortinaFull;
    [SerializeField] private Vector3 fullOpenPosition;

    [SerializeField] private float animCortinasTime;
    [SerializeField] private LeanTweenType animCortinasType;
    
    public static UiController Instance { get; private set; }

    private void Awake() 
    {

        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    private void Start()
    {

    }
    

    public void SetUP()
    {
        leftObj.localPosition = initialPosLeftObj;
        rightObj.localPosition = initialPosRightObj;
        centralObj.localPosition = initialPosCentralObj;
        buttonsContainer.SetActive(false);
    }

    public void PauseUI()
    {
        SetUP();
        pauseCanvas.enabled = true;
        animateBordes();
        animateCentral();
    }
    public void UnPauseUI()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
    }
    private void animateBordes()
    {
        Debug.Log("animation bordes");
        LeanTween.moveLocal(leftObj.gameObject, finalPosLeftObj, animBordesTime).setEase(animBordesType);
        LeanTween.moveLocal(rightObj.gameObject, finalPosRightObj, animBordesTime).setEase(animBordesType);
    }

    private void animateCentral()
    {
        Debug.Log("animation Central");
        LeanTween.moveLocal(centralObj.gameObject, finalPosCentralObj, animCentralTime).setEase(animCentralType).setOnComplete(
            () =>
            {
                buttonsContainer.SetActive(true);
                //Time.timeScale = 0;
            });
    }
    
    
   public void CloseFullCortina(UnityAction callback =null)
   {
       MoveFullCortina(Vector3.zero,callback);
   }

   public void CloseSemiCortina(Vector3 initialposition, UnityAction callback = null)
   {
       cortinaSemi.localPosition= initialposition;
       semiOpenPosition = -initialposition;
       MoveSemiCortina(Vector3.zero,callback);
   }

   private void MoveSemiCortina(Vector3 position,UnityAction callback =null)
   {
       LeanTween.moveLocal(cortinaSemi.gameObject, position, animCortinasTime).setEase(animCortinasType).setOnComplete(
           () =>
           {
               callback?.Invoke();
           });
   }
   private void MoveFullCortina(Vector3 position,UnityAction callback =null)
   {
       LeanTween.moveLocal(cortinaFull.gameObject, position, animCortinasTime).setEase(animCortinasType).setOnComplete(
           () =>
           {
               callback?.Invoke();
           });
   }
    

    private void OnAnimationLoadSceneComplete(string sceneName, bool additive)
    {
        LoadSceneMode mode = LoadSceneMode.Single;
        if(additive) mode = LoadSceneMode.Additive;
        var time = SceneManager.LoadSceneAsync(sceneName,mode);
    }

    public void OpenCortinas()
    {
        MoveFullCortina(fullOpenPosition);
        MoveSemiCortina(semiOpenPosition);
    }

}
