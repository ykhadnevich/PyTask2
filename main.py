import requests
from datetime import datetime, timedelta
import pytz

base_url = "https://sef.podkolzin.consulting/api/users/lastSeen"

offset = 0  
page_size = 10  

current_time = datetime.now(pytz.utc)

while True:

    api_url = f"{base_url}?offset={offset}"
    response = requests.get(api_url)

    if response.status_code == 200:
        json_data = response.json()
        user_data_list = json_data.get("data", [])

        for user in user_data_list:
            print("User Information:")
            print(f"Nickname: {user.get('nickname')}")
            is_online = user.get('isOnline')

            if is_online:
                last_seen_status = "online"
            else:
                last_seen_str = user.get('lastSeenDate')

                if last_seen_str:

                    last_seen_str = last_seen_str.replace("Z", "+00:00")

                    last_seen_time = datetime.fromisoformat(last_seen_str)

                    last_seen_time = last_seen_time.replace(tzinfo=pytz.utc)

                    time_elapsed = current_time - last_seen_time

                    if time_elapsed < timedelta(seconds=30):
                        last_seen_status = "just now"
                    elif time_elapsed < timedelta(minutes=1):
                        last_seen_status = "less than a minute ago"
                    elif time_elapsed < timedelta(minutes=60):
                        last_seen_status = "a couple of minutes ago"
                    elif time_elapsed < timedelta(minutes=120):
                        last_seen_status = "an hour ago"
                    elif time_elapsed < timedelta(days=1):
                        last_seen_status = "today"
                    elif time_elapsed < timedelta(days=2):
                        last_seen_status = "yesterday"
                    elif time_elapsed < timedelta(days=7):
                        last_seen_status = "this week"
                    else:
                        last_seen_status = "a long time ago"
                else:
                    last_seen_status = "N/A"

            print(f"Status: {last_seen_status}")
            print()

        if len(user_data_list) < page_size:
            break  
        offset += page_size

