from time import sleep

from gpiozero import MotionSensor
from gpiozero import LED

pir = MotionSensor(17)
led = LED(12)


def detectMotion():
    motionDetected = False
    while motionDetected == False:
        pir.wait_for_motion()
        pir.wait_for_no_motion()
        print("Movement Detected")
        motionDetected = True
        # test_email.request_message(3)
        # led.active_high()
        # sleep(1)
        # led.closed()
    return True

if __name__ == "__main__":
    detectMotion()