[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/ZKGepmmw)
# <div align='center'> 420-6A6-AB Application Development III
# <div align='center'> 420-6P3-AB Connected Objects 
# <div align='center'> Winter 2024

## <div align='center'> Final Project

# Project Description
The purpose of this project is to provide farmer technicians and fleet owners an easy way to take care of the container they own/manage. We aim to allow owners to take care of the security of their containers, and for farmers a way to view the status of the container such as temperature, humidity and more to enable best growth for the plant. For the security section the sensors and actuators are the loudness sensor, magnetic door sensor, servo actuator, luminosity and the buzzer which are built into the ReTerminal. For the geo-location there is the GPS location sensor, pitch and roll angles which are built in the ReTerminal. For the plant subsystem there are the temperature, humidity, relative water levels, soil moisture levels sensors, and the fan and LED actuators. 

Our engineers at Growers.IO have created a python solution to process the readings from these sensors, and created methods to turn on and off the actuators. When the sensor data is processed it is then sent to Azure IotHub. From the mobile app we can retrieve the data from Azure IotHub and display it for both user roles. Another feature within the mobile app the farmers and owners will be able to send commands by C2D messaging. 


# Team Information
Growers.IO by:
- Aidan Grant-Ushinsky | 2183276
- Jose David Torres Garvin | 2135164
- Nitpreet Arneja | 2128811

# Contributions

| Team Memebers | Contribution                                | Main Pages                               |
|---------------|---------------------------------------------|------------------------------------------|
| Jose          | Responsible for mainly UI and some backend  | Home, Profile, Container Info            |
| Nitpreet      | Responsible for both UI and some backend    | Container Info, Containers, AddContainer |
| Aidan         | Responsible for mostly backend, and some UI | Container Info, Containers, AddContainer |
| All           |                                             | Sign in, Sign up, Welcome                |

# Mobile App

## App Overview
The mobile app welcomes the user to a user friendly welcome page which provides the user with two options either to sign up if they don't have an account created or sign in page if they have an account already. Within the sign up page they will choose their role either farmer or owner and once logged in it will navigate to certain pages depending on your role. The user will then be greeted by the specified home page with options to go to the containers page. 
Both roles have access to a flyout menu where they can view their profile or sign out.
For farmers, the home page will display the containers and current readings, in the containers page it will show the containers they were assigned to. When clicking on a container it will display container information only for the plant sub system. Users will be able to view live data and graphs displaying past and new data. 
For the owners, the home page has a map displaying all the containers with live time location with a button to add a container. Within the add container page they can enter a name, add in their connection string, device id and select farmers they wish to the container. Adding a container will populate the farmer containers. Within the owners container page they have all the containers they own and within the container info page they have access to all 3 subsystem tabs.

### Features
- Live updates from IotHub
- Controlling actuators
- Graphs
- Maps
- User Roles
- Container management

#### Login/Signup
> [!NOTE]  
> Some of the reading's units are incorrect due to the test data being used.

#### Signin
![Login](/doc_pictures/Login.png) 
#### Signup
![Signup](/doc_pictures/Signup.png)
#### Add Container
![Add Container](/doc_pictures/AddContainer.png)
#### Container List
![Container List](/doc_pictures/ContainerList.png)
#### Home Page (Technician)
![Technician Home](/doc_pictures/FarmerHome.png)
#### Home Page (Fleet Owner)
![Fleet Owner](/doc_pictures/OwnerHome.png)
#### Container Page (Technician View)
![Container Page for technicians](/doc_pictures/ContainerPage.png)
#### Plant (Technician/FleetOwner View)
![Plant subsystem for fleet owner](/doc_pictures/Plant_Graphs.png)
#### Security (FleetOwner View)
![Security subsystem for fleet owner](/doc_pictures/Security_Graphs.png)
#### GeoLocation (FleetOwner View)
![GeoLocation subsystem for fleet owner](/doc_pictures/GeoLocation_Graphs.png)

## UML Diagram
![UML Diagram](/class-diagrams.png)

## App Setup
> Create a Firebase app with `Authentication` enabled using email & password, and a realtime database.

`FirebaseDomain`:
- Firebase -> Authentication -> Settings -> Authorised Domains -> Copy domain ending in `.com`

`FirebaseApiKey`:
- Firebase -> Project settings -> General -> Copy `Web API key`

`FirebaseDatabase`:
- Firebase -> Realtime Database -> Data -> Copy database URL

> Create a Azure IoTHub

`EventHubString`:
- Azure IoT Hub -> Hub settings -> Built-in endpoints -> Copy `Event Hub-compatible endpoint`

`IoTHubString`:
- Azure Cloud Shell
- Execute: `az iot hub connection-string show --hub-name grantushinsky-iothub --output table`
- Copy `ConnectionString`

## Future Work

We had enough time to develop all the features we planned in this project, including the nice-to-haves, but as a future improvement to the project we could make the multiple container system more scalable by optimizing the amount of data retrieved from the hub to improve the user's experience as well as the overall cost of the app. Also, we would like to add an option to edit and delete containers since that feature is currently missing. The alert feature, the profile data customization we described in the proposal were cancelled.

