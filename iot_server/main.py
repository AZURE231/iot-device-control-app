import sys
import random
import time
from Adafruit_IO import MQTTClient
from simpleAI import *
import cv2
from uart import *


AIO_FEED_IDs = ["button2", "button1"]
AIO_USERNAME = "tuanhuynh231"
AIO_KEY = "aio_eMOZ03y2hVZLHvNGdvCM0pb4t49c"

def connected(client):
    print("Ket noi thanh cong...") 
    for topic in AIO_FEED_IDs:
        client.subscribe(topic)

def subscribe(client , userdata , mid , granted_qos):
    print("Subcribe thanh cong...")

def disconnected(client): 
    print("Ngat ket noi...") 
    sys.exit (1)

def message(client , feed_id , payload): 
    print("Nhan du lieu: " + payload + " , feed id:" + feed_id)
    if feed_id == "button1":
        if payload == "0":
            writeData(1)
        else:
            writeData(2)

    if feed_id == "button2":
        if payload == "0":
            writeData(3)
        else:
            writeData(4)
    

client = MQTTClient(AIO_USERNAME , AIO_KEY) 
client.on_connect = connected 
client.on_disconnect = disconnected 
client.on_message = message 
client.on_subscribe = subscribe 
client.connect()
client.loop_background()
counter = 20
counter_ai = 5
previous_result = ""
while True: 
    counter = counter - 1
    # if counter <= 0:
    #     counter = 10
    #     client.publish("sensor1", random.randint(10, 20))
    #     client.publish("sensor2", random.randint(50, 70))
    #     client.publish("sensor3", random.randint(100, 500))

    

    counter_ai = counter_ai - 1
    if counter_ai <= 0:
        counter_ai = 5
        class_name = image_detector()
        previous_result = class_name
        if previous_result != class_name:
            print(class_name)
            client.publish("ai", class_name)
    readSerial(client)
    time.sleep(1)

    # keyboard_input = cv2.waitKey(1)
    # if keyboard_input == 27:
    #     exit()
    #     break
    