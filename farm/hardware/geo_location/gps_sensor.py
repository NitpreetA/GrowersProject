#!/usr/bin/env python

from time import sleep
import serial
from ..sensors import ISensor, AReading
import pynmea2
import math


class GPS_Sensor(ISensor):

    def __init__(self, gpio: int, model: str, type: AReading.Type) -> None:

        self.serial = serial.Serial('/dev/ttyAMA0', 9600, timeout=1)
        self.serial.reset_input_buffer()
        self.serial.flush()
        self.model = model
        self.type = type

    def parse_gps_data(self, line: str) -> list[AReading]:
        readings = []

        try:
            lat = None
            lon = None
            msg = pynmea2.parse(line)

            if msg.sentence_type == 'GGA':
                lat = float(msg.lat)
                lon = float(msg.lon)

            if lat is not None:
                if msg.lat_dir == 'S':
                    lat = lat * -1
                readings.append(
                    AReading(
                        AReading.Type.LOCATION_LATITUDE,
                        AReading.Unit.LATITUDE,
                        lat))

            if lon is not None:
                if msg.lon_dir == 'W':
                    lon = lon * -1

                readings.append(
                    AReading(
                        AReading.Type.LOCATION_LONGITUDE,
                        AReading.Unit.LONGITUDE,
                        lon))

        except BaseException:
            # This will just return readings
            pass

        return readings

    def read_sensor(self) -> list[AReading]:
        try:
            line = self.serial.readline().decode('utf-8')
            readings = []

            while len(line) > 0:
                line = self.serial.readline().decode('utf-8')
                readings.extend(self.parse_gps_data(line))

                if (len(readings) > 0):
                    return readings
        except BaseException:
            return []


def main():

    gps = GPS_Sensor(1, "Air530", AReading.Type.LOCATION_LONGITUDE)

    while True:
        readings = gps.read_sensor()
        if readings is not None:
            for reading in readings:
                print(f'{reading.reading_type}: {reading.value}')

        sleep(1)


if __name__ == '__main__':
    main()
