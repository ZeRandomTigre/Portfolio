using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ACW_08346_464814_Client.ACW_Service_Reference;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace ACW_08346_464814_Client
{

    class Program
    {
        static void Main(string[] args)
        {
            string serverResponse = "";
            string clientInstruction = "";

            string[] message;

            int numberOfLines = 0;

            RSAParams rsaParams = new RSAParams();

            // setting up link to server
            ACW_Interface_ServiceClient ACW_Client = new ACW_Interface_ServiceClient();
            
            // read in first line - number of lines to follow
            numberOfLines = int.Parse(Console.ReadLine());

            // read in following lines - instructions for server
            for (int i = 0; i < numberOfLines; i++)
            {
                // read in instruction
                clientInstruction = Console.ReadLine();

                // split by space char
                message = clientInstruction.Split(new[] { ' ' }, 2);

                try
                { 
                    // HELLO  message
                    if (message[0].Equals("HELLO", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.HELLOMessage(message[1]);
                        Console.Write(serverResponse + "\r\n");
                    }
                    // PUBKEY message
                    else if (message[0].Equals("PUBKEY", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.PUBKEYMessage(message[0]);
                        Console.Write(serverResponse + "\r\n");

                        string[] responseSplit = serverResponse.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                        // update public key
                        rsaParams.ExponentHEX = responseSplit[0];
                        rsaParams.ModulusHEX = responseSplit[1];
                    }
                    // SORT message
                    else if (message[0].Equals("SORT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.SORTMessage(message[1]);
                        Console.Write("Sorted values:\r\n" + serverResponse + "\r\n");
                    }
                    // Encrpyt message
                    else if (message[0].Equals("ENC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        // exit if not public key
                        if (rsaParams.ExponentHEX == null && rsaParams.ModulusHEX == null)
                        {
                            Console.Write("No public key.\r\n");
                        }
                        // encrypt message and send to server
                        else
                        {
                            RSAParameters encryptParams = new RSAParameters();
                            encryptParams.Exponent = StringToByteArray(rsaParams.ExponentHEX);
                            encryptParams.Modulus = StringToByteArray(rsaParams.ModulusHEX);

                            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                            RSA.ImportParameters(encryptParams);

                            //TestEmail(clientInstruction);

                            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message[1]);
                            byte[] encryptedByteMessage;
                            
                            encryptedByteMessage = RSAEncrypt(asciiByteMessage, RSA.ExportParameters(false));

                            if (encryptedByteMessage != null)
                            {
                                ACW_Client.ENCMessage(encryptedByteMessage);
                                Console.Write("Encrypted message sent.\r\n");
                            }
                        }
                    }
                    // SHA1 message
                    else if (message[0].Equals("SHA1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.SHA1Message(message[1]);

                        Console.Write(serverResponse + "\r\n");
                    }
                    // SHA256 message
                    else if (message[0].Equals("SHA256", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.SHA256Message(message[1]);

                        Console.Write(serverResponse + "\r\n");
                    }
                    // SIGN message
                    else if (message[0].Equals("SIGN", StringComparison.CurrentCultureIgnoreCase))
                    {
                        serverResponse = ACW_Client.SIGNMessage(message[1]);

                        Console.Write(serverResponse + "\r\n");
                    }
                }
                catch (Exception e)
                {
                    //TestEmail(clientInstruction);
                    //SendEmail(e);
                    Environment.Exit(0);
                }

                
            }

            //Console.ReadLine();
        }

        /// <summary>
        /// Encrypt text in form "byte[]"
        /// </summary>
        /// <param name="DataToEncrypt">Text to encrpyt in form of byte array</param>
        /// <param name="RSAKeyInfo">RSA Parameters</param>
        /// <returns></returns>
        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                //SendEmail(e);
                Console.Write(e.Message);
                return null;
            }
        }

        public struct RSAParams
        {
            public string DHEX;
            public string DPHEX;
            public string DQHEX;
            public string ExponentHEX;
            public string InverseQHEX;
            public string ModulusHEX;
            public string PHEX;
            public string QHEX;

            public byte[] D;
            public byte[] DP;
            public byte[] DQ;
            public byte[] Exponent;
            public byte[] InverseQ;
            public byte[] Modulus;
            public byte[] P;
            public byte[] Q;
        }

        static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        // Send email for debugging
        static void SendEmail(Exception e)
        {
            var mail = new MailMessage("distributedtest123@gmail.com", "distributedtest123@gmail.com");
            var client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Credentials = new NetworkCredential("distributedtest123@gmail.com", "elPassword1");

            mail.Subject = "Client Error";
            mail.Body = $"{e.Message}\r\n\r\n{e.StackTrace}";

            client.Send(mail);
            Environment.Exit(0);
        }

        // send email for debugging
        static void TestEmail(string data)
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
