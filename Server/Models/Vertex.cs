using System.Collections.Generic;
using Entities;

namespace Server.Models
{
    /// <summary>
    /// Vertex of Aho-Corasick.
    /// </summary>
    public class Vertex
    {
        public readonly Dictionary<char, Vertex> Direct, Next;
        public Vertex Link;
        public readonly Vertex Parent;

        public readonly char ParentCharacter;
        public Suspicious Suspicious;

        public Vertex(char parentCharacter, Vertex parent)
        {
            Parent = parent;
            ParentCharacter = parentCharacter;

            Direct = new Dictionary<char, Vertex>();
            Next = new Dictionary<char, Vertex>();
        }
    }
}