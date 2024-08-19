from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from ..actuators import IActuator, ACommand
from rpi_ws281x import Color


class LEDStripActuator(IActuator):
    def __init__(
            self,
            gpio: int,
            type: ACommand.Type,
            initial_state: dict = {"value": "OFF"}) -> None:
        self.gpio = gpio
        self.strip = GroveWS2813RgbStrip(gpio, 10, 255)
        self._current_state = initial_state["value"]
        self.type = type

    def validate_command(self, command: ACommand) -> bool:
        led_data = command.data["value"].upper()
        if led_data not in ["ON", "OFF"]:
            return False

        return command.target_type == self.type

    def _set_color_all_leds(self, color: Color):
        for i in range(self.strip.numPixels()):
            self.strip.setPixelColor(i, color)
        self.strip.show()

    def control_actuator(self, data: dict) -> bool:
        led_data = data["value"].upper()
        if led_data not in ["ON", "OFF"]:
            raise ValueError('Led state must be a a string of "on" or "off"')

        if led_data != self._current_state:
            self._current_state = led_data
            match led_data:
                case "ON":
                    self._set_color_all_leds(Color(255, 255, 255))
                case "OFF":
                    self._set_color_all_leds(Color(0, 0, 0))
            return True

        return False

        '''previous_state = self._current_state

        if command.target_type == ACommand.Type.LEDSTRIP_ON_OFF:
            self._current_state = command.value
            match command.value:
                case "ON":
                    self._set_color_all_leds(Color(255, 255, 255))
                case "OFF":
                    self._set_color_all_leds(Color(0, 0, 0))
            return previous_state != self._current_state

        return False
        '''


if __name__ == "__main__":
    led = LEDStripActuator(12, ACommand.Type.LEDSTRIP_ON_OFF, {"value": "OFF"})
    import time
    while True:
        led.control_actuator({"value": "on"})
        time.sleep(1)
        led.control_actuator({"value": "OFF"})
        time.sleep(1)
