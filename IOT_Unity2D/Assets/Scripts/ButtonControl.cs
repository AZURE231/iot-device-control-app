using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public bool state = false;
    public Sprite onSprite;
    public Sprite offSprite;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.image.sprite = gameObject.GetComponent<Image>().sprite;
    }

    public void ButtonOn()
    {
        button.image.sprite = onSprite;
    }

    public void ButtonOff()
    {
        button.image.sprite = offSprite;
    }
}
