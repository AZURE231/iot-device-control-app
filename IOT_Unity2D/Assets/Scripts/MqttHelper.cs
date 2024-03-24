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

    bool Validate()
    {
        if (readInput.getAddress() == "")
        {
            this.message = "Address can not be empty./n";
            return false;
        }
        if (readInput.getPort() == 0)
        {
            this.message = "Port can not be empty./n";
            return false;
        }
        if (readInput.getUsername() == "")
        {
            this.message = "Username can not be empty./n";
            return false;
        }
        if (readInput.getPassword() == "")
        {
            this.message = "Password can not be empty./n";
            return false;
        }
        return true;
    }

    public bool setProperty()
    {
        if (!Validate())
        {
            canvas.SetMessagePannel(true, this.message);
            return false;
        }
        this.setAddress(readInput.getAddress());
        this.setPort(readInput.getPort());
        this.setUsername(readInput.getUsername());
        this.setPassword(readInput.getPassword());

        this.sensor1 = readInput.getSensor1Input();
        this.sensor2 = readInput.getSensor2Input();
        this.button1 = readInput.getButton1Input();
        this.button2 = readInput.getButton2Input();
        return true;
    }

    public override void Connect()
    {
        if (!setProperty()) return;
        printProperty();

        StartCoroutine(LoadingConnect());
    }

    IEnumerator LoadingConnect()
    {
        canvas.SwiftUpEnter();
        base.Connect();
        yield return new WaitForSeconds(1);
        canvas.SwiftUpOut();
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        canvas.StartButtonClicked();
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

        if (lightButton.state)
        {
            client.Publish("tuanhuynh231/feeds/button1", System.Text.Encoding.UTF8.GetBytes("1"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            lightButton.ButtonOn();
        }
        else
        {
            client.Publish("tuanhuynh231/feeds/button1", System.Text.Encoding.UTF8.GetBytes("0"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            lightButton.ButtonOff();
        }
        lightButton.state = !lightButton.state;
    }

    public void PumpButtonPublish()
    {
        if (pumpButton.state)
        {
            client.Publish("tuanhuynh231/feeds/button2", System.Text.Encoding.UTF8.GetBytes("1"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            pumpButton.ButtonOn();
        }
        else
        {
            client.Publish("tuanhuynh231/feeds/button2", System.Text.Encoding.UTF8.GetBytes("0"),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            pumpButton.ButtonOff();
        }
        pumpButton.state = !pumpButton.state;
    }


    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "tuanhuynh231/feeds/button1", "tuanhuynh231/feeds/button2",
            "tuanhuynh231/feeds/sensor1", "tuanhuynh231/feeds/sensor2"
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

        if (topic == "tuanhuynh231/feeds/sensor1")
        {
            temperatureText.SetText(msg);
        }
        if (topic == "tuanhuynh231/feeds/sensor2")
        {
            lightText.SetText(msg);
        }
        if (topic == "tuanhuynh231/feeds/button1")
        {
            if (msg == "1")
            {
                lightButton.ButtonOn();
            }
            if (msg == "0")
            {
                lightButton.ButtonOff();
            }
        }
        if (topic == "tuanhuynh231/feeds/button2")
        {
            if (msg == "1")
            {
                pumpButton.ButtonOn();
            }
            if (msg == "0")
            {
                pumpButton.ButtonOff();
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
