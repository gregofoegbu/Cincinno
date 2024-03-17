import cv2
from picamera import PiCamera
from picamera.array import PiRGBArray

cam = PiCamera()
cam.resolution = (1296, 976)
cam.framerate = 15
rawCapture = PiRGBArray(cam, size=(1296, 976))

img_counter = 0

def test():
    while True:
        for frame in cam.capture_continuous(rawCapture, format="bgr", use_video_port=True):
            image = frame.array
            image1 = cv2.resize(image, (512, 304))
            cv2.imshow("Press Space to take a photo", image1)
            rawCapture.truncate(0)

            k = cv2.waitKey(1)


if __name__ == "__main__":
    test()