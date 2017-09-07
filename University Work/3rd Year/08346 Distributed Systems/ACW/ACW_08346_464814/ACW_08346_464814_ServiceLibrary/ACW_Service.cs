using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;

namespace ACW_08346_464814_ServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ACW_Service : ACW_Interface_Service
    {
        static RSAParams rsaParams = new RSAParams();

        public void initRSAParams (string[] value)
        {
            // init RSA params from server input
            rsaParams.DHEX = value[0];
            rsaParams.DPHEX = value[1];
            rsaParams.DQHEX = value[2];
            rsaParams.ExponentHEX = value[3];
            rsaParams.InverseQHEX = value[4];
            rsaParams.ModulusHEX = value[5];
            rsaParams.PHEX = value[6];
            rsaParams.QHEX = value[7];
        }

        public string HELLOMessage(string message)
        {
            // read in id
            int id = int.Parse(message);

            Console.Write("Client No. {0} has contacted the server.\r\n", id);

            return "Hello";
        }

        public string PUBKEYMessage(string message)
        {
            // get pubkey and sent to client
            string output = rsaParams.ExponentHEX + "\r\n" + rsaParams.ModulusHEX;
            Console.Write("Sending the public key to the client.\r\n");

            return output;
        }

        public string SORTMessage(string message)
        {
            string output = "";

            // split by space char
            string[] stringValues = message.Split(' ');

            int numberOfValues = 0;

            // get array length
            numberOfValues = int.Parse(message[0].ToString());

            // delete array length value from array
            string[] values = stringValues.Skip(1).ToArray();

            Array.Sort(values);

            // add sorted values to output
            for (int i = 0; i < values.Length; i++)
            {
                output = string.Join(" ", values);
            }

            Console.Write("Sorted values:\r\n" + output + "\r\n");

            //TestEmail(output);

            return output;
        }

        public void ENCMessage(byte[] message)
        {
            // init RSA parameters
            RSAParameters encryptParams = new RSAParameters();
            encryptParams.D = StringToByteArray(rsaParams.DHEX);
            encryptParams.DP = StringToByteArray(rsaParams.DPHEX);
            encryptParams.DQ = StringToByteArray(rsaParams.DQHEX);
            encryptParams.Exponent = StringToByteArray(rsaParams.ExponentHEX);
            encryptParams.InverseQ = StringToByteArray(rsaParams.InverseQHEX);
            encryptParams.Modulus = StringToByteArray(rsaParams.ModulusHEX);
            encryptParams.P = StringToByteArray(rsaParams.PHEX);
            encryptParams.Q = StringToByteArray(rsaParams.QHEX);

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(encryptParams);

            byte[] encryptedByteMessage = message;
            byte[] decryptedByteMessage;

            decryptedByteMessage = RSADecrypt(encryptedByteMessage, RSA.ExportParameters(true));

            if (decryptedByteMessage != null)
            {
                string decryptedStringMessage = System.Text.Encoding.ASCII.GetString(decryptedByteMessage);
                Console.Write("Decrypted message is: " + decryptedStringMessage + ".\r\n");
            }
        }

        public string SHA1Message(string message)
        {
            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);

            byte[] sha1ByteMessage;
            SHA1 sha1Provider = new SHA1CryptoServiceProvider();
            sha1ByteMessage = sha1Provider.ComputeHash(asciiByteMessage);
            string sha1HexMessage = ByteArrayToHexString(sha1ByteMessage);

            Console.Write("SHA-1 hash of " + message + " is " + sha1HexMessage + ".\r\n");

            return sha1HexMessage;
        }

        public string SHA256Message(string message)
        {
            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);

            byte[] sha256ByteMessage;
            SHA256 sha256Provider = new SHA256CryptoServiceProvider();
            sha256ByteMessage = sha256Provider.ComputeHash(asciiByteMessage);
            string sha256HexMessage = ByteArrayToHexString(sha256ByteMessage);

            Console.Write("SHA-256 hash of " + message + " is " + sha256HexMessage + ".\r\n");

            return sha256HexMessage;
        }

        public string SIGNMessage(string message)
        {
            return "";
        }

        public string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (null != byteArray)
            {
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");
                }
            }
            return hexString;
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }
            catch (Exception e)
            {
                //SendEmail(e);
                Console.Write(e.ToString());
                return null;
            }
        }

        public void SendEmail(Exception e)
        {
            var mail = new MailMessage("distributedtest123@gmail.com", "distributedtest123@gmail.com");
            var client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Credentials = new NetworkCredential("distributedtest123@gmail.com", "elPassword1");

            mail.Subject = "Server Error";
            mail.Body = $"{e.Message}\r\n\r\n{e.StackTrace}";

            client.Send(mail);
        }

        public void TestEmail(string data)
        {
            var mail = new MailMessage("distributedtest123@gmail.com", "distributedtest123@gmail.com");
            var client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Credentials = new NetworkCredential("distributedtest123@gmail.com", "elPassword1");

            mail.Subject = "Server Test";
            mail.Body = data;

            client.Send(mail);
        }
    }
}
