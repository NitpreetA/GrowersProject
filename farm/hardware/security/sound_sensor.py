import grove.i2c
from grove.adc import ADC
from time import sleep
from ..sensors import ISensor, AReading


class customADC(ADC):
    def __init__(self, address=0x04, bus=1):
        self.address = address
        self.bus = grove.i2c.Bus(bus)


class SoundSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        """Constructor for Sensor  class. May be called from childclass.

        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """
        self.adc = customADC()
        self.model = model
        self.type = type

    def read_sensor(self) -> list[AReading]:
        """Takes a reading form the sensor

        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        sound_reading = self.adc.read(2)
        return [
            AReading(AReading.Type.SOUND, AReading.Unit.SOUND, sound_reading),
        ]
        pass


def main():
    sound_sensor = SoundSensor(22, "Sound Sensor", AReading.Type.SOUND)
    while True:
        output = sound_sensor.read_sensor()
        print(f"{output[0].value} Sound ratio")
        sleep(1)


if __name__ == '__main__':
    main()
