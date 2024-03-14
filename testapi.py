import requests
import urllib3
import os
import shutil

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
    response = requests.get(f"{BASE_URL}/Image/getimages", verify=False)
    # Check if the response is successful (status code 200)
    assert response.status_code == 200

    # Extract username and filename from the response JSON
    response_json = response.json()
    print(response_json)
    user_name = response_json.get('name')
    filename = response_json.get('filename')  # Assuming the key for filename in JSON is 'filename'

    # Create the dataset directory if it doesn't exist
    user_dir = os.path.join(DATASET_DIR, user_name)
    os.makedirs(user_dir, exist_ok=True)

    # Check if the directory was created
    assert os.path.isdir(user_dir)

    # Save the image with the given filename under the user's directory
    image_path = os.path.join(user_dir, filename)
    with open(image_path, "wb") as f:
        f.write(response.content)

    # Check if the file was created
    assert os.path.isfile(image_path)

    # Clean up after the test
    #shutil.rmtree(user_dir)

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
