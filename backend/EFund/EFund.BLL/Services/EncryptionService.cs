using System.Security.Cryptography;
using System.Text;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.Configs;

namespace EFund.BLL.Services;

public class EncryptionService : IEncryptionService
{
    private readonly EncryptionConfig _config;

    private const int InitializationVectorLength = 16;

    public EncryptionService(EncryptionConfig config)
    {
        _config = config;
    }

    public byte[] Encrypt(string value)
    {
        using var aes = Aes.Create();

        aes.Key = Encoding.UTF8.GetBytes(_config.Key);
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using var swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(value);
        }

        // Concatenate IV and encrypted data for complete result
        return aes.IV.Concat(msEncrypt.ToArray()).ToArray();
    }

    public string Decrypt(byte[] value)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_config.Key);
        aes.IV = value.Take(InitializationVectorLength).ToArray();

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var msDecrypt = new MemoryStream(value.Skip(InitializationVectorLength).ToArray());
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}
