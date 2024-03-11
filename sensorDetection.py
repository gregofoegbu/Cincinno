from time import sleep

from gpiozero import MotionSensor
from gpiozero import LED

import test_email

pir = MotionSensor(4)
led = LED(12)


def detectMotion():
    motionDetected = False
    while motionDetected == False:
        pir.wait_for_motion()
        print("Movement Detected")
        motionDetected = True
        test_email.request_message(3)
        led.active_high()
        sleep(1)
        led.closed()


    return True

