
from gpiozero import Servo
from time import sleep

myGPIO = 26

myCorrection = 0.45
maxPW = (2.0 + myCorrection) / 1000
minPW = (1.0 - myCorrection) / 1000

servo = Servo(myGPIO, min_pulse_width=minPW, max_pulse_width=maxPW)

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
        sleep(0.5)
        servo.min()
        print("min")
        sleep(1)
        servo.mid()
        print("mid")
        sleep(0.5)
        servo.max()
        print("max")
        sleep(1)