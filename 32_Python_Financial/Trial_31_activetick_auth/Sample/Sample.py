#https://api.activetick.com/en/index.html?python#requesting-session-tokens

import requests

# Define the API endpoint
url = "https://api.activetick.com/authorize.json"

# Define the request parameters
params = {
    "username": "xxx",
    "password": "xxx",
    "apikey": "xxx"
}

# Send the GET request
response = requests.get(url, params=params)

# Extract the JSON response
data = response.json()

# Print the output
print("Type: ", data["type"])
print("Status: ", data["status"])
print("Session ID: ", data["sessionid"])
print("Created At: ", data["created_at"])
print("Is Realtime: ", data["is_realtime"])