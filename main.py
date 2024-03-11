from sympy import true

import facial_req
import facial_req2
import headshots
import sensorDetection
import servo
import test_email
import train_model

if __name__ == "__main__":
    headshotAddition = input('Do you need to add additional people? Please enter 1 for Yes and 2 for No\n')
    headshotAdditionInteger = int(headshotAddition)
    while (headshotAdditionInteger != 1) and (headshotAdditionInteger != 2):
        headshotAdditionInteger = int(input('Sorry that is an invalid response. Please enter 1 for Yes and 2 for No\n'))

    application_running = True
    while application_running:
        if headshotAddition == '1':
            headshots.headshotCapture()
            train_model.train_model()
            motionDetected = sensorDetection.detectMotion()
            if motionDetected:
                facial_req.facial_recognition()

        else:
            train_model.train_model()
            motionDetected = sensorDetection.detectMotion()
            if motionDetected:
                approvedFace = facial_req2.detect()

                if approvedFace[0]:
                    print("Approved Face has been found: " + approvedFace[1])
                    test_email.request_message(1)
                    servo.Unlock()
                    locked = False
                    print("The door is Unlocked")

                elif not approvedFace[0]:
                    print("Unknown Face Detected!")
                    print("Entrance is not allowed")
                    test_email.request_message(2)






