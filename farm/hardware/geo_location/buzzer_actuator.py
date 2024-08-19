import seeed_python_reterminal.core as rt
from time import sleep
from ..actuators import IActuator, ACommand


class GPSBuzzer(IActuator):
    def __init__(
            self,
            gpio: int,
            type: ACommand.Type,
            initial_state: dict = {"value": "OFF"}) -> None:

        self.current_state = initial_state["value"]
        self.type = type

    def validate_command(self, command: ACommand) -> bool:
        return command.target_type == self.type

    def control_actuator(self, data: dict) -> bool:
        buzzer_data = data["value"].upper()
        if (buzzer_data != "ON" and buzzer_data != "OFF"):
            raise ValueError(
                'Buzzer state must be a a string of "on" or "off"')
        if buzzer_data != self._current_state:
            self._current_state = buzzer_data
            if buzzer_data == "ON":
                rt.buzzer = True
            elif buzzer_data == "OFF":
                rt.buzzer = False
            return True
        else:
            return False


def main():
    buzzer = Buzzer(1, ACommand.Type.BUZZER, "OFF")
    buzzer.control_actuator(ACommand(ACommand.Type.BUZZER, "ON"))
    print(f"Buzzer state: {buzzer.current_state}")
    sleep(2)
    buzzer.control_actuator(ACommand(ACommand.Type.BUZZER, "OFF"))
    print(f"Buzzer state: {buzzer.current_state}")
    sleep(2)


if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        pass
