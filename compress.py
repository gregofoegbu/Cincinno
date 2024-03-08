# run this in any directory
# add -v for verbose
# get Pillow (fork of PIL) from
# pip before running -->
# pip install Pillow

# import required libraries
import os
import sys
from PIL import Image
from imutils import paths


# define a function for
# compressing an image
def compressMe(file, verbose=False):
    # Get the path of the file
    filepath = os.path.join(os.getcwd(),
                            file)

    # open the image
    picture = Image.open(filepath)

    # Save the picture with desired quality
    # To change the quality of image,
    # set the quality variable at
    # your desired level, The more
    # the value of quality variable
    # and lesser the compression

    picture.save(filepath,
                 optimize=True,
                 quality=10)
    return


# Define a main function
def compressPhotos():
    verbose = False

    # checks for verbose flag
    if (len(sys.argv) > 1):

        if (sys.argv[1].lower() == "-v"):
            verbose = True


    imagePaths = list(paths.list_images("dataset"))

    formats = ('.jpg', '.jpeg')

    for (i, imagePath) in enumerate(imagePaths):
        # extract the person name from the image path
        print("[INFO] Compressing image {}/{}".format(i + 1,
                                                     len(imagePaths)))
        name = imagePath.split(os.path.sep)[-2]


        compressMe(imagePath, verbose)

    print("Done")

if __name__ == "__main__":
    compressPhotos()
