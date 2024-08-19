from time import sleep
from hardware.sensors import ISensor, AReading
from hardware.actuators import IActuator, ACommand
from . import Buzzer, MagneticDoor, Motion_Sensor, ReTerminalSensor, ServoActuator, SoundSensor


class SecurityDeviceController:

    def __init__(self) -> None:
        """DeviceController constructor to manage a group of sensors and actuators.

        :param list[ISensor] sensors: List of sensors to be read.
        :param list[IActuator] actuators: List of actuators to be controlled.
        """
        self._sensors = [
            SoundSensor(
                22, "Sound Sensor", AReading.Type.SOUND), ReTerminalSensor(
                4, "reTerminal", AReading.Type.LUMINOSITY), Motion_Sensor(
                22, "PIR", AReading.Type.MOTION), MagneticDoor(
                    16, "Magnetic door sensor reed switch", AReading.Type.MAGDOOR)]
        self._actuators = [
            Buzzer(1, ACommand.Type.BUZZER, {"value": "off"}),
            ServoActuator(24, ACommand.Type.SERVO, {"value": "CLOSED"}),
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

    device_manager = SecurityDeviceController()

    while True:
        print(device_manager.read_sensors())

        # Message body should minimally include a key named "value" with
        # a value that is parsable inside the specific actuator.
        fake_servo_message_body = '{"value":"open"}'
        fake_servo_command = ACommand(
            ACommand.Type.SERVO, fake_servo_message_body)

        fake_on_message_body = '{"value":"off"}'
        fake_on_command = ACommand(
            ACommand.Type.BUZZER, fake_on_message_body)

        device_manager.control_actuator(fake_servo_command)
        device_manager.control_actuator(fake_on_command)

        sleep(2)

        fake_servo_message_body = '{"value":"close"}'
        fake_servo_command = ACommand(
            ACommand.Type.SERVO, fake_servo_message_body)

        fake_on_message_body = '{"value":"off"}'
        fake_on_command = ACommand(
            ACommand.Type.BUZZER, fake_on_message_body)

        device_manager.control_actuator(fake_servo_command)
        device_manager.control_actuator(fake_on_command)

        sleep(2)
