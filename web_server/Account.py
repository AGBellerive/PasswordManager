#Account obj beacuse all accounts are in this format
class Account:
    def __init__(self, Site, Username, Email, Password, Others):
        self.Site = Site
        self.Username = Username
        self.Email = Email
        self.Password = Password
        self.Others = Others

    #convert obj to dict
    def to_dict(self):
        return {
            "Site": self.Site,
            "Username": self.Username,
            "Email": self.Email,
            "Password": self.Password,
            "Others": self.Others
        }
        