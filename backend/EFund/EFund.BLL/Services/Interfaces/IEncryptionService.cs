namespace EFund.BLL.Services.Interfaces;

public interface IEncryptionService
{
    byte[] Encrypt(string value);

    string Decrypt(byte[] value);
}
