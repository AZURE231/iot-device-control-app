using System.Collections;
using System.Collections.Generic;
using M2MqttUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MqttHelper : M2MqttUnityClient
{
    [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
    public bool autoTest = false;

    private List<string> eventMessages = new List<string>();

    [Header("Real User Interface")]
    public TextMeshProUGUI objText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI lightText;
    public ButtonControl lightButton;
    public ButtonControl pumpButton;
    public CanvasControl canvasControl;
    public string message;

    [Header("Canvas control")]
    public CanvasControl canvas;
    public ReadInput readInput;

    [Header("Feeds")]
    public string sensor1;
    public string sensor2;
    public string button1;
    public string button2;

    private bool isReceiveButton1 = false;
    private bool isReceiveButton2 = false;

    bool isDisconnect = false;
    bool isFetchData = false;

    bool Validate(string address, int port, string username, string password,
        string sensor1, string sensor2, string button1, string button2)
    {
        if (address == "")
        {
            this.message = "Address can not be empty.";
            return false;
        }
        if (port == 0)
        {
            this.message = "Port can not be empty.";
            return false;
        }
        if (username == "")
        {
            this.message = "Username can not be empty.";
            return false;
        }
        if (password == "")
        {
            this.message = "Password can not be empty.";
            return false;
        }
        if (sensor1 == "")
        {
            this.message = "Temperature can not be empty.";
            return false;
        }
        if (sensor2 == "")
        {
            this.message = "Light can not be empty.";
            return false;
        }
        if (button1 == "")
        {
            this.message = "Button can not be empty.";
            return false;
        }
        if (button2 == "")
        {
            this.message = "Button can not be empty.";
            return false;
        }
        return true;
    }

    bool setProperty(string address, int port, string username, string password,
        string sensor1, string sensor2, string button1, string button2)
    {
        if (!Validate(address, port, username, password, sensor1, sensor2, button1, button2)) return false;
        this.setAddress(address);
        this.setPort(port);
        this.setUsername(username);
        this.setPassword(password);

        this.sensor1 = sensor1;
        Debug.Log("set property");
        Debug.Log(this.sensor1);
        this.sensor2 = sensor2;
        this.button1 = button1;
        this.button2 = button2;
        return true;
    }

    public void ConnectOnNew()
    {

        if (!setProperty(readInput.getAddress(), readInput.getPort(),
            readInput.getUsername(), readInput.getPassword(),
            readInput.getSensor1Input(), readInput.getSensor2Input(), readInput.getButton1Input(),
            readInput.getButton2Input()))
        {
            Debug.Log("Sensor1: ");
            Debug.Log(readInput.getSensor1Input());
            canvas.SetMessagePannel(true, this.message);
            return;
        };
        this.Connect();
    }

    public void ConnectOnStart()
    {
        ServerData data = SaveSystem.LoadData();
        Debug.Log("Run start");
        if (data == null)
        {
            this.message = "No save data found!";
            canvas.SetMessagePannel(true, this.message);
            return;
        }
        if (!setProperty(data.address, data.port, data.username, data.password, data.sensor1,
            data.sensor2, data.button1, data.button2))
        {
            canvas.SetMessagePannel(true, this.message);
            return;
        };
        this.Connect();
    }

    public override void Connect()
    {
        StartCoroutine(LoadingConnect());
    }

    IEnumerator LoadingConnect()
    {
        canvas.SwiftUpEnter();
        base.Connect();
        yield return new WaitForSeconds(1);
        canvas.SwiftUpOut();
    }
    public void Signout()
    {
        StartCoroutine(LoadingDisconnect());
    }

    IEnumerator LoadingDisconnect()
    {
        canvas.SwiftUpEnter();
        base.Disconnect();
        yield return new WaitForSeconds(1);
        canvas.SwiftUpOut();
        canvas.Start();
    }


    protected override void OnConnected()
    {
        base.OnConnected();
        ServerProps data = new ServerProps(this.brokerAddress, this.brokerPort,
            this.mqttUserName, this.mqttPassword, this.sensor1, this.sensor2,
            this.button1, this.button2);
        SaveSystem.SaveData(data);
        FetchData();
        canvas.StartButtonClicked();
    }

    void FetchData()
    {
        client.Publish(this.mqttUserName + "/feeds/" + this.sensor1 + "/get", null);
        client.Publish(this.mqttUserName + "/feeds/" + this.sensor2 + "/get", null);
        client.Publish(this.mqttUserName + "/feeds/" + this.button1 + "/get", null);
        client.Publish(this.mqttUserName + "/feeds/" + this.button2 + "/get", null);
    }

    public void printProperty()
    {
        Debug.Log(this.brokerAddress);
    }

    public void TestPublish()
    {
        //client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        client.Publish("tuanhuynh231/feeds/ai", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("Test message published");
    }

    public void LightButtonPublish()
    {
        if (LostConnection())
        {
            canvas.SetMessagePannel(true, "Connection Lost");
            return;
        }
        if (lightButton.state)
        {
            client.Publish(this.mqttUserName + "/feeds/" + this.button1,
                System.Text.Encoding.UTF8.GetBytes("1"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        else
        {
            client.Publish(this.mqttUserName + "/feeds/" + this.button1,
                System.Text.Encoding.UTF8.GetBytes("0"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }

    bool LostConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            canvas.SetMessagePannel(true, "Connection Lost");
            return true;
        }
        return false;
    }

    public void PumpButtonPublish()
    {
        if (LostConnection())
        {
            canvas.SetMessagePannel(true, "Connection Lost");
            return;
        }
        if (pumpButton.state)
        {
            client.Publish(this.mqttUserName + "/feeds/" + this.button2,
                System.Text.Encoding.UTF8.GetBytes("1"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        else
        {
            client.Publish(this.mqttUserName + "/feeds/" + this.button2,
                System.Text.Encoding.UTF8.GetBytes("0"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }

    protected override void SubscribeTopics()
    {
        Debug.Log("feed name: ");
        Debug.Log(this.sensor1);
        client.Subscribe(new string[] { this.mqttUserName + "/feeds/" + this.sensor1,
            this.mqttUserName + "/feeds/" + this.sensor2,
            this.mqttUserName + "/feeds/" + this.button1,
            this.mqttUserName + "/feeds/" + this.button2
        }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE , MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
    }


    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("***Received: " + msg + " on topic: " + topic);
        objText.SetText(msg);
        StoreMessage(msg);

        if (topic == this.mqttUserName + "/feeds/" + this.sensor1)
        {
            temperatureText.SetText(msg);
        }
        if (topic == this.mqttUserName + "/feeds/" + this.sensor2)
        {
            lightText.SetText(msg);
        }
        if (topic == this.mqttUserName + "/feeds/" + this.button1)
        {
            if (msg == "1")
            {
                lightButton.ButtonOn();
                lightButton.state = false;
            }
            if (msg == "0")
            {
                lightButton.ButtonOff();
                lightButton.state = true;
            }
        }
        if (topic == this.mqttUserName + "/feeds/" + this.button2)
        {
            if (msg == "1")
            {
                pumpButton.ButtonOn();
                pumpButton.state = false;
            }
            if (msg == "0")
            {
                pumpButton.ButtonOff();
                pumpButton.state = true;
            }
        }
    }

    private void StoreMessage(string eventMsg)
    {
        eventMessages.Add(eventMsg);
    }

    private void ProcessMessage(string msg)
    {
        Debug.Log("Received: " + msg);
    }

    protected override void Update()
    {


        if (LostConnection())
        {
            isDisconnect = true;
        }
        else
        {
            isDisconnect = false;
            isFetchData = false;
        }

        if (isDisconnect && !isFetchData)
        {
            FetchData();
            isFetchData = false;
        }

        base.Update(); // call ProcessMqttEvents()
        if (eventMessages.Count > 0)
        {
            foreach (string msg in eventMessages)
            {
                ProcessMessage(msg);
            }
            eventMessages.Clear();
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        canvas.SetMessagePannel(true, errorMessage);
        base.OnConnectionFailed(errorMessage);
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }

}
