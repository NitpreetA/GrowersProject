import seeed_python_reterminal.core as rt
from time import sleep
from ..sensors import ISensor, AReading


class ReTerminalSensor(ISensor):
    """
    A sensor class for reading Temperature and/or Humidity

    Implements the ISensor interface

    Attributes:
        gpio (int): I'm actually using this as the bus number...
        model (str): The model of the temperature controller.
        type (AReading.Type): The type of the sensor reading.

    Methods:
        read_sensor(): Reads the temperature and humidity from the sensor.

    """

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        """
        Initialize a reTerminal sensor object.

        Args:
            gpio (int): The GPIO pin number.
            model (str): The model of the temperature and humidity sensor.
            type (AReading.Type): The type of reading (e.g., temperature or humidity).

        Returns:
            None

        """
        self.gpio = gpio
        self.model = model
        self.type = type

    def read_sensor(self) -> list[AReading]:
        """
        Takes a reading from the temperature and humidity sensor.

        Args: none

        Returns:
            list[AReading]: a list of temperature and humidity readings.

        """
        luminosity = rt.illuminance
        return [
            AReading(AReading.Type.LUMINOSITY, AReading.Unit.LUX, luminosity),
        ]


def main():
    lum_sensor = ReTerminalSensor(4, "reTerminal", AReading.Type.LUMINOSITY)

    while True:
        lum = lum_sensor.read_sensor()
        print(f"Luminosity: {lum[0].value} lx")
        sleep(1)


if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        pass
