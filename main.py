import requests
from datetime import datetime, timedelta
import pytz

def get_current_time():
    return datetime.now(pytz.utc)

def get_last_seen_status(user):
    current_time = get_current_time()
    is_online = user.get('isOnline')

    if is_online:
        return "online"
    
    last_seen_str = user.get('lastSeenDate')

    if not last_seen_str:
        return "N/A"

    last_seen_str = last_seen_str.replace("Z", "+00:00")
    last_seen_time = datetime.fromisoformat(last_seen_str)
    last_seen_time = last_seen_time.replace(tzinfo=pytz.utc)
    time_elapsed = current_time - last_seen_time

    if time_elapsed < timedelta(seconds=30):
        return "just now"
    elif time_elapsed < timedelta(minutes=1):
        return "less than a minute ago"
    elif time_elapsed < timedelta(minutes=60):
        return "a couple of minutes ago"
    elif time_elapsed < timedelta(minutes=120):
        return "an hour ago"
    elif time_elapsed < timedelta(days=1):
        return "today"
    elif time_elapsed < timedelta(days=2):
        return "yesterday"
    elif time_elapsed < timedelta(days=7):
        return "this week"
    else:
        return "a long time ago"

def main():
    base_url = "https://sef.podkolzin.consulting/api/users/lastSeen"
    offset = 0  
    page_size = 20  

    while True:
        api_url = f"{base_url}?offset={offset}"
        response = requests.get(api_url)

        if response.status_code != 200:
            print(f"Failed to retrieve data. Status code: {response.status_code}")
            break

        json_data = response.json()
        user_data_list = json_data.get("data", [])

        for user in user_data_list:
            print("User Information:")
            print(f"Name: {user.get('firstName')}")
            print(f"Nickname: {user.get('nickname')}")
            last_seen_status = get_last_seen_status(user)
            print(f"Status: {last_seen_status}")
            print()

        if len(user_data_list) < page_size:
            break
        offset += page_size

if __name__ == "__main__":
    main()
