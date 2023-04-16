namespace InvisibleManTUN.Models
{
    public class Flag
    {
        private string key;
        private string value;

        public string Key => key;

        public string Value => value;

        public Flag(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}