from time import sleep
from hardware.sensors import AReading
from hardware.actuators import ACommand
from . import Acc_Sensor, GPS_Sensor, GPSBuzzer


class GeoLocationDeviceController:

    def __init__(self) -> None:
        """DeviceController constructor to manage a group of sensors and actuators.

        :param list[ISensor] sensors: List of sensors to be read.
        :param list[IActuator] actuators: List of actuators to be controlled.
        """
        self._sensors = [
            GPS_Sensor(0, "Air530", AReading.Type.LOCATION_LONGITUDE),
            Acc_Sensor(0, "reTerminal", AReading.Type.ACCELERATIONX)
        ]
        self._actuators = [
            GPSBuzzer(1, ACommand.Type.BUZZER, {"value": "off"}),
        ]

    def read_sensors(self) -> list[AReading]:
        """Reads data from all sensors.

        :return list[AReading]: a list containing all readings collected from sensors.
        """
        readings: list[AReading] = []
        for sensor in self._sensors:
            readings.extend(sensor.read_sensor())
        return readings

    def control_actuator(self, command: ACommand) -> None:
        """Controls actuators according to a command.

        :param ACommand command: the command to be dispatched to the corresponding actuator.
        """

        for actuator in self._actuators:
            if actuator.validate_command(command):
                actuator.control_actuator(command.data)
                break

        pass


"""This script is intented to be used as a module, however, code below can be used for testing.
"""

if __name__ == "__main__":

    device_manager = GeoLocationDeviceController()

    while True:
        print(device_manager.read_sensors())

        # Message body should minimally include a key named "value" with
        # a value that is parsable inside the specific actuator.
        fake_on_message_body = '{"value":"on"}'
        fake_on_command = ACommand(
            ACommand.Type.BUZZER, fake_on_message_body)

        device_manager.control_actuator(fake_on_command)

        sleep(2)

        fake_off_message_body = '{"value":"off"}'
        fake_off_command = ACommand(
            ACommand.Type.BUZZER, fake_off_message_body)

        device_manager.cofftrol_actuator(fake_off_command)

        sleep(2)
