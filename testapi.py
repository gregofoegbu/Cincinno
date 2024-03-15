import requests
import urllib3
import os
import shutil
from PIL import Image
from io import BytesIO
import base64

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
BASE_URL = "https://3.98.138.165/api/"
DATASET_DIR = "dataset"


def test_post_data():
    data = {"key": "value"}
    response = requests.post(f"{BASE_URL}data", json=data, verify=False)
    assert response.status_code == 200
    assert "success" in response.json()

def test_put_data():
    data = {"key": "new_value"}
    response = requests.put(f"{BASE_URL}data", json=data, verify=False)
    assert response.status_code == 200
    assert "success" in response.json()

def test_delete_data():
    id = 123
    response = requests.delete(f"{BASE_URL}data/{id}", verify=False)
    assert response.status_code == 200
    assert "success" in response.json()


def test_get_images():
    # Send request to get images
    response = requests.get(f"{BASE_URL}Image/getimages", verify=False)
    # Check if the response is successful (status code 200)
    assert response.status_code == 200
    response_json = response.json()
    for obj in response.json():
        user_name = obj['name']
        image_data_str = obj['data']
        filename = obj['filename']
        image_id = obj['id']
        image_data = base64.b64decode(image_data_str)

        # Create the dataset directory if it doesn't exist
        user_dir = os.path.join(DATASET_DIR, user_name)
        os.makedirs(user_dir, exist_ok=True)

        # Check if the directory was created
        assert os.path.isdir(user_dir)

        image = Image.open(BytesIO(image_data))

        # Save the image with the given filename under the user's directory
        # I have included a unique id to the filename incase the user manages to upload
        # two images with the same filename that are somehow different
        image_path = os.path.join(user_dir, str(image_id) + filename)
        image.save(image_path)

        # Check if the file was created
        assert os.path.isfile(image_path)

        # Clean up after the test
        # Note, the command below deletes the created directories
        # Useful to have for testing purposes. For actual integration, have to remove command
        shutil.rmtree(user_dir)

# Add more tests for other endpoints...


def test_get_images1(): #Base test: JSON will ALWAYS BE PRINTED
    response = requests.get(f"{BASE_URL}Image/getimages", verify=False)
    assert response.status_code == 200
    print(response.json())

if __name__ == "__main__":
    test_get_images()
    #test_get_images1()

    # test_post_data()
    # test_put_data()
    # test_delete_data()
    # Run other tests...
