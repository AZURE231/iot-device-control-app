using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M2MqttUnity;

public class ReadInput : MonoBehaviour
{
    private string addressInput = "";
    private string portInput = "";
    private string usernameInput = "";
    private string passwordInput = "";
    private string sensor1Input = "";
    private string sensor2Input = "";
    private string button1Input = "";
    private string button2Input = "";


    public string getUsername()
    {
        return this.usernameInput;
    }

    public string getPassword()
    {
        return this.passwordInput;
    }

    public string getAddress()
    {
        return this.addressInput;
    }

    public int getPort()
    {
        if (this.portInput == "")
            return 0;
        return int.Parse(this.portInput);
    }

    public string getSensor1Input()
    {
        return this.sensor1Input;
    }

    public string getSensor2Input()
    {
        return this.sensor2Input;
    }

    public string getButton1Input()
    {
        return this.button1Input;
    }

    public string getButton2Input()
    {
        return this.button2Input;
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
