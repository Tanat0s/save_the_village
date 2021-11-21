using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip soundSelect;
    private AudioSource source { get { return GetComponent<AudioSource>(); } }


    void Start()
    {
        gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(soundSelect);
    }
}
