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




def test_get_userID():
    deviceId = '123456789'
    response = requests.get(f"{BASE_URL}User/getuserid/123456789", verify=False)
    assert response.status_code == 200, f"Failed to get user ID. Status code: {response.status_code}. Response content: {response.text}"
    try:
        json_data = response.json()
        if isinstance(json_data, dict):
            userId = json_data.get('userId')
        else:
            userId = json_data
        assert userId is not None, "User ID is missing in the response"
        assert isinstance(userId, str) or isinstance(userId, int), f"Expected 'userId' to be a string or integer, but got {type(userId)}"
    except ValueError:
        print(f"Response content is not in JSON format: {response.text}")



def test_get_threshold():
    deviceId = '123456789'  # Replace with a valid deviceId for the test
    response = requests.get(f"{BASE_URL}User/getuserid/{deviceId}", verify=False)
    assert response.status_code == 200, f"Failed to get user ID. Status code: {response.status_code}. Response content: {response.text}"
    try:
        json_data = response.json()
        if isinstance(json_data, dict):
            userId = json_data.get('userId')
        else:
            userId = json_data
        assert userId is not None, "User ID is missing in the response"
        assert isinstance(userId, str) or isinstance(userId, int), f"Expected 'userId' to be a string or integer, but got {type(userId)}"
    except ValueError:
        print(f"Response content is not in JSON format: {response.text}")

    response = requests.get(f"{BASE_URL}User/GetUser/{userId}", verify=False)
    assert response.status_code == 200, f"Failed to get threshold. Status code: {response.status_code}. Response content: {response.text}"
    try:
        json_data = response.json()
        if isinstance(json_data, dict):
            print(json_data)
            threshold = json_data.get('deviceThreshold')
            assert threshold is not None, "Threshold is missing in the response"
            # Add more assertions here if you have an expected value for 'threshold'
        else:
            print("Response is not a JSON object")
    except ValueError:
        print("Response content is not in JSON format")

if __name__ == "__main__":
    #test_get_images()
    test_get_userID()
    test_get_threshold()
    #test_get_images1()

    # test_post_data()
    # test_put_data()
    # test_delete_data()
    # Run other tests...
