from device_controller import DeviceController
from connection_manager import ConnectionManager

from hardware.sensors import ISensor, AReading
from hardware.actuators import IActuator, ACommand

from hardware.geo_location import Acc_Sensor, GPSBuzzer, GPS_Sensor

import asyncio


class Geo_Location:

    DEBUG = True
    LOOP_INTERVAL = 4  # in seconds

    def __init__(self) -> None:

        sensors: list[ISensor] = [
            Acc_Sensor(1, "reTerminal", AReading.Type.ACCELERATION),
            GPS_Sensor(0, "Air530", AReading.Type.LOCATION)
        ]
        actuators: list[IActuator] = [
            GPSBuzzer(1, ACommand.Type.BUZZER, {"value": "off"})
        ]

        self._device_manager = DeviceController(sensors, actuators)
        self._connection_manager = ConnectionManager()

    async def loop(self) -> None:
        await self._connection_manager.connect()
        self._connection_manager.register_command_callback(
            self._device_manager.control_actuator)

        while True:
            readings = self._device_manager.read_sensors()
            if self.DEBUG:
                print(readings)

            await self._connection_manager.send_readings(readings)
            await asyncio.sleep(self.LOOP_INTERVAL)


async def geo_location_main():
    gl = Geo_Location()
    await gl.loop()

if __name__ == "__main__":
    asyncio.run(geo_location_main())
