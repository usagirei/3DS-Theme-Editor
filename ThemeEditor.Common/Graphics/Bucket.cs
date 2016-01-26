// --------------------------------------------------
// ThemeEditor.Common - Bucket.cs
// --------------------------------------------------

using System.Collections.Generic;

namespace ThemeEditor.Common.Graphics
{
    public class Bucket<T>
    {
        public int Count => Elements.Count;
        public List<T> Elements { get; } = new List<T>();
    }
}
