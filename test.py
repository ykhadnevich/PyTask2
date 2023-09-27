import io
import unittest
import sys
from unittest.mock import Mock, patch
from main import get_last_seen_status, main

class TestMain(unittest.TestCase):

    @patch('main.requests.get')
    def test_get_last_seen_status_online(self, mock_get):

        mock_response = Mock()
        mock_response.status_code = 200
        mock_response.json.return_value = {
            "data": [{
                "nickname": "Alice",
                "isOnline": True,
                "lastSeenDate": None
            }]
        }
        mock_get.return_value = mock_response

        user = {"isOnline": True, "lastSeenDate": None}
        status = get_last_seen_status(user)
        self.assertEqual(status, "online")

    @patch('main.requests.get')
    def test_get_last_seen_status_a_couple_of_minutes_ago(self, mock_get):

        mock_response = Mock()
        mock_response.status_code = 200
        mock_response.json.return_value = {
            "data": [{
                "nickname": "Nick37",
                "isOnline": False,
                "lastSeenDate": "023-09-27T19:54:1Z"
            }]
        }
        mock_get.return_value = mock_response

        user = {"isOnline": False, "lastSeenDate": "2023-09-21T14:28:00Z"}
        status = get_last_seen_status(user)
        self.assertEqual(status, "this week")

    

    @patch('main.requests.get')
    def test_main_request_failure(self, mock_get):
        
        mock_response = Mock()
        mock_response.status_code = 404
        mock_get.return_value = mock_response

        
        with unittest.mock.patch('sys.stdout', new_callable=io.StringIO) as mock_stdout:
            main()

        
        self.assertIn("Failed to retrieve data. Status code: 404", mock_stdout.getvalue())

    

if __name__ == "__main__":
    unittest.main()
