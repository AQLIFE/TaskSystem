namespace TaskManangerSystem.IServices.SystemServices{
    public interface ISE{
        public byte[] Encrypt(string plainText, byte[] Key, byte[] IV);
        public string Decrypt(byte[] cipherText, byte[] Key, byte[] IV);
    }

    public interface AsymmetricCryptographicAlgorithm{}
    public interface IACA{
        
    }


}