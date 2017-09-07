using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;

namespace ACW_08346_464814_ServiceLibrary
{
    [ServiceContract]
    public interface ACW_Interface_Service
    {
        [OperationContract]
        string HELLOMessage(string message);

        [OperationContract]
        string PUBKEYMessage(string message);

        [OperationContract]
        string SORTMessage(string message);

        [OperationContract]
        void ENCMessage(byte[] message);

        [OperationContract]
        string SHA1Message(string message);

        [OperationContract]
        string SHA256Message(string message);

        [OperationContract]
        string SIGNMessage(string message);
        void SendEmail(Exception e);
        void TestEmail(string data);
        string ByteArrayToHexString(byte[] byteArray);
        byte[] StringToByteArray(string hex);

        byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo);
    }

    [DataContract]
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
}
