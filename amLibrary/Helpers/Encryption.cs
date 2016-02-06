using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace amLibrary.Helpers
{
    public class Encryption
    {
        public static void RunSnippet()
        {
            // ENCRYPTING
            string result = SimpleEncrypt("Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.");
            WL(result);
            WL("");

            // DECRYPTING
            result = SimpleDecrypt("51196a80db5c51b8523220383de600fd116a947e00500d6b9101ed820d29f198c705000791c07ecc1e090213c688a4c7a421eae9c534b5eff91794ee079b15ecb862a22581c246e15333179302a7664d4be2e2384dc49dace30eba36546793be");
            WL(result);

            // PAUSE
            RL();
        }

        public static string SimpleEncrypt(string Data)
        {
            byte[] key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            byte[] iv = Encoding.ASCII.GetBytes("password");
            byte[] data = Encoding.ASCII.GetBytes(Data);
            byte[] enc = new byte[0];
            TripleDES tdes = TripleDES.Create();
            tdes.IV = iv;
            tdes.Key = key;
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.Zeros;
            ICryptoTransform ict = tdes.CreateEncryptor();
            enc = ict.TransformFinalBlock(data, 0, data.Length);
            return ByteArrayToString(enc);
        }

        public static string SimpleDecrypt(string Data)
        {
            byte[] key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            byte[] iv = Encoding.ASCII.GetBytes("password");
            byte[] data = StringToByteArray(Data);
            byte[] enc = new byte[0];
            TripleDES tdes = TripleDES.Create();
            tdes.IV = iv;
            tdes.Key = key;
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.Zeros;
            ICryptoTransform ict = tdes.CreateDecryptor();
            enc = ict.TransformFinalBlock(data, 0, data.Length);
            return Encoding.ASCII.GetString(enc);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        #region Helper methods

        public static void Main()
        {
            try
            {
                RunSnippet();
            }
            catch (Exception e)
            {
                string error = string.Format("---\nThe following error occurred while executing the snippet:\n{0}\n---", e.ToString());
                Console.WriteLine(error);
            }
            finally
            {
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void WL(object text, params object[] args)
        {
            Console.WriteLine(text.ToString(), args);
        }

        private static void RL()
        {
            Console.ReadLine();
        }

        private static void Break()
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}

        #endregion
/*** PHP FRIEND
 * 
 * ?php
// very simple ASCII key and IV
$key = "passwordDR0wSS@P6660juht";
$iv = "password";

$cipher = mcrypt_module_open(MCRYPT_3DES, '', 'cbc', '');

// ENCRYPTING
printvar(
  SimpleTripleDes('Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.'),
  'Encrypted:'
);

// DECRYPTING
printvar(
  SimpleTripleDesDecrypt('51196a80db5c51b8523220383de600fd116a947e00500d6b9101ed820d29f198c705000791c07ecc1e090213c688a4c7a421eae9c534b5eff91794ee079b15ecb862a22581c246e15333179302a7664d4be2e2384dc49dace30eba36546793be'),
  'Decrypted:'
);

function SimpleTripleDes($buffer) {
  global $key, $iv, $cipher;
  printvar($buffer, 'Encrypting:');

  // get the amount of bytes to pad
  $extra = 8 - (strlen($buffer) % 8);
  //printvar($extra, 'Padding with n zeros');

  // add the zero padding
  if($extra > 0) {
    for($i = 0; $i < $extra; $i++) {
      $buffer .= "\0";
    }
  }

  mcrypt_generic_init($cipher, $key, $iv);
  $result = bin2hex(mcrypt_generic($cipher, $buffer));
  mcrypt_generic_deinit($cipher);
  return $result;
}

function SimpleTripleDesDecrypt($buffer) {
  global $key, $iv, $cipher;
  printvar($buffer, 'Decrypting:');

  mcrypt_generic_init($cipher, $key, $iv);
  $result = rtrim(mdecrypt_generic($cipher, hex2bin($buffer)), "\0");
  mcrypt_generic_deinit($cipher);
  return $result;
}

function hex2bin($data)
{
  $len = strlen($data);
  return pack("H" . $len, $data);
} 

// HELPER FUNCTIONS

function printvar($var, $label="") {
    print "<pre style=\"border: 1px solid #999; background-color: #f7f7f7; color: #000; overflow: auto; width: auto; text-align: left; padding: 1em;\">" .
        (
            (
                strlen(
                    trim($label)
                )
            ) ? htmlentities($label)."\n===================\n" : ""
        ) .
        htmlentities(print_r($var, TRUE)) . "</pre>";
}
?>
 * 
 * 
 * 
 * 
 * **/