import requests

base_url = "https://sef.podkolzin.consulting/api/users/lastSeen"

offset = 0  
page_size = 10  

while True:
    # Construct the API URL with the current offset
    api_url = f"{base_url}?offset={offset}"

    # Make a GET request to the API
    response = requests.get(api_url)

    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        json_data = response.json()
        user_data_list = json_data.get("data", [])

        # Process and print user information
        for user in user_data_list:
            print("User Information:")
            print(f"User ID: {user.get('userId')}")
            print(f"Nickname: {user.get('nickname')}")
            print(f"First Name: {user.get('firstName')}")
            print(f"Last Name: {user.get('lastName')}")
            print(f"Registration Date: {user.get('registrationDate')}")
            print(f"Is Online: {user.get('isOnline')}")
            print()
            

        # Check if there's more data to retrieve
        if len(user_data_list) < page_size:
            break  # Break the loop if no more data is available

        offset += page_size
