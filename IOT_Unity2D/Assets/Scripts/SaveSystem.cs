using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(ServerProps props)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/iot.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        ServerData data = new ServerData(props);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Data saved");
        Debug.Log(data.port);
    }

    public static ServerData LoadData()
    {
        string path = Application.persistentDataPath + "/iot.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ServerData data = formatter.Deserialize(stream) as ServerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
