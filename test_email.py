#! /usr/bin/python

# Imports
import requests


def send_approved_message():
    print("I am sending an email.")
    return requests.post(
        "https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages",
        auth=("api", "YOUR_API_KEY"),
        data={"from": 'hello@example.com',
              "subject": "Visitor Alert",
              "html": "<html> Your Raspberry Pi recognizes an approved visitor. </html>"})

def send_unknown_message():
    print("I am sending an email.")
    return requests.post(
        "https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages",
        auth=("api", "YOUR_API_KEY"),
        data={"from": 'hello@example.com',
              "subject": "Visitor Alert",
              "html": "<html> Your Raspberry Pi has detected an unknown visitor. </html>"})

def send_motion_message():
    print("I am sending an email.")
    return requests.post(
        "https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages",
        auth=("api", "YOUR_API_KEY"),
        data={"from": 'hello@example.com',
              "subject": "Visitor Alert",
              "html": "<html> Your Raspberry Pi has detected an motion. </html>"})


def request_message(message):
    if message == 1:
        request = send_approved_message()
        print('Status: ' + format(request.status_code))
        print('Body:' + format(request.text))
    elif message == 2:
        request = send_unknown_message()
        print('Status: ' + format(request.status_code))
        print('Body:' + format(request.text))
    elif message == 3:
        request = send_motion_message()
        print('Status: ' + format(request.status_code))
        print('Body:' + format(request.text))
