using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Interfaces
{
    public interface IImageProfile
    {
        int MaxSizeBytes { get; }
        IEnumerable<string> AllowedExtensions { get; }
        string BadExtensionError { get; }
    }
}
