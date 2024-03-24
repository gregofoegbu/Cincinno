import pickle
import shutil
from pathlib import Path
from time import sleep

from imutils.video import VideoStream

import api
# import sensorDetection
# import servo
# import test_email
from Algorithm import headshots_picam, facial_recognition, sensorDetection, train_model, servo
import os

from Algorithm.facial_recognition import callGetUserID, callGetThreshold

if __name__ == "__main__":
    application_running = True
    pickleFound = False
    DATASET_DIR = "dataset"

    directory = os.getcwd()

    for file in os.listdir(directory):
        if file.endswith(".pickle"):
            pickleFound = True
            break

    if pickleFound == False:
        headshots_picam.headshotCapturePi()
        api.get_images()
        train_model.train_model()
    else:
        if (os.path.isdir(DATASET_DIR) == True):
            shutil.rmtree(DATASET_DIR)
            Path("dataset/").mkdir(parents=True, exist_ok=True)
        api.get_images()
        train_model.train_model()

    # Determine faces from encodings.pickle file model created from train_model.py
    encodingsP = "encodings.pickle"

    # load the known faces and embeddings along with OpenCV's Haar
    # cascade for face detection
    print("[INFO] loading encodings + face detector...")
    data = pickle.loads(open(encodingsP, "rb").read())

    userId = api.get_userID()
    threshold = api.get_threshold()


    while application_running:
        motionDetected = sensorDetection.detectMotion()
        if motionDetected:
            vs = VideoStream(usePiCamera=True, framerate=15, resolution=(1296, 976)).start()
            approvedFace = facial_recognition.facial_recognition(data, vs, threshold)
            if approvedFace[0]:
                print("Approved Face has been found: " + approvedFace[1])
                # test_email.request_message(1)
                servo.Unlock()
                locked = False
                print("The door is Unlocked")
                sleep(15)
                servo.Lock()
                print("The door has Locked")

            elif not approvedFace[0]:
                print("Unknown Face Detected!")
                print("Entrance is not allowed")
            #     test_email.request_message(2)

            vs.stop()
            sleep(5)
