from time import sleep
from hardware.sensors import ISensor, AReading
from hardware.actuators import IActuator, ACommand


class DeviceController:

    def __init__(
            self,
            sensors: list[ISensor],
            actuators: list[IActuator]) -> None:
        """DeviceController constructor to manage a group of sensors and actuators.

        :param list[ISensor] sensors: List of sensors to be read.
        :param list[IActuator] actuators: List of actuators to be controlled.
        """
        self._sensors = sensors
        self._actuators = actuators

    def read_sensors(self) -> list[AReading]:
        """Reads data from all sensors.

        :return list[AReading]: a list containing all readings collected from sensors.
        """
        readings: list[AReading] = []
        for sensor in self._sensors:
            sensor_readings = sensor.read_sensor()
            if sensor_readings:
                readings.extend(sensor_readings)

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

    TEST_SLEEP_TIME = 2

    sensors: list[ISensor] = [
        # Include testing sensors here.
        TemperatureSensor("AH20", AReading.Type.TEMPERATURE),
    ]
    actuators: list[IActuator] = [
        # Include testing actuators here.
        PulseLight(18),
        Fan()
    ]

    device_manager = DeviceController(sensors, actuators)

    while True:
        print(device_manager.read_sensors())

        # Message body should minimally include a key named "value" with
        # a value that is parsable inside the specific actuator.
        fake_led_message_body = '{"value": 3.5}'
        fake_led_command = ACommand(
            ACommand.Type.LIGHT_PULSE, fake_led_message_body)

        fake_fan_message_body = '{"value": "on"}'
        fake_fan_command = ACommand(
            ACommand.Type.FAN, fake_fan_message_body)

        device_manager.control_actuator(fake_led_command)
        device_manager.control_actuator(fake_fan_command)

        sleep(TEST_SLEEP_TIME)
