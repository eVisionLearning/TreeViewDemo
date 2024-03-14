using System.Security.Cryptography;
using System.Text;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo;

public class CoreHandler
{
    private static CoreHandler obj;
    private CoreHandler()
    {
        
    }

    public static CoreHandler GetInstance()
    {
        obj ??= new ();
        return obj;
    }
    
    private string GetEncryptionKey()
    {
        return "23oh2i3bufiyswg8ye23v";
    }

    public string Encrypt(string clearText)
    {
        var encryptionKey = GetEncryptionKey();
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        using Aes encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(clearBytes, 0, clearBytes.Length);
        cs.Close();
        clearText = Convert.ToBase64String(ms.ToArray());

        return clearText;
    }

    public string Decrypt(string cipherText)
    {
        try
        {
            if (string.IsNullOrEmpty(cipherText)) return "";
            var encryptionKey = GetEncryptionKey();
            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            using Aes encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
            cipherText = Encoding.Unicode.GetString(ms.ToArray());

            return cipherText;
        }
        catch (Exception ex)
        {
            // Log the exception or display a message to the user
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public AppUser GetLoggedInUser(AppDbContext context)
    {
        var cookie = context.HttpContextAccessor?.HttpContext?.Request.Cookies[Globals.AuthenticationCookieName];
        if (string.IsNullOrEmpty(cookie)) return null;
        var user = context.AppUserLoginHistories.Where(m => m.Token == cookie).Select(m => m.User).FirstOrDefault();
        return user;
    }
}