import fnmatch
import os
from matplotlib import pyplot as plt
from facenet_pytorch import MTCNN, InceptionResnetV1
import torch
import os

import cv2
from facenet_pytorch import MTCNN, InceptionResnetV1

resnet = InceptionResnetV1(pretrained='vggface2').eval()
# Load the cascade
# face_cascade = cv2.CascadeClassifier('/haarcascade_frontalface_default.xml')
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

def face_match(img_path, data_path):  # img_path= location of photo, data_path= location of data.pt
    img = cv2.imread(img_path)
    if img is None:
        raise FileNotFoundError(f"Failed to read the image file at: {img_path}")

    # Convert the image to RGB if it has more than 3 channels
    if img.shape[2] > 3:
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    # Detect faces
    faces = face_cascade.detectMultiScale(gray, 1.1, 4)

    crop_face = None

    # Draw rectangle around the faces
    for (x, y, w, h) in faces:
        crop_face = img[y:y + h, x:x + w]

    # Check if any face is detected
    if crop_face is None:
        raise RuntimeError("No face detected in the image.")

    gray_image = cv2.cvtColor(crop_face, cv2.COLOR_RGB2GRAY)

    img = cv2.imwrite('savedImage.jpg', gray_image)
    newimg = cv2.imread('savedImage.jpg')
    print("After saving image:")
    # Image directory
    directory = r'C:\Users\zack-\PycharmProjects\Cincinno\main'
    print(os.listdir(directory))

    i = torch.tensor(newimg)

    crop_face_resized = cv2.resize(crop_face, (3, 3))

    # emb = resnet(newimg.unsqueeze(0)).detach()  # detech is to make required gradient false
    emb = resnet(torch.tensor(crop_face_resized).unsqueeze(0)).detach()

    saved_data = torch.load('model.pt')  # loading data.pt file
    embedding_list = saved_data[0]  # getting embedding data
    name_list = saved_data[1]  # getting list of names
    dist_list = []  # list of matched distances, minimum distance is used to identify the person

    for idx, emb_db in enumerate(embedding_list):
        dist = torch.dist(emb, emb_db).item()
        dist_list.append(dist)

    idx_min = dist_list.index(min(dist_list))
    return (name_list[idx_min], min(dist_list))


result = face_match('testimage.jpg', '/model.pt')

print('Face matched with: ', result[0], 'With distance: ', result[1])