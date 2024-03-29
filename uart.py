import serial.tools.list_ports

def getPort():
    ports = serial.tools.list_ports.comports()
    N = len(ports)
    commPort = "None"
    for i in range(0, N):
        port = ports[i]
        strPort = str(port)
        if "USB Serial Device" in strPort:
            splitPort = strPort.split(" ")
            commPort = (splitPort[0])
    # return commPort
    return "/dev/ttys009"

if getPort() != "None":
    ser = serial.Serial(port = getPort(), baudrate = 115200)
    print(ser)

def processData(client, data):
    print("processing data")
    print(data)
    data = data.replace("@", "")
    data = data.replace("*", "")
    splitData = data.split(":")
    if splitData[1] == "T":
        client.publish("sensor1", splitData[2])
        print("Publishing " + splitData[2] + " to sensor1")
    elif splitData[1] == "H":
        client.publish("sensor2", splitData[2])
        print("Publishing " + splitData[2] + " to sensor2")
    elif splitData[1] == "B":
        if (splitData[2] == "0"):
            client.publish("button1", "0")
            print("Publishing 0 to button1")
        else:
            client.publish("button1", "1")
            print("Publishing 1 to button1")
    elif splitData[1] == "L":
        if (splitData[2] == "0"):
            client.publish("button2", "0")
            print("Publishing 0 to button2")
        else:
            client.publish("button2", "1")
            print("Publishing 1 to button2")

mess = ""
def readSerial(client):
    bytesToRead = ser.in_waiting

    if bytesToRead > 0:
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        print("Received data:", mess)

        while "@" in mess and "*" in mess:
            start = mess.find("@")
            end = mess.find("*")
            processData(client, mess[start:end + 1])

            if end == len(mess):
                mess = ""
            else:
                mess = mess[end + 1:]

def writeData(data):
    ser.write(str(data).encode("UTF-8"))

