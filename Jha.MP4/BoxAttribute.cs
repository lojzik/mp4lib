using JHa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class BoxAttribute : Attribute
{
    public String4 Token { get; private set; }
    public BoxAttribute(string tokenName)
    {
        Token = tokenName;
    }
}
