using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonAnimation : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public float animationTime = 0.2f;
    public float targetScale = 1.5f;

    public Ease easeIn;
    public Ease easeOut;
    private float originalScale;
    // Start is called before the first frame update
    void Awake()
    {
        originalScale = transform.localScale.x;
    }


    private void OnEnable()
    {
        transform.localScale = new Vector3(originalScale, originalScale, originalScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(targetScale, animationTime).SetEase(easeIn).SetUpdate(true);
        print("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, animationTime).SetEase(easeOut).SetUpdate(true);
        print("OnPointerExit");
    }
}
