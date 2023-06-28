using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    /**
     * This class is used specifically for the setup file which houses 
     * the path, hashed password, and the hint the user inputed during set up
     * this file can be found in the exe file location, bin/debug/setup.json
     */
    public class FileSetup
    {
        public string Path { get; set; }
        public int HashedPassword { get; set; }
        public string Hint { get; set; }
        public string LastLogin { get; set; }

        public FileSetup()
        {
            this.Path = "";
            this.HashedPassword = 0;
            this.Hint = "";
            this.LastLogin = "";
        }
    }
}
