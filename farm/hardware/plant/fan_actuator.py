from gpiozero import OutputDevice
from ..actuators import IActuator, ACommand


class FanActuator(IActuator):
    def __init__(
            self,
            gpio: int,
            type: ACommand.Type,
            initial_state: dict = {"value": "OFF"}) -> None:
        self.fan = OutputDevice(gpio)
        self._current_state = initial_state["value"]
        self.type = type
        self._fan_states = {
            "ON": 1,
            "OFF": 0
        }

    def validate_command(self, command: ACommand) -> bool:
        if command.data["value"].upper() not in self._fan_states.keys():
            return False

        # Validate the command type
        return command.target_type == self.type

    def control_actuator(self, data: dict) -> bool:
        fan_data = data["value"].upper()
        if fan_data not in self._fan_states.keys():
            raise ValueError('Fan state must be a a string of "on" or "off"')

        if fan_data != self._current_state:
            self._current_state = fan_data
            self.fan.value = self._fan_states.get(fan_data)
            return True

        return False

        '''previous_state = self._current_state
        if command.target_type == ACommand.Type.FAN:
            self.fan.value = self._fan_states.get(command.value)
            self._current_state = command.value
            return previous_state != self._current_state

        return False
        '''


if __name__ == "__main__":
    import time
    fan = FanActuator(5, ACommand.Type.FAN, {"value": "OFF"})
    while True:
        print(fan.control_actuator({"value": "OFF"}))
        time.sleep(1)
        print(fan.control_actuator({"value": "OFF"}))
        time.sleep(1)
