#! /usr/bin/python
# import the necessary packages
import os
import pickle
import time
from types import MethodType

import cv2
import face_recognition
import torch
from facenet_pytorch import InceptionResnetV1, MTCNN
from imutils.video import FPS
from imutils.video import VideoStream


# helper function
def encode(img):
    res = resnet(torch.Tensor(img))
    return res


def detect_box(self, img, save_path=None):
    batch_boxes, batch_probs, batch_points = self.detect(img, landmarks=True)
    if not self.keep_all:
        batch_boxes, batch_probs, batch_points = self.select_boxes(
            batch_boxes, batch_probs, batch_points, img, method=self.selection_method
        )
    faces = self.extract(img, batch_boxes, save_path)
    return batch_boxes, faces


# Load models
resnet = InceptionResnetV1(pretrained='vggface2').eval()


def detect(cam=0, thres=0.7):
    mtcnn = MTCNN(image_size=224, keep_all=True, thresholds=[0.4, 0.5, 0.5], min_face_size=60)
    mtcnn.detect_box = MethodType(detect_box, mtcnn)

    # Process images
    saved_pictures = "./dataset/Zachery"
    all_people_faces = {}

    for file in os.listdir(saved_pictures):
        if file.endswith(".jpg") or file.endswith(".jpeg"):
            try:
                person_face = os.path.splitext(file)[0]
                img = cv2.imread(os.path.join(saved_pictures, file))
                cropped = mtcnn(img)
                if cropped is not None:
                    # Check the shape of the cropped tensor
                    print("Shape of cropped tensor:", cropped.shape)
                    all_people_faces[person_face] = encode(cropped)[0, :]
            except Exception as e:
                print(f"Error processing {file}: {e}")

    vdo = cv2.VideoCapture(cam)
    fps = FPS().start()

    while vdo.grab():
        _, img0 = vdo.retrieve()
        batch_boxes, cropped_images = mtcnn.detect_box(img0)

        if cropped_images is not None:
            for box, cropped in zip(batch_boxes, cropped_images):
                x, y, x2, y2 = [int(x) for x in box]
                img_embedding = encode(cropped.unsqueeze(0))
                detect_dict = {}
                for k, v in all_people_faces.items():
                    detect_dict[k] = (v - img_embedding).norm().item()
                min_key = min(detect_dict, key=detect_dict.get)

                if detect_dict[min_key] >= thres:
                    min_key = 'Undetected'

                cv2.rectangle(img0, (x, y), (x2, y2), (0, 0, 255), 2)
                cv2.putText(
                    img0, min_key, (x + 5, y + 10),
                    cv2.FONT_HERSHEY_DUPLEX, 0.5, (255, 255, 255), 1)

        # display
        cv2.imshow("output", img0)
        if cv2.waitKey(1) == ord('q'):
            cv2.destroyAllWindows()
            break

        # update the FPS counter
        fps.update()

        # stop the timer and display FPS information
    fps.stop()
    print("[INFO] elasped time: {:.2f}".format(fps.elapsed()))
    print("[INFO] approx. FPS: {:.2f}".format(fps.fps()))


def facial_recognition():
    # Initialize 'currentname' to trigger only when a new person is identified.
    currentname = "unknown"
    # Determine faces from encodings.pickle file model created from train_model.py
    encodingsP = "encodings.pickle"

    # load the known faces and embeddings along with OpenCV's Haar
    # cascade for face detection
    print("[INFO] loading encodings + face detector...")
    data = pickle.loads(open(encodingsP, "rb").read())

    approvedFace = False
    approvedName = []

    # initialize the video stream and allow the camera sensor to warm up
    # Set the ser to the followng
    # src = 0 : for the build in single web cam, could be your laptop webcam
    # src = 2 : I had to set it to 2 inorder to use the USB webcam attached to my laptop
    vs = VideoStream(src=0, framerate=50).start()
    # vs = VideoStream(usePiCamera=True).start()
    time.sleep(2.0)

    # start the FPS counter
    fps = FPS().start()

    # loop over frames from the video file stream
    while True:
        # grab the frame from the threaded video stream and resize it
        # to 500px (to speedup processing)
        frame = vs.read()
        # frame = imutils.resize(frame, width=500)
        frame = cv2.resize(frame, (0, 0), fx=0.5, fy=0.5)
        # Detect the fce boxes
        boxes = face_recognition.face_locations(frame)
        # compute the facial embeddings for each face bounding box
        encodings = face_recognition.face_encodings(frame, boxes)
        names = []

        # loop over the facial embeddings
        for encoding in encodings:
            # attempt to match each face in the input image to our known
            # encodings
            matches = face_recognition.compare_faces(data["encodings"],
                                                     encoding)
            name = "Unknown"  # if face is not recognized, then print Unknown

            # check to see if we have found a match
            if True in matches:
                # find the indexes of all matched faces then initialize a
                # dictionary to count the total number of times each face
                # was matched
                matchedIdxs = [i for (i, b) in enumerate(matches) if b]
                counts = {}

                # loop over the matched indexes and maintain a count for
                # each recognized face face
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

        # update the FPS counter
        fps.update()

    # stop the timer and display FPS information
    fps.stop()
    print("[INFO] elasped time: {:.2f}".format(fps.elapsed()))
    print("[INFO] approx. FPS: {:.2f}".format(fps.fps()))

    # do a bit of cleanup
    cv2.destroyAllWindows()
    vs.stop()

    if approvedFace == True:
        return approvedName
    else:
        return [False, "Unknown"]
