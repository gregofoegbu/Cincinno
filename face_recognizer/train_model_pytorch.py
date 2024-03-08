import os
import cv2
import torch
from facenet_pytorch import InceptionResnetV1, MTCNN
from tqdm import tqdm
from types import MethodType
import splitfolders

splitfolders.ratio('dataset', output="data", seed=1337, ratio =(.8,0.2))


### helper function
def encode(img):
    res = resnet(torch.Tensor(img))
    return res


# Define detect_box method
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
mtcnn = MTCNN(image_size=224, keep_all=True, thresholds=[0.4, 0.5, 0.5], min_face_size=60)
mtcnn.detect_box = MethodType(detect_box, mtcnn)

# Process images
face_pictures = "./dataset/"
for root,_,files in os.walk(face_pictures):
    for filename in files:
        file = os.path.join(root,filename)


saved_pictures = "./dataset/"
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




# if __name__ == "__main__":
#     detect(0)

