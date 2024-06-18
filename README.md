# IoT Devices Control App

Welcome to the IoT Devices Control App project! This repository contains the code for an IoT control application developed in Unity2D that communicates with an Adafruit server using MQTT. The project includes a Unity application for device control and a Python server for publishing sensor data and AI mask recognition results.

## Overview

The IoT Devices Control App allows users to control IoT devices and monitor sensor data through a Unity2D interface. The application uses MQTT to communicate with an Adafruit server, sending and receiving data. Additionally, the Python server publishes sensor data and AI mask recognition results to the Adafruit server.

## Features

- **Device Control**: Control IoT devices from the Unity2D application.
- **Sensor Data Monitoring**: Receive and display real-time sensor data from the Adafruit server.
- **AI Mask Recognition**: Publish AI mask recognition results from the Python server to the Adafruit server.
- **Configuration**: Configure and save Adafruit connection settings within the Unity2D app.
- **Cross-Platform**: Run the app within Unity or export it as a standalone application.

## Technology Stack

- **Frontend**: Unity2D
- **Backend**: Python
- **Communication**: MQTT via Adafruit IO

## Getting Started

### Prerequisites

- [Unity](https://unity.com/) 2022.3.15f1
- [Python](https://www.python.org/) (3.8 or later)
- MQTT broker (Adafruit IO)
- Unity MQTT plugin ( [M2MQTT](https://github.com/ppatierno/m2mqtt))

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/AZURE231/iot-device-control-app.git
2. **Setup Unity project:**
    - Open Unity Hub
    - Click on "Add" and select the iot_unity2d folder.
    - Open the project in Unity.
3. Install python dependencies:
### Running the Application
#### Unity Application
1. **Run in Unity Editor:**
    - Open the Unity project.
    - Press the Play button to start the application.
2. **Export as Standalone App:**
    - Go to File > Build Settings.
    - Select your target platform and click on "Build".
    - Follow the prompts to export the application.
#### Python Server
You only need to run the main.py file at iot_server directory.
```bash
    python main.py
```
### Configuring Adafruit Connection
1. Open the Unity application.
2. Navigate to the EventSystem in the scene hierarchy and look for Mqtt Helper in Inspector.
3. Enter your Adafruit IO credentials and connection details.
4. Save the configuration.
## Contributing
We welcome contributions to this project! If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are warmly welcome.
## License
This project is licensed under the MIT License
## Contact
For any inquiries, please contact huynhvotuan231@gmail.com.

