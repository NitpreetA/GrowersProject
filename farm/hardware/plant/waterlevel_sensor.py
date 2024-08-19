from grove.adc import ADC
from grove.i2c import Bus
import time

from ..sensors import ISensor, AReading


class CustomADC(ADC):
    def __init__(self, address=0x04, bus=1):
        self.address = address
        self.bus = Bus(bus)

# Table for mapping voltage to water height in mm:
# https://files.waveshare.com/upload/7/78/Liquid-Level-Sensor-UserManual.pdf
# Function to approximate height:
# f(x) = 0.0239875x^(8.35289)+0.130124


def voltage_to_height_mm(voltage):
    if voltage > 0:
        return (0.0239875 * (voltage ** 8.35289)) + 0.130124
    return 0


class WaterLevelSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self.gpio = gpio
        self._sensor_model = model
        self.sensor = CustomADC()
        self.reading_type = type

    def read_sensor(self) -> list[AReading]:
        voltage = self.sensor.read_voltage(self.gpio) / 1000
        waterlevel = voltage_to_height_mm(voltage)
        return [
            AReading(
                AReading.Type.WATER_LEVEL,
                AReading.Unit.MILLIMITERS,
                waterlevel)]


if __name__ == "__main__":
    sensor = WaterLevelSensor(
        0x08,
        "Liquid Level Sensor",
        AReading.Type.WATER_LEVEL)
    while True:
        print(sensor.read_sensor()[0])
        time.sleep(0.5)
