using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    private string addressInput;
    private string portInput;
    private string usernameInput;
    private string passwordInput;
    private string sensor1Input;
    private string sensor2Input;
    private string button1Input;
    private string button2Input;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadAddressInput(string s)
    {
        addressInput = s;
    }

    public void ReadPortInput(string s)
    {
        portInput = s;
    }

    public void ReadUsernameInput(string s)
    {
        usernameInput = s;
    }

    public void ReadPasswordInput(string s)
    {
        passwordInput = s;
    }
    public void ReadSensor1Input(string s)
    {
        sensor1Input = s;
    }
    public void ReadSensor2Input(string s)
    {
        sensor2Input = s;
    }
    public void ReadButton1Input(string s)
    {
        button1Input = s;
    }
    public void ReadButton2Input(string s)
    {
        button2Input = s;
    }
}
