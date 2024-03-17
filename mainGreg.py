import pickle
from time import sleep

from imutils.video import VideoStream

# import sensorDetection
# import servo
# import test_email
from Algorithm import headshots, facial_req2, train_model
import os

if __name__ == "__main__":
    # headshotAddition = input('Do you need to add additional people? Please enter 1 for Yes and 2 for No\n')
    # headshotAdditionInteger = int(headshotAddition)
    # while (headshotAdditionInteger != 1) and (headshotAdditionInteger != 2):
    #     headshotAdditionInteger = int(input('Sorry that is an invalid response. Please enter 1 for Yes and 2 for No\n'))

    application_running = True
    pickleFound = False

    directory = os.getcwd()

    for file in os.listdir(directory):
        if file.endswith(".pickle"):
            pickleFound = True
            break

    if pickleFound == False:
        headshots.headshotCapture()
        train_model.train_model()
    else:
        train_model.train_model()

    # Determine faces from encodings.pickle file model created from train_model.py
    encodingsP = "encodings.pickle"

    # load the known faces and embeddings along with OpenCV's Haar
    # cascade for face detection
    print("[INFO] loading encodings + face detector...")
    data = pickle.loads(open(encodingsP, "rb").read())

    while application_running:
        # motionDetected = sensorDetection.detectMotion()
        # if motionDetected:
            # vs = VideoStream(usePiCamera=True, framerate=15, resolution=(1296, 976)).start()
            vs = VideoStream(src=0, framerate=50).start()
            approvedFace = facial_req2.facial_recognition(data, vs)
            if approvedFace[0]:
                print("Approved Face has been found: " + approvedFace[1])
                # test_email.request_message(1)
                # servo.Unlock()
                # locked = False
                # print("The door is Unlocked")
                # sleep(15)
                # servo.Lock()
                # print("The door has Locked")

            elif not approvedFace[0]:
                print("Unknown Face Detected!")
                print("Entrance is not allowed")
            #     test_email.request_message(2)

            vs.stop()
            sleep(5)
