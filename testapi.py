import requests
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
BASE_URL = "https://3.98.138.165/api/"

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
    response = requests.get(f"{BASE_URL}images", verify=False)
    assert response.status_code == 200
    assert "images" in response.json()  # Assuming the response contains a key named "images"

# Add more tests for other endpoints...

if __name__ == "__main__":
    test_get_images()
    test_post_data()
    test_put_data()
    test_delete_data()
    # Run other tests...
