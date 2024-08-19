from grove.adc import ADC
from grove.i2c import Bus
from ..sensors import ISensor, AReading
import time


class CustomADC(ADC):
    def __init__(self, address=0x04, bus=1):
        self.address = address
        self.bus = Bus(bus)


class MoistureSensor(ISensor):

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self.gpio = gpio
        self._sensor_model = model
        self.reading_type = type
        self.sensor = CustomADC()

    def read_sensor(self) -> list[AReading]:
        moisture = self.sensor.read(self.gpio)
        return [
            AReading(AReading.Type.MOISTURE, AReading.Unit.UNITLESS, moisture)
        ]


if __name__ == "__main__":
    sensor = MoistureSensor(
        0x04,
        "Grove - Capacitive Moisture Sensor (Corrosion Resistant)",
        AReading.Type.MOISTURE)
    while True:
        print(sensor.read_sensor()[0])
        time.sleep(0.5)
