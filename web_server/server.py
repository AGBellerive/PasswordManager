from flask import Flask, jsonify, request
import json
from Account import Account
import os
from threading import Lock

app = Flask(__name__)

file_lock = Lock()

# Loading config file
with open('config.json', 'r', encoding='utf-8') as f:
    config_file = json.load(f)

PASSWORD_FILE = config_file["PASSWORD_PATH"]
PORT = config_file["PORT"]
ALLOWED_IPS= config_file["ALLOWED_IP"]

#global variable
ACCOUNTS = {}

def parseData():
    global ACCOUNTS
    with file_lock:
        if len(ACCOUNTS) != 0:
            print("Data already parsed")
            return ACCOUNTS
        
        if not os.path.exists(PASSWORD_FILE):
            print("Password file not found")
            ACCOUNTS = {}
            return ACCOUNTS
        
        try:
            with open(PASSWORD_FILE, 'r', encoding='utf-8') as f:
                data = json.load(f)
        except json.JSONDecodeError:
            print("JSON file is empty")
            ACCOUNTS = {}
            return ACCOUNTS
        
        for entry in data:
            account = Account(
                Site=entry.get("Site", ""),
                Username=entry.get("Username", ""),
                Email=entry.get("Email", ""),
                Password=entry.get("Password", ""),
                Others=entry.get("Others", "")
            )
            ACCOUNTS[account.Site.lower()] = account
            
        print("Data Parsed")
        return ACCOUNTS

def writeToJSON():
    global ACCOUNTS
    with file_lock:
        # Sort alphabetically by Site name (case-insensitive) for clean file storage
        sorted_accounts = sorted(ACCOUNTS.values(), key=lambda x: x.Site.lower())
        with open(PASSWORD_FILE, 'w', encoding='utf-8') as f:
            json.dump([account.to_dict() for account in sorted_accounts], f, indent=4)
        print("Data saved to JSON file")
    return

def getAccount(request_data):
    parseData()
    search_data = Account(
        Site=request_data.get("Site", ""),
        Username=request_data.get("Username", ""),
        Email=request_data.get("Email", ""),
        Password=request_data.get("Password", ""),
        Others=request_data.get("Others", "")
    )
    
    return search_data

@app.before_request
def check_ip():
    client_ip = request.environ.get('REMOTE_ADDR')
    if client_ip not in ALLOWED_IPS:
        return jsonify({"error": "Access Denied"}), 403
    parseData()

@app.route('/')
def index():
    return 'Password Manager API is running!'

@app.route('/createAccount', methods=['POST'])
def create():
    global ACCOUNTS
    parseData()
    
    new_data = request.json
    if not new_data:
        return jsonify({"error": "No data provided!"}), 400
    new_account = getAccount(new_data)
    site_key = new_account.Site.lower()
    
    ACCOUNTS[site_key] = new_account
    writeToJSON()

    return jsonify({"message": "Account created successfully!"})


@app.route('/readAccounts', methods=['GET'])
def read():
    global ACCOUNTS
    parseData()
    # Sort alphabetically by Site name (case-insensitive) for output
    sorted_accounts = sorted(ACCOUNTS.values(), key=lambda x: x.Site.lower())
    return jsonify([account.to_dict() for account in sorted_accounts])

@app.route('/updateAccount', methods=['POST'])
def update():
    global ACCOUNTS
    parseData()
    
    update_data = request.json
    if not update_data:
        return jsonify({"error": "No data provided!"}), 400
    updated_account = getAccount(update_data)
    site_key = updated_account.Site.lower()

    if site_key in ACCOUNTS:
        ACCOUNTS[site_key] = updated_account
        writeToJSON()
        return jsonify({"message": "Account updated successfully!"})
    
    return jsonify({"error": "Account not found!"}), 404

@app.route('/deleteAccount', methods=['POST'])
def delete():
    global ACCOUNTS
    parseData()
    
    delete_data = request.json
    site_to_delete = delete_data.get("Site", "").lower()

    if site_to_delete in ACCOUNTS:
        del ACCOUNTS[site_to_delete]
        writeToJSON()
        return jsonify({"message": "Account deleted successfully!"})
        
    return jsonify({"error": "Account not found!"}), 404

if __name__ == '__main__':
    app.run(debug=True, port=PORT)

# # 1. Activate the virtual environment
# source ../.venv/Scripts/activate

# # 2. Install Flask inside the active environment
# pip install Flask
