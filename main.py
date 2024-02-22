import sys
import random
import time
from Adafruit_IO import MQTTClient


AIO_FEED_IDs = "button1"
AIO_USERNAME = "tuanhuynh231"
AIO_KEY = "aio_eMOZ03y2hVZLHvNGdvCM0pb4t49c"

def connected(client):
    print("Ket noi thanh cong...") 
    client.subscribe(AIO_FEED_IDs)

def subscribe(client , userdata , mid , granted_qos):
    print("Subcribe thanh cong...")

def disconnected(client): 
    print("Ngat ket noi...") 
    sys.exit (1)

def message(client , feed_id , payload): 
    print("Nhan du lieu: " + payload + " , feed id:" + feed_id)

client = MQTTClient(AIO_USERNAME , AIO_KEY) 
client.on_connect = connected 
client.on_disconnect = disconnected 
client.on_message = message 
client.on_subscribe = subscribe 
client.connect()
client.loop_background()
counter = 10
while True: 
    counter = counter - 1
    if counter <= 0:
        counter = 10
        client.publish("sensor1", random.randint(10, 20))
        client.publish("sensor2", random.randint(50, 70))
        client.publish("sensor3", random.randint(100, 500))

    time.sleep(1)