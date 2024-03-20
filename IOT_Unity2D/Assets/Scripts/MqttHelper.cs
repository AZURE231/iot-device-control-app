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
    [Header("User Interface")]
    public InputField consoleInputField;
    public Toggle encryptedToggle;
    public InputField addressInputField;
    public InputField portInputField;
    public Button connectButton;
    public Button disconnectButton;
    public Button testPublishButton;
    public Button clearButton;

    private List<string> eventMessages = new List<string>();
    private bool updateUI = false;

    [Header("Real User Interface")]
    public TextMeshProUGUI objText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI lightText;
    public ButtonControl lightButton;
    public ButtonControl pumpButton;


    public void TestPublish()
    {
        //client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        client.Publish("tuanhuynh231/feeds/ai", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("Test message published");
        AddUiMessage("Test message published.");
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
        client.Publish("tuanhuynh231/feeds/button2", System.Text.Encoding.UTF8.GetBytes(pumpButton.state.ToString()),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }

    public void SetBrokerAddress(string brokerAddress)
    {
        if (addressInputField && !updateUI)
        {
            this.brokerAddress = brokerAddress;
        }
    }

    public void SetBrokerPort(string brokerPort)
    {
        if (portInputField && !updateUI)
        {
            int.TryParse(brokerPort, out this.brokerPort);
        }
    }

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }


    public void SetUiMessage(string msg)
    {
        if (consoleInputField != null)
        {
            consoleInputField.text = msg;
            updateUI = true;
        }
    }

    public void AddUiMessage(string msg)
    {
        if (consoleInputField != null)
        {
            consoleInputField.text += msg + "\n";
            updateUI = true;
        }
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
        SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        SetUiMessage("Connected to broker on " + brokerAddress + "\n");

        if (autoTest)
        {
            TestPublish();
        }
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

    protected override void OnConnectionFailed(string errorMessage)
    {
        AddUiMessage("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        AddUiMessage("Disconnected.");
    }

    protected override void OnConnectionLost()
    {
        AddUiMessage("CONNECTION LOST!");
    }

    private void UpdateUI()
    {
        if (client == null)
        {
            if (connectButton != null)
            {
                connectButton.interactable = true;
                disconnectButton.interactable = false;
                testPublishButton.interactable = false;
            }
        }
        else
        {
            if (testPublishButton != null)
            {
                testPublishButton.interactable = client.IsConnected;
            }
            if (disconnectButton != null)
            {
                disconnectButton.interactable = client.IsConnected;
            }
            if (connectButton != null)
            {
                connectButton.interactable = !client.IsConnected;
            }
        }
        if (addressInputField != null && connectButton != null)
        {
            addressInputField.interactable = connectButton.interactable;
            addressInputField.text = brokerAddress;
        }
        if (portInputField != null && connectButton != null)
        {
            portInputField.interactable = connectButton.interactable;
            portInputField.text = brokerPort.ToString();
        }
        if (encryptedToggle != null && connectButton != null)
        {
            encryptedToggle.interactable = connectButton.interactable;
            encryptedToggle.isOn = isEncrypted;
        }
        if (clearButton != null && connectButton != null)
        {
            clearButton.interactable = connectButton.interactable;
        }
        updateUI = false;
    }

    protected override void Start()
    {
        SetUiMessage("Ready.");
        updateUI = true;
        base.Start();
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

        if (topic == "M2MQTT_Unity/test")
        {
            if (autoTest)
            {
                autoTest = false;
                Disconnect();
            }
        }
    }

    private void StoreMessage(string eventMsg)
    {
        eventMessages.Add(eventMsg);
    }

    private void ProcessMessage(string msg)
    {
        AddUiMessage("Received: " + msg);
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
        if (updateUI)
        {
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }
}
