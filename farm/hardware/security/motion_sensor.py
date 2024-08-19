from time import sleep
from grove.gpio import GPIO
from grove.grove_mini_pir_motion_sensor import GroveMiniPIRMotionSensor
from ..sensors import ISensor, AReading


class Motion_Sensor(ISensor):

    def __init__(self, gpio: int, model: str, type: AReading.Type):

        self.pir = GroveMiniPIRMotionSensor(gpio)
        self.model = model
        self.type = type
        self.motion = ''

    def read_sensor(self) -> list[AReading]:
        def callback():
            self.motion = 'Motion detected.'

        try:
            self.pir._on_detect = callback
        except BaseException:
            print("Failed to configure motion sensor. Retrying on next read.")

        if self.motion == '':
            motion = 0
            self.motion = ''
        else:
            motion = 1
            self.motion = ''

        return [
            AReading(AReading.Type.MOTION, AReading.Unit.MOTION, motion),
        ]


def main():
    pir = Motion_Sensor(22, "PIR", AReading.Type.MOTION)

    while True:
        mt = pir.read_sensor()
        print(f"{mt[0].value} ")
        sleep(1)


if __name__ == '__main__':
    main()
