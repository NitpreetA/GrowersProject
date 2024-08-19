from time import sleep
from hardware.sensors import AReading
from hardware.actuators import ACommand
from . import WaterLevelSensor, MoistureSensor, HumiditySensor, TemperatureSensor, FanActuator, LEDStripActuator


class PlantDeviceController:

    def __init__(self) -> None:
        """DeviceController constructor to manage a group of sensors and actuators.

        :param list[ISensor] sensors: List of sensors to be read.
        :param list[IActuator] actuators: List of actuators to be controlled.
        """
        self._sensors = [
            WaterLevelSensor(
                4,
                "Liquid Level Sensor",
                AReading.Type.WATER_LEVEL),
            MoistureSensor(
                0,
                "Grove - Capacitive Moisture Sensor (Corrosion Resistant)",
                AReading.Type.MOISTURE),
            HumiditySensor(
                4,
                "AHT20",
                AReading.Type.HUMIDITY),
            TemperatureSensor(
                4,
                "AHT20",
                AReading.Type.TEMPERATURE),
        ]
        self._actuators = [
            FanActuator(
                5, ACommand.Type.FAN, {
                    "value": "off"}),
            LEDStripActuator(
                18, ACommand.Type.LEDSTRIP_ON_OFF, {
                    "value": "off"}),
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

    device_manager = PlantDeviceController()

    while True:
        print(device_manager.read_sensors())

        # Message body should minimally include a key named "value" with
        # a value that is parsable inside the specific actuator.
        turn_led_on_command_body = '{"value":"ON"}'
        turn_led_on_command = ACommand(
            ACommand.Type.LEDSTRIP_ON_OFF, turn_led_on_command_body)

        turn_fan_on_command_body = '{"value":"ON"}'
        turn_fan_on_command = ACommand(
            ACommand.Type.FAN, turn_fan_on_command_body)

        device_manager.control_actuator(turn_led_on_command)
        device_manager.control_actuator(turn_fan_on_command)

        sleep(2)

        turn_led_off_command_body = '{"value":"OFF"}'
        turn_led_off_command = ACommand(
            ACommand.Type.LEDSTRIP_ON_OFF, turn_led_off_command_body)

        turn_fan_off_command_body = '{"value":"OFF"}'
        turn_fan_off_command = ACommand(
            ACommand.Type.FAN, turn_fan_off_command_body)

        device_manager.control_actuator(turn_led_off_command)
        device_manager.control_actuator(turn_fan_off_command)

        sleep(2)
