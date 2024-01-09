using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject sprite;

    [SerializeField] private TextMeshProUGUI text;
    private Animator animator;
    private bool isTextActive;
    

    private void Update()
    {
        this.text.text = Daylight_Manager.current.currentTime.Hour.ToString() + ":00";
    }

    private void Start()
    {
        this.isTextActive = false;
        animator = this.gameObject.GetComponent<Animator>();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }


    public void hideTimer()
    {
        isTextActive = false;
        animator.Play("HourSignAnimation_Close");
        StartCoroutine(hideObject());
    }

    public void showTimer()
    {
        isTextActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        animator.Play("HourSignAnimation_Open");
    }


    private IEnumerator hideObject()
    {
        while (this.animator.GetCurrentAnimatorStateInfo(0).IsName("HourSignAnimation_Close"))
        {
            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public bool isActive() { return this.isTextActive; }
}
