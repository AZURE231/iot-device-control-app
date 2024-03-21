using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject homePanel;
    [Header("Particle Effect")]
    [SerializeField] ParticleSystem particle;

    public void StartButtonClicked()
    {
        particle.Play();
        homePanel.SetActive(false);
        mainPanel.SetActive(true);


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
