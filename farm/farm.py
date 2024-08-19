from device_controller import DeviceController
from connection_manager import ConnectionManager
from hardware.sensors import ISensor, AReading
from hardware.actuators import IActuator, ACommand
from hardware.security.security_dc import SecurityDeviceController
from hardware.plant.plant_dc import PlantDeviceController
from hardware.geo_location.geolocation_dc import GeoLocationDeviceController

from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodRequest, MethodResponse

import asyncio
import json


class Farm():
    DEBUG = True
    DEFAULT_LOOP_INTERVAL = 5  # in seconds
    INTERVAL_KEY = "telemetryInterval"

    def __init__(self) -> None:
        security_controller = SecurityDeviceController()
        plant_controller = PlantDeviceController()
        geolocation_controller = GeoLocationDeviceController()

        sensors: list[ISensor] = []
        actuators: list[IActuator] = []

        sensors.extend(security_controller._sensors)
        sensors.extend(plant_controller._sensors)
        sensors.extend(geolocation_controller._sensors)

        actuators.extend(security_controller._actuators)
        actuators.extend(plant_controller._actuators)
        actuators.extend(geolocation_controller._actuators)

        self._sensors = sensors
        self._actuators = actuators

        self._device_manager = DeviceController(sensors, actuators)
        self._connection_manager = ConnectionManager()

        self._connection_manager.register_method_request_callback(
            self._on_method_request_received)

        self._connection_manager.register_twin_patched_callback(
            self.on_twin_desired_properties_patch_received)

        self._loop_interval = self.DEFAULT_LOOP_INTERVAL

    async def get_latest_interval(self):
        twin = (await self._connection_manager.get_twin()).get("desired")
        if self.INTERVAL_KEY in twin.keys():
            self._loop_interval = twin.get(self.INTERVAL_KEY)

    async def _on_method_request_received(
            self,
            client: IoTHubDeviceClient,
            method_request: MethodRequest):
        # Default no found response:
        response_status = 404
        response_payload = {"details": "method name unknown"}

        # is_online
        if method_request.name == "is_online":
            response_status = 200
            response_payload = {}  # Empty payload.
        elif method_request.name == "control_actuator":
            raw_json = json.dumps(method_request.payload)
            command_type = ACommand.Type(
                method_request.payload["command-type"])
            command = ACommand(command_type, raw_json)
            self.command_callback(command)

            response_status = 200
            response_payload = {"value": "success"}
        elif method_request.name == "get_actuator_value":
            command_type = method_request.payload["command-type"]
            try:
                actuator = next(
                    act for act in self._actuators if act.type.value == command_type)

                response_status = 200
                response_payload = {"value": actuator._current_state}
            except StopIteration:
                response_status = 400
                response_payload = {"details": "unknown command-type"}

        # Create & send response
        method_response = MethodResponse(
            method_request.request_id,
            response_status,
            response_payload)
        await client.send_method_response(method_response)

    def on_twin_desired_properties_patch_received(
            self, _: IoTHubDeviceClient, twin: dict):
        if self.INTERVAL_KEY in twin.keys():
            self._loop_interval = twin.get(self.INTERVAL_KEY)

    def command_callback(self, command: ACommand):
        self._device_manager.control_actuator(command)
        if self.DEBUG:
            print(f"Changing {command.target_type} to {command.data}")

    async def loop(self) -> None:
        """Main loop of the HVAC System. Collect new readings, send them to connection
        manager, collect new commands and dispatch them to device manager.
        """

        await self._connection_manager.connect()
        self._connection_manager.register_command_callback(
            self.command_callback)

        while True:
            # Collect new readings
            readings = self._device_manager.read_sensors()
            if self.DEBUG:
                print("========================================")
                for reading in readings:
                    print(
                        f"{reading.reading_type.value}: {reading.value}{reading.reading_unit.value}")

            # Send collected readings
            await self._connection_manager.send_readings(readings)
            await asyncio.sleep(self._loop_interval)


# This script is intented to be used as a module
async def farm_main():
    farm = Farm()
    await farm.get_latest_interval()
    await farm.loop()

if __name__ == "__main__":
    asyncio.run(farm_main())
