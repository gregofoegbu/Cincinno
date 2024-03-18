import requests
from io import BytesIO
import urllib3
import os
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

BASE_URL = "https://3.98.138.165/api/"
DATASET_DIR = "dataset"

# @app.route('/api/data', methods=['POST'])
# def post_data():
#     # Here you can process the data sent from the front end
#     data = request.get_json()
#     response = requests.post(f"{BASE_URL}/data", json=data, verify=False)
#     return jsonify(response.json()), response.status_code
#
# @app.route('/api/data', methods=['PUT'])
# def put_data():
#     # Here you can update the data
#     data = request.get_json()
#     response = requests.put(f"{BASE_URL}/data", json=data,verify=False)
#     return jsonify(response.json()), response.status_code
#
# @app.route('/api/data/<id>', methods=['DELETE'])
# def delete_data(id):
#     # Here you can delete the data with the given id
#     response = requests.delete(f"{BASE_URL}/data/{id}",verify=False)
#     return jsonify(response.json()), response.status_code
#
# @app.route('/api/auth/login', methods=['POST'])
# def login():
#     # Here you can authenticate the user
#     data = request.get_json()
#     username = data.get('Username')
#     password = data.get('Password')
#     # Authenticate the user using the username and password
#     response = requests.post(f"{BASE_URL}/login", json={"Username": username, "Password": password}, verify=False)
#     return jsonify(response.json()), response.status_code
#
# @app.route('/api/auth/register', methods=['POST'])
# def register():
#     # Here you can register a new user
#     data = request.get_json()
#     # Register the user using the data
#     response = requests.post(f"{BASE_URL}/register", json=data, verify=False)
#     return jsonify(response.json()), response.status_code


import os

def get_images():
    response = requests.get(f"{BASE_URL}/Image/getImages", verify=False)
    if response.status_code == 200:
        # Assuming the response contains image data in bytes
        image_bytes = response.content

        # Extract username and filename from the response JSON
        response_json = response.json()
        user_name = response_json.get('name')
        filename = response_json.get('filename')  # Assuming the key for filename in JSON is 'filename'

        # Create the dataset directory if it doesn't exist
        user_dir = os.path.join(DATASET_DIR, user_name)
        if not os.path.isdir(user_dir):
            os.makedirs(user_dir)

        # Save the image with the given filename under the user's directory
        image_path = os.path.join(user_dir, filename)
        with open(image_path, "wb") as f:
            f.write(image_bytes)

        return ({"message": "Image saved successfully", "image_path": image_path})
    else:
        return({"error": "Failed to get images"})


def get_userID():
    # Reading deviceId from a text file
    file_path = os.path.join(os.path.dirname(__file__), 'device_id.txt')
    with open(file_path, 'r') as file:
        deviceId = file.read().strip()
    if deviceId:
        response = requests.get(f"{BASE_URL}User/getuserId/{deviceId}", verify=False)
        if response.status_code == 200:
            json_data = response.json()
            userId = json_data
            return userId
        else:
            return ({"error": "Failed to retrieve user ID"})
    else:
        return ({"error": "Device ID is required"})

def get_threshold():
    user_id = get_userID()
    response = requests.get(f"{BASE_URL}User/GetUser/{user_id}", verify=False)
    if response.status_code == 200:
        try:
            json_data = response.json()
            if isinstance(json_data, dict):
                threshold = json_data.get('deviceThreshold')
                if threshold is not None:
                    return (threshold)
                else:
                    return ({"error": "Threshold is missing in the response"})
            else:
                return ({"error": "Response is not a JSON object"})
        except ValueError:
            return ({"error": "Response content is not in JSON format"})
    else:
        return ({"error": "Failed to get threshold"})

