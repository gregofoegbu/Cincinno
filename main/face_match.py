import fnmatch
import os
from matplotlib import pyplot as plt
import cv2
from facenet_pytorch import MTCNN, InceptionResnetV1

resnet = InceptionResnetV1(pretrained='vggface2').eval()
# Load the cascade
face_cascade = cv2.CascadeClassifier('/haarcascade_frontalface_default.xml')


def face_match(img_path, data_path):  # img_path= location of photo, data_path= location of data.pt
    # getting embedding matrix of the given img
    img = cv2.imread(img_path)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    # Detect faces
    faces = face_cascade.detectMultiScale(gray, 1.1, 4)
    # Draw rectangle around the faces
    for (x, y, w, h) in faces:
        crop_face = img[y:y + h, x:x + w]

    img = cv2.imwrite(img_path, crop_face)
    emb = resnet(img.unsqueeze(0)).detach()  # detech is to make required gradient false

    saved_data = torch.load('model.pt')  # loading data.pt file
    embedding_list = saved_data[0]  # getting embedding data
    name_list = saved_data[1]  # getting list of names
    dist_list = []  # list of matched distances, minimum distance is used to identify the person

    for idx, emb_db in enumerate(embedding_list):
        dist = torch.dist(emb, emb_db).item()
        dist_list.append(dist)

    idx_min = dist_list.index(min(dist_list))
    return (name_list[idx_min], min(dist_list))


result = face_match('trainset/0006/0006_0000546/0006_0000546_script.jpg', '/model.pt')

print('Face matched with: ', result[0], 'With distance: ', result[1])