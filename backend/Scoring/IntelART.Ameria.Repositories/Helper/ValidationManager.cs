using System.Text.RegularExpressions;

namespace IntelART.Ameria.Repositories
{
    public class ValidationManager
    {
        /// <summary>
        /// When changing the password, the method checks whether
        /// the new password meets the password requirements.
        /// </summary>
        public static void ValidatePasswordChange(string login, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            if (newPassword == oldPassword)
            {
                throw new ApplicationException("ERR-0030", "Հին և նոր գաղտնաբառերը համընկնում են");
            }

            ValidateCustomerPasswordCreation(login, newPassword, newPasswordRepeat);
        }

        /// <summary>
        /// When creating a new password for a customer, the method
        /// checks whether the password meets the password requirements.
        /// </summary>
        public static void ValidateCustomerPasswordCreation(string login, string newPassword, string newPasswordRepeat)
        {
            if (newPassword != newPasswordRepeat)
            {
                throw new ApplicationException("ERR-0031", "Մուտքագրել նոր գաղտնաբառը երկու անգամ");
            }

            ValidatePasswordCreation(login, newPassword);
        }

        /// <summary>
        /// When creating a new password, the method checks whether
        /// the password meets the password requirements.
        /// </summary>
        public static void ValidatePasswordCreation(string login, string password)
        {
            if (password.Contains(login))
            {
                throw new ApplicationException("ERR-0032", "Գաղտնաբառը չպետք է պարունակի օգտագործողի անունը");
            }

            if (!CheckPasswordCriteria(password))
            {
                throw new ApplicationException("ERR-0033", "Գաղտնաբառը պետք է լինի լատինատառ, պարունակի առնվազն ութ նիշ, մեկ մեծատառ, մեկ փոքրատառ և մեկ թվանշան");
            }
        }

        /// <summary>
        /// Defines password criteria and checks whether
        /// the given password meets them.
        /// </summary>
        private static bool CheckPasswordCriteria(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasLowerChar = new Regex(@"[a-z]+");
            Regex hasMinimum8Chars = new Regex(@".{8,}");

            return (hasNumber.IsMatch(password) &&
                    hasUpperChar.IsMatch(password) &&
                    hasLowerChar.IsMatch(password) &&
                    hasMinimum8Chars.IsMatch(password));
        }
    }
}
