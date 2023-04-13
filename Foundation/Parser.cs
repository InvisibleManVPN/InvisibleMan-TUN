using System.Linq;
using System.Collections.Generic;

namespace InvisibleManTUN.Foundation
{
    using Models;

    public class Parser
    {
        private List<Flag> flags;
        private readonly string[] validFlags;

        public Parser(string[] validFlags)
        {
            this.validFlags = validFlags;
        }

        public void Parse(string arguments)
        {
            Parse(arguments.Split(" "));
        }

        public void Parse(string[] arguments)
        {
            flags = new List<Flag>();

            if (!IsAnyFlagExists())
                return;

            foreach(string argument in arguments)
            {
                string[] flag = argument.Split("=");
                string key = flag.First();
                string value = flag.Last();

                if (IsEmptyFlag(key))
                    continue;

                if (!IsValidFlag(key))
                    return;

                flags.Add(new Flag(key, value));
            }

            bool IsAnyFlagExists() => arguments.Length > 0;

            bool IsEmptyFlag(string key) => string.IsNullOrWhiteSpace(key);

            bool IsValidFlag(string key) => validFlags.Contains(key);
        }

        public Flag GetFlag(string key) => flags.FirstOrDefault(flag => flag.Key == key);
    }
}