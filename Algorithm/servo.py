from gpiozero import Servo
from time import sleep
from gpiozero.pins.pigpio import PiGPIOFactory

import RPi.GPIO as GPIO
import pigpio

myGPIO = 26

servo = 26

myCorrection = 0.45
maxPW = (2.0 + myCorrection) / 1000
minPW = (1.0 - myCorrection) / 1000

pigpioFactory = PiGPIOFactory()

servo = Servo(myGPIO, min_pulse_width=minPW, max_pulse_width=maxPW, pin_factory=pigpioFactory, initial_value=minPW)


def Unlock():
    servo.max()
    print("max")


def Lock():
    servo.min()
    print("min")


def test():
    while True:
        servo.mid()
        print("mid")
        sleep(3)
        servo.min()
        print("min")
        sleep(3)
        servo.mid()
        print("mid")
        sleep(3)
        servo.max()
        print("max")
        sleep(1)



if __name__ == "__main__":
    test()
