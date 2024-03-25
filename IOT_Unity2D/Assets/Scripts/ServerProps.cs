using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerProps
{
    public string address;
    public int port;
    public string username;
    public string password;

    public string sensor1;
    public string sensor2;
    public string button1;
    public string button2;

    public ServerProps(string address, int port, string username,
        string password, string sensor1, string sensor2, string button1, string button2)
    {
        this.address = address;
        this.port = port;
        this.username = username;
        this.password = password;

        this.sensor1 = sensor1;
        this.sensor2 = sensor2;
        this.button1 = button1;
        this.button2 = button2;
    }
}