The app does not have any known bugs.

## Bonus Features

### Multiple Containers
#### Description
The app and IoTHub can have any number of devices. To use the feature, a new device must be created on the IoTHub. On the connected objects side, the farm will use the device's connection string from the `.env` file. And on the app side, when creating a container, simply input the deviceId from before.
#### How does it work?
The features works by listening to all events on the IoTHub using `EventHubConsumerClient`, and looking at the `iothub-connection-device-id` found in the `SystemProperties` of the event. This allows us to determine which container the reading belongs to. This solution works very well, but it won't scale well because it listens all events across all devices, which may affect performance if many devices are sending messages.
#### Solution
We must listen for events from devices that are relevant to the currently authenticated user. This can be solved by modifying the `EventHubConsumerClient` implementation to use `EventProcessorClient` instead, and creating an `Event Hubs namespace` and a `Storage account` on Azure.

# Connected Objects
## Device GPIO
| Device                               | Pin  |
|--------------------------------------|------|
| ReTerminal                           | N/A  |
| Air530                               | UART |
| Fan                                  | 5    |
| AHT20                                | 26   |
| Grove - WS2813 RGB LED Strip         | 18   |
| Grove - Capacitive Moisture Sensor   | A0   |
| Grove - Adjustable PIR Motion Sensor | 22   |
| Grove - Loudness Sensor              | A2   |
| SEC-100 Magnetic Door Sensor         | 16   |
| MG90S 180° Micro Servo               | 24   |
| Liquid Level Sensor                  | A4   |
| Buzzer                               | N/A  |
| Accelerometer                        | N/A  |
| Luminosity                           | N/A  |

## Controlling Actuators

Choice of Communication: C2D Messages

Explanation: We chose to use Cloud to Device Messages because we deemed the one-way communication is good to control actuators, nonetheless Direct Methods would have been useful as well since they return a confirmation for the commands execution.

### Fan Actuator
Turning ON (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `fan`

Body:
- `{"value": "ON"}`
        
Turning ON (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "ON"}' --props 'command-type=fan'`

Turning OFF (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `fan`

Body:
- `{"value": "OFF"}`
        
Turning OFF (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "OFF"}' --props 'command-type=fan'`

### Buzzer Actuator 
Turning ON (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `buzzer`

Body:
- `{"value": "ON"}`
        
Turning ON (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "ON"}' --props 'command-type=buzzer'`

Turning OFF (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `buzzer`

Body:
- `{"value": "OFF"}`
        
Turning OFF (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "OFF"}' --props 'command-type=buzzer'`

### LED Actuator
Turning ON (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `led`

Body:
- `{"value": "ON"}`
        
Turning ON (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "ON"}' --props 'command-type=led'`

Turning OFF (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `led`

Body:
- `{"value": "OFF"}`
        
Turning OFF (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "OFF"}' --props 'command-type=led'`

### Servo Actuator
Turning ON (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `servo`

Body:
- `{"value": "ON"}`
        
Turning ON (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "ON"}' --props 'command-type=servo'`

Turning OFF (Azure Message)
---
Custom Property
- Key: `command-type`
- Value: `servo`

Body:
- `{"value": "OFF"}`
        
Turning OFF (Azure C2D Command)
---
`az iot device c2d-message send -d device1 -n grantushinsky-iothub --data '{"value": "OFF"}' --props 'command-type=servo'`

## D2C Messages (sending readings)
Sending Temperature
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Temperature' --data "{'value': 25, 'unit': 'C'}"`

Sending Humidity
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Humidity' --data "{'value': 40, 'unit': '% HR'}"`

Sending WaterLevel
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=WaterLevel' --data "{'value': 2.5, 'unit': 'mm'}"`

Sending SoilMoisture
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=SoilMoisture' --data "{'value': 600, 'unit': 'unitless'}"`

Sending NoiseLevel
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=NoiseLevel' --data "{'value': 520, 'unit': 'sound ratio'}"`

Sending Luminosity
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Luminosity' --data "{'value': 100, 'unit': 'lx'}"`

Sending Motion
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Motion' --data "{'value': 1, 'unit': 'unitless'}"`

Sending DoorState
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=DoorState' --data "{'value': 1, 'unit': 'door state'}"`

Sending GPSLongitude
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=GPSLongitude' --data "{'value': -75.55, 'unit': 'degrees'}"`

Sending GPSLatitude
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=GPSLatitude' --data "{'value': 45.53, 'unit': 'degrees'}"`

Sending Pitch
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Pitch' --data "{'value': 65, 'unit': 'degrees'}"`

Sending Yaw
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Yaw' --data "{'value': 20, 'unit': 'degrees'}"`

Sending Roll
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=Roll' --data "{'value': 45, 'unit': 'degrees'}"`

Sending AccelerationX
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=AccelerationX' --data "{'value': 85, 'unit': 'accx'}"`

Sending AccelerationY
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=AccelerationY' --data "{'value': 27, 'unit': 'accy'}"`

Sending AccelerationZ
---
`az iot device send-d2c-message -d device1 -n grantushinsky-iothub --props 'reading-type=AccelerationZ' --data "{'value': -24, 'unit': 'accz'}"`