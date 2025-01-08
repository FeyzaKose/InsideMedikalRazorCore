using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace AdaKurumsal.Tools
{
    public class Kit
    {

        public static bool ToBoolen(string str)
        {
            if (str == "True" || str == "true" || str == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetLanguage()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        }
        public static string convertEnglish(string str)
        {
            str = str.Replace("ı", "i")
                     .Replace("ö", "o")
                     .Replace("ü", "u")
                     .Replace("İ", "I")
                     .Replace("Ö", "O")
                     .Replace("Ü", "U")
                     .Replace("ç", "c")
                     .Replace("Ç", "c")
                     .Replace("ğ", "g")
                     .Replace("Ğ", "G")
                     .Replace("ş", "s")
                     .Replace("Ş", "S");

            return str;
        }
        public static string convertLink(string str)
        {
            str = convertEnglish(str.ToLower());
            str = str.Replace("&", "").Replace(" ", "-").Replace("?", "").Replace(".", "");
            return str;

        }
        public static string cleanPhone(string str)
        {
            if (str == null) return "";
            else return str.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("/", "");
        }
        public static string returnMount(int ay)
        {
            if (ay == 1) return "Ocak";
            else if (ay == 2) return "Şub";
            else if (ay == 3) return "Mart";
            else if (ay == 4) return "Nis";
            else if (ay == 5) return "May";
            else if (ay == 6) return "Haz";
            else if (ay == 7) return "Tem";
            else if (ay == 8) return "Agu";
            else if (ay == 9) return "Eyl";
            else if (ay == 10) return "Ekim";
            else if (ay == 11) return "Kas";
            else if (ay == 12) return "Ara";
            else return "";

        }
        public static string HashString(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyString(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public static string GenerateNewPassword(int size)
        {
            char[] cr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVYZ".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
            }
            return result;
        }
        public static string HtmlToString(string htmlPath)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(htmlPath))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
        public static string smtpport = "smtp.yandex.com.tr";
        public static int port = 587;
        public static string mailfrom = "web@3bkegitim.com.tr";
        public static string mailto = "3bk@3bkegitim.com.tr";
        public static string gonderenmailsifre = "6Ebi1820";
        //public static string mailfrom = "tasarimbalonu@yandex.com";
        //public static string mailto = "kose.feyza@gmail.com";
        //public static string gonderenmailsifre = "feyza6479feyza";

        internal static void MailSender(string body, string konu, string strmailto)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress(mailfrom);
            ePosta.To.Add(strmailto);
            ePosta.Subject = konu;
            ePosta.Body = body;
            ePosta.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(mailfrom, gonderenmailsifre);
            smtp.Port = port;
            smtp.Host = smtpport;
            smtp.EnableSsl = true;
            smtp.Send(ePosta);
        }

        public static void MailSender(string body, string konu)
        {
            var fromAddress = new MailAddress(mailfrom);
            var toAddress = new MailAddress(mailto);
            string subject = konu;
            using (var smtp = new SmtpClient
            {
                Host = smtpport,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, gonderenmailsifre)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }

        }

        public static List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }
            return randomList; //return the new random list
        }
        //public static bool isMobil()
        //{
        //    string userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        //    Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    string device_info = string.Empty;
        //    if (OS.IsMatch(userAgent))
        //    {
        //        device_info = OS.Match(userAgent).Groups[0].Value;
        //    }
        //    if (device.IsMatch(userAgent.Substring(0, 4)))
        //    {
        //        device_info += device.Match(userAgent).Groups[0].Value;
        //    }
        //    if (!string.IsNullOrEmpty(device_info))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
