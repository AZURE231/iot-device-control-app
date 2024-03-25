[System.Serializable]

public class ServerData
{
    public string address;
    public int port;
    public string username;
    public string password;

    public string sensor1;
    public string sensor2;
    public string button1;
    public string button2;

    public ServerData(ServerProps props)
    {
        this.address = props.address;
        this.port = props.port;
        this.username = props.username;
        this.password = props.password;

        this.sensor1 = props.sensor1;
        this.sensor2 = props.sensor2;
        this.button1 = props.button1;
        this.button2 = props.button2;
    }
}
