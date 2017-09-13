namespace DTcms.Common
{
    internal class Mac
    {
        private string accessKey;
        private string secretKey;

        public Mac(string accessKey, string secretKey)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
        }
    }
}