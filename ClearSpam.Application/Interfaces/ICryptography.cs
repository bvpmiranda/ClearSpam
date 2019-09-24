using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSpam.Application.Interfaces
{
    public interface ICryptography
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
