import sys
import os

# Add both the web_server folder and the project root to Python's path
# This allows imports to work regardless of where the script is executed from
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..', '..')))

import pytest
import json
import web_server.server as server

@pytest.fixture(autouse=True)
def setup_test_db(tmp_path, monkeypatch):
    # Create an isolated temporary JSON file for testing
    test_db = tmp_path / "test_passwords.json"
    test_db.write_text("[]", encoding="utf-8")
    
    # Patch PASSWORD_FILE path and ALLOWED_IPS
    monkeypatch.setattr(server, "PASSWORD_FILE", str(test_db))
    monkeypatch.setattr(server, "ALLOWED_IPS", ["127.0.0.1"])
    
    # Reset/clear ACCOUNTS dictionary before each test
    server.ACCOUNTS.clear()
    yield

@pytest.fixture
def client():
    server.app.config['TESTING'] = True
    with server.app.test_client() as client:
        yield client

def test_index_route(client):
    response = client.get('/')
    assert response.status_code == 200
    assert b'Password Manager API is running!' in response.data

def test_create_account(client):
    # Test creating a new account
    payload = {
        "Site": "GitHub",
        "Username": "testuser",
        "Email": "test@github.com",
        "Password": "supersecretpassword",
        "Others": "Personal account"
    }
    response = client.post('/createAccount', json=payload)
    assert response.status_code == 200
    data = json.loads(response.data)
    assert data["message"] == "Account created successfully!"
    
    # Verify it is stored
    assert "github" in server.ACCOUNTS
    assert server.ACCOUNTS["github"].Username == "testuser"

def test_create_account_empty_data(client):
    response = client.post('/createAccount', json={})
    assert response.status_code == 400
    data = json.loads(response.data)
    assert "error" in data

def test_read_accounts(client):
    # Seed account
    payload = {
        "Site": "GitHub",
        "Username": "testuser",
        "Email": "test@github.com",
        "Password": "supersecretpassword",
        "Others": "Personal account"
    }
    client.post('/createAccount', json=payload)
    
    # Read accounts
    response = client.get('/readAccounts')
    assert response.status_code == 200
    accounts = json.loads(response.data)
    assert len(accounts) == 1
    assert accounts[0]["Site"] == "GitHub"
    assert accounts[0]["Username"] == "testuser"

def test_update_account(client):
    # Seed account first
    payload = {
        "Site": "GitHub",
        "Username": "testuser",
        "Email": "test@github.com",
        "Password": "oldpassword",
        "Others": "Personal account"
    }
    client.post('/createAccount', json=payload)
    
    # Update password
    update_payload = {
        "Site": "GitHub",
        "Username": "testuser",
        "Email": "test@github.com",
        "Password": "newpassword",
        "Others": "Personal account updated"
    }
    response = client.post('/updateAccount', json=update_payload)
    assert response.status_code == 200
    data = json.loads(response.data)
    assert data["message"] == "Account updated successfully!"
    
    # Verify updated values
    assert server.ACCOUNTS["github"].Password == "newpassword"
    assert server.ACCOUNTS["github"].Others == "Personal account updated"

def test_update_account_not_found(client):
    payload = {
        "Site": "NonExistentSite",
        "Username": "testuser",
        "Email": "test@example.com",
        "Password": "password",
        "Others": ""
    }
    response = client.post('/updateAccount', json=payload)
    assert response.status_code == 404
    data = json.loads(response.data)
    assert data["error"] == "Account not found!"

def test_delete_account(client):
    # Seed account first
    payload = {
        "Site": "GitHub",
        "Username": "testuser",
        "Email": "test@github.com",
        "Password": "password",
        "Others": ""
    }
    client.post('/createAccount', json=payload)
    assert "github" in server.ACCOUNTS
    
    # Delete account
    delete_payload = {
        "Site": "GitHub"
    }
    response = client.post('/deleteAccount', json=delete_payload)
    assert response.status_code == 200
    data = json.loads(response.data)
    assert data["message"] == "Account deleted successfully!"
    
    # Verify deleted
    assert "github" not in server.ACCOUNTS

def test_delete_account_not_found(client):
    delete_payload = {
        "Site": "NonExistentSite"
    }
    response = client.post('/deleteAccount', json=delete_payload)
    assert response.status_code == 404
    data = json.loads(response.data)
    assert data["error"] == "Account not found!"

if __name__ == "__main__":
    pytest.main()
