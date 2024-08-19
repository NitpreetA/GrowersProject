#!/usr/bin/env python
from enum import Enum
# import evdev
from time import sleep
import math

import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from ..sensors import ISensor, AReading

# Credit to William for helping me revise my code.


class Acc_Sensor(ISensor):

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self.gpio = gpio
        self.device = rt.get_acceleration_device()
        self.model = model
        self.type = type

    def read_sensor(self) -> list[AReading]:
        readings = self.sort_readings(self.get_readings())
        readings.append(
            self.calculate_pitch(
                readings[0].value,
                readings[1].value,
                readings[2].value))
        readings.append(
            self.calculate_roll(
                readings[0].value,
                readings[1].value,
                readings[2].value))
        readings.append(
            self.calculate_yaw(
                readings[0].value,
                readings[1].value,
                readings[2].value))
        return readings

    def get_readings(self) -> list[AReading]:
        x = False
        y = False
        z = False
        readings = []
        for event in self.device.read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)
            name = str(accelEvent.name)
            if name == "AccelerationName.X":
                readings.append(
                    AReading(
                        AReading.Type.ACCELERATIONX,
                        AReading.Unit.ACCELERATIONX,
                        float(
                            accelEvent.value)))
                x = True
            elif name == "AccelerationName.Y":
                readings.append(
                    AReading(
                        AReading.Type.ACCELERATIONY,
                        AReading.Unit.ACCELERATIONY,
                        float(
                            accelEvent.value)))
                y = True
            elif name == "AccelerationName.Z":
                readings.append(
                    AReading(
                        AReading.Type.ACCELERATIONZ,
                        AReading.Unit.ACCELERATIONZ,
                        float(
                            accelEvent.value)))
                z = True
            if x and y and z:
                return readings

    def sort_readings(self, readings: list[AReading]) -> list[AReading]:
        x = {}
        y = {}
        z = {}
        for reading in readings:
            if reading.reading_unit == AReading.Unit.ACCELERATIONX:
                x = reading
            elif reading.reading_unit == AReading.Unit.ACCELERATIONY:
                y = reading
            elif reading.reading_unit == AReading.Unit.ACCELERATIONZ:
                z = reading

        return [x, y, z]

    def calculate_pitch(self, x: float, y: float, z: float) -> AReading:
        pitch = 180 * math.atan2(x, math.sqrt((y * y) + (z * z))) / math.pi
        return AReading(
            AReading.Type.PITCH,
            AReading.Unit.PITCH_ANGLE,
            pitch)

    def calculate_yaw(self, x: float, y: float, z: float) -> AReading:
        yaw = 180 * math.atan2(z, math.sqrt((x * x) + (y * y))) / math.pi
        return AReading(
            AReading.Type.YAW, 
            AReading.Unit.YAW_ANGLE, 
            yaw)

    def calculate_roll(self, x: float, y: float, z: float) -> AReading:
        roll = 180 * math.atan2(y, math.sqrt((x * x) + (z * z))) / math.pi
        return AReading(
            AReading.Type.ROLL,
            AReading.Unit.ROLL_ANGLE,
            roll)


def main():
    acc = Acc_Sensor(1, "reTerminal", AReading.Type.ACCELERATION)
    while True:
        print(acc.read_sensor())
        sleep(5)


if __name__ == "__main__":
    main()
