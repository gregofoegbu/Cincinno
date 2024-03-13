import requests

BASE_URL = "http://localhost:7240"

# Test POST /api/data
data = {"key": "value"}
response = requests.post(f"{BASE_URL}/api/data", json=data)
print(response.json())

# Test PUT /api/data
data = {"key": "new_value"}
response = requests.put(f"{BASE_URL}/api/data", json=data)
print(response.json())

# Test DELETE /api/data/<id>
id = 123
response = requests.delete(f"{BASE_URL}/api/data/{id}")
print(response.json())

# Test POST /api/auth/login
login_data = {"Username": "example_username", "Password": "example_password"}
response = requests.post(f"{BASE_URL}/api/auth/login", json=login_data)
print(response.json())

# Test POST /api/auth/register
register_data = {"username": "new_user", "password": "password123"}
response = requests.post(f"{BASE_URL}/api/auth/register", json=register_data)
print(response.json())

# Test GET /api/images
response = requests.get(f"{BASE_URL}/api/images")
print(response.json())
