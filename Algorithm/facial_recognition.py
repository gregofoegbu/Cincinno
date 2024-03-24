#! /usr/bin/python
# import the necessary packages
import time

import cv2
import face_recognition
# import torch
# from facenet_pytorch import InceptionResnetV1, MTCNN
import imutils
from imutils.video import FPS

from api import get_threshold, get_userID


def callGetUserID(device_id):
    response = get_userID()
    return response


def callGetThreshold(user_id):
    response = get_threshold()
    return response

def facial_recognition(data, vs, threshold):
    # Initialize 'currentname' to trigger only when a new person is identified.
    currentname = "unknown"

    approvedFace = False
    approvedName = []

    # initialize the video stream and allow the camera sensor to warm up
    # Set the ser to the followng
    # src = 0 : for the build in single web cam, could be your laptop webcam
    # src = 2 : I had to set it to 2 inorder to use the USB webcam attached to my laptop
    # vs = VideoStream(src=0, framerate=50).start()
    # vs = VideoStream(usePiCamera=True, framerate=50).start()

    time.sleep(5.0)

    # start the FPS counter
    fps = FPS().start()
    startTime = time.time()

    # loop over frames from the video file stream
    while True:
        # grab the frame from the threaded video stream and resize it
        # to 500px (to speedup processing)
        frame = vs.read()
        frame = imutils.resize(frame, width=500)
        # frame = cv2.resize(frame, (0, 0), fx=0.5, fy=0.5)
        # Detect the face boxes
        boxes = face_recognition.face_locations(frame, model='hog')
        # compute the facial embeddings for each face bounding box
        encodings = face_recognition.face_encodings(frame, boxes)
        names = []

        # loop over the facial embeddings
        for encoding in encodings:
            # attempt to match each face in the input image to our known
            # encodings
            matches = face_recognition.compare_faces(data["encodings"],
                                                     encoding, (threshold/100))
            name = "Unknown"  # if face is not recognized, then print Unknown

            # check to see if we have found a match
            if True in matches:
                # find the indexes of all matched faces then initialize a
                # dictionary to count the total number of times each face
                # was matched
                matchedIdxs = [i for (i, b) in enumerate(matches) if b]
                counts = {}

                # loop over the matched indexes and maintain a count for
                # each recognized face
                for i in matchedIdxs:
                    name = data["names"][i]
                    counts[name] = counts.get(name, 0) + 1

                # determine the recognized face with the largest number
                # of votes (note: in the event of an unlikely tie Python
                # will select first entry in the dictionary)
                name = max(counts, key=counts.get)

                # If someone in your dataset is identified, print their name on the screen
                if currentname != name:
                    currentname = name
                    print(currentname)
                    approvedFace = True
                    approvedName = [True, currentname]

            # update the list of names
            names.append(name)

        # loop over the recognized faces
        for ((top, right, bottom, left), name) in zip(boxes, names):
            # draw the predicted face name on the image - color is in BGR
            cv2.rectangle(frame, (left, top), (right, bottom),
                          (0, 255, 225), 2)
            y = top - 15 if top - 15 > 15 else top + 15
            cv2.putText(frame, name, (left, y), cv2.FONT_HERSHEY_SIMPLEX,
                        .8, (0, 255, 255), 2)

        # display the image to our screen
        cv2.imshow("Facial Recognition is Running", frame)
        key = cv2.waitKey(1) & 0xFF

        # quit when 'q' key is pressed
        if key == ord("q") or approvedFace == True:
            break

        endTime = time.time()

        comparedTime = endTime - startTime

        if approvedFace == False and comparedTime >30.0:
            break

        # update the FPS counter
        fps.update()

    # stop the timer and display FPS information
    fps.stop()
    print("[INFO] elapsed time: {:.2f}".format(fps.elapsed()))
    print("[INFO] approx. FPS: {:.2f}".format(fps.fps()))

    # do a bit of cleanup
    cv2.destroyAllWindows()
    vs.stop()

    if approvedFace == True:
        return approvedName
    else:
        return [False, "Unknown"]
