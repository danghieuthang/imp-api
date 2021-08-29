using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IMP.WebApi.Policies
{
    public class HyphenNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(char.ToLower(name[0]));

            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    builder.Append('-');
                }
                builder.Append(char.ToLower(name[i]));
            }

            return builder.ToString();
        }
    }
}
