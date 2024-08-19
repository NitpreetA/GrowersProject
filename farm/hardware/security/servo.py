from gpiozero import Servo
from time import sleep
from ..actuators import IActuator, ACommand


class ServoActuator(IActuator):

    def __init__(
            self,
            gpio: int,
            type: ACommand.Type,
            initial_state: dict = {"value": "CLOSE"}) -> None:
        self.servo = Servo(gpio)
        self._current_state = initial_state["value"]
        self.type = type

    def validate_command(self, command: ACommand) -> bool:
        return command.target_type == self.type

    def control_actuator(self, data: dict) -> bool:
        servo_data = data["value"].upper()
        if (servo_data != "OPEN" and servo_data != "CLOSE"):
            raise ValueError(
                'Servo door sate must be a a string of "open" or "close"')
        if servo_data != self._current_state:
            self._current_state = servo_data
            if servo_data == "OPEN":
                self.servo.max()
            elif servo_data == "CLOSE":
                self.servo.min()
            return True
        else:
            return False


def main():
    servo = ServoActuator(24, ACommand.Type.SERVO, {"value": "close"})
    servo.control_actuator({"value": "open"})
    print(f"Servo state: {servo._current_state}")
    sleep(2)
    servo.control_actuator({"value": "close"})
    print(f"Servo state: {servo._current_state}")
    sleep(2)


if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        pass
