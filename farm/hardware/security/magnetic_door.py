from time import sleep
from gpiozero import Button
from time import sleep
from ..sensors import ISensor, AReading


class MagneticDoor(ISensor):

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self.sensor = Button(gpio)
        self.reading_type = type
        self._sensor_model = model

    def read_sensor(self) -> list[AReading]:
        return [
            AReading(
                AReading.Type.MAGDOOR,
                AReading.Unit.MAGDOOR,
                0 if self.sensor.is_pressed else 1),
        ]


def main():
    reed_switch = MagneticDoor(
        16,
        "Magnetic door sensor reed switch",
        AReading.Type.MAGDOOR)

    while True:
        state = reed_switch.read_sensor()
        print(f"Is Door Closed: {state[0].value} ")
        sleep(1)


if __name__ == '__main__':
    main()
