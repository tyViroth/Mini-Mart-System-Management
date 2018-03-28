using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mart.ControlClasses
{
    public class ConvertHashCode
    {
        public static string ConvertPasswordToHashCode(string inputPasswordText)
        {
            byte[] bytePassword = Encoding.ASCII.GetBytes(inputPasswordText);

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(bytePassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            /*Copy salt and hash into hashBytes array*/
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool CompareDbHashWithInputHash(string dbHashPasswordText, string inputPasswordText)
        {
            bool success = true;
            try
            {
                byte[] dbHashBytes = Convert.FromBase64String(dbHashPasswordText.Trim());

                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(dbHashBytes, 0, salt, 0, 16);

                /* Compute the hash on the password the user entered */
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(inputPasswordText.Trim(), salt, 10000);
                byte[] inputHashBytes = pbkdf2.GetBytes(20);

                /* Compare the results */
                for (int i = 0; i < 20; i++)
                    if (dbHashBytes[i + 16] != inputHashBytes[i]) success = false;
            }
            catch (ArgumentException ex)
            {
                success = true;                   
            }                                                         
            return success;
        }

    }
}
