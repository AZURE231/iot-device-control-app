using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject homePanel;
    [SerializeField] GameObject configurationPanel;
    [SerializeField] GameObject feedPanel;
    [Header("Effect")]
    [SerializeField] Animator transition;

    [Header("Message panel")]
    [SerializeField] GameObject messagePannel;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Image messageTexture;
    [SerializeField] Sprite errorTexture;
    [SerializeField] Sprite successTexture;

    public void Start()
    {
        messagePannel.SetActive(false);
        SetActiveFalseAllPanel();
        homePanel.SetActive(true);
    }

    public void SetMessagePannel(bool isError, string message)
    {
        messagePannel.SetActive(true);
        messageTexture.sprite = isError ? errorTexture : successTexture;
        this.message.text = message;
    }

    public void CloseErrorPannel()
    {
        messagePannel.SetActive(false);
        Debug.Log("Close pannel");
    }

    public void SwiftRightEnter()
    {
        transition.SetTrigger("SwiftLeftEnter");
    }

    public void SwiftRightOut()
    {
        transition.SetTrigger("SwifLeftOut");
    }

    public void SwiftUpEnter()
    {
        transition.SetTrigger("SwiftTopEnter");
    }

    public void SwiftUpOut()
    {
        transition.SetTrigger("SwiftTopOut");
    }

    public void StartButtonClicked()
    {
        SetActiveFalseAllPanel();
        mainPanel.SetActive(true);
    }

    public void OpenConfiguration()
    {
        SetActiveFalseAllPanel();
        configurationPanel.SetActive(true);
    }

    public void NextConfigurationButton()
    {
        //SwiftRightEnter();
        //SwiftRightOut();
        SetActiveFalseAllPanel();
        feedPanel.SetActive(true);

    }

    public void BackConfigurationButton()
    {
        //SwiftRightEnter();
        //SwiftRightOut();
        SetActiveFalseAllPanel();
        homePanel.SetActive(true);

    }

    public void BackFeedButton()
    {
        //particle.Play();
        //SwiftRightEnter();
        //SwiftRightOut();
        SetActiveFalseAllPanel();
        configurationPanel.SetActive(true);

    }

    public void SetActiveFalseAllPanel()
    {
        mainPanel.SetActive(false);
        homePanel.SetActive(false);
        configurationPanel.SetActive(false);
        feedPanel.SetActive(false);
    }
}
