using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Happy
{
    public class EncryptionService
    {
        private int[] p1 = { 84, 121, 108, 88, 55, 114, 89, 57, 51, 67, 120, 50, 89, 57, 35, 116, 88 };
        private string masterPass = "";

        public EncryptionService()
        {
            foreach (int i in p1)
            {
                this.masterPass += ((char)i).ToString();
            }
        }
        public void Encryption(string inputfile, string outputfile)
        {
            enc_method11(inputfile, outputfile);
        }
        public byte[] Encryption(byte[] inputfile)
        {
            return enc_method1(inputfile);
        }

        public byte[] Decryption(byte[] inputfile)
        {
            return dec_method1(inputfile);
        }

        public void Decryption(string input, string output)
        {
            dec_method11(input, output);
        }

        private byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }
            return data;
        }

        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

     //   private void FileEncrypt(string inputfile, string outPutFile)
      //  {
     //       enc_method11(inputfile, outPutFile);
      //  }


        public string EncryptPass(string inputfile)
        {

            return Encrypt(inputfile, this.masterPass);
        }

        public string DecryptPass(string inputfile)
        {
            return Decrypt(inputfile, this.masterPass);
        }
        /// <summary>
        /// 
        /// </summary>
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;

        private string Encrypt(string plainText, string passPhrase)
        {

            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        private string Decrypt(string cipherText, string passPhrase)
        {




            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        private byte[] enc_method1(byte[] inputfile)
        {
            try
            {
                MemoryStream memoryStream;
                CryptoStream cryptoStream;
                Rijndael rijndael = Rijndael.Create();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(this.masterPass, SALT);
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Mode = CipherMode.CBC;
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(inputfile, 0, inputfile.Length);
                cryptoStream.Close();
                return memoryStream.ToArray();
            }
            catch (Exception esc)
            {
                throw esc;
            }

        }
        private void enc_method11(string inputfile, string outputfile)
        {
            try
            {
                FileStream fsCrypt = new FileStream(outputfile, FileMode.Create);

                CryptoStream cryptoStream;
                Rijndael rijndael = Rijndael.Create();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(this.masterPass, SALT);
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Mode = CipherMode.CBC;
                cryptoStream = new CryptoStream(fsCrypt, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputfile, FileMode.Open);
                byte[] buffer = new byte[1048576];
                int read;

                try
                {
                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, read);
                    }
                    fsIn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    cryptoStream.Close();
                    fsCrypt.Close();
                }

            }
            catch (Exception esc)
            {
                throw esc;
            }

        }

        private byte[] dec_method1(byte[] inputfile)
        {
            try
            {
                MemoryStream memoryStream;
                CryptoStream cryptoStream;
                Rijndael rijndael = Rijndael.Create();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(this.masterPass, SALT);
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Mode = CipherMode.CBC;
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(inputfile, 0, inputfile.Length);
                cryptoStream.Close();
                return memoryStream.ToArray();

            }
            catch (Exception esc)
            {
                throw esc;
            }
        }
        private void dec_method11(string inputfile, string outputfile)
        {
            try
            {
                FileStream fsCrypt = new FileStream(outputfile, FileMode.Create);

                CryptoStream cryptoStream;
                Rijndael rijndael = Rijndael.Create();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(this.masterPass, SALT);
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Mode = CipherMode.CBC;
                cryptoStream = new CryptoStream(fsCrypt, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputfile, FileMode.Open);
                byte[] buffer = new byte[1048576];
                int read;

                try
                {
                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, read);
                    }
                    fsIn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    cryptoStream.Close();
                    fsCrypt.Close();
                }

            }
            catch (Exception esc)
            {
                throw esc;
            }
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);
        public static byte[] RandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

    }
}
