using System.IO;
using Entities;

namespace Server.Models
{
    public class AhoCorasick
    {
        private readonly Vertex _root;

        public AhoCorasick()
        {
            _root = new Vertex('\0', null);
        }

        /// <summary>
        /// Adds suspicious to Aho-Corasick, terminal vertex stores suspicious.
        /// </summary>
        /// <param name="suspicious">to add</param>
        public void Add(Suspicious suspicious)
        {
            Vertex vertex = _root;
            foreach (var character in suspicious.BadString)
            {
                if (!vertex.Direct.ContainsKey(character))
                    vertex.Direct.Add(character, new Vertex(character, vertex));

                vertex = vertex.Direct[character];
            }

            if (vertex != _root)
                vertex.Suspicious = suspicious;
        }

        /// <summary>
        /// Processing stream letter by letter.
        /// If suspicious found, then update it.
        /// </summary>
        /// <param name="reader"></param>
        public void Process(StreamReader reader)
        {
            Vertex vertex = _root;

            while (!reader.EndOfStream)
            {
                char character = (char) reader.Read();
                vertex = CrossByCharacter(vertex, character);

                if (vertex.Suspicious != null)
                {
                    ++vertex.Suspicious.FilesCount;
                    break;
                }
            }
        }

        private Vertex GetLink(Vertex vertex)
        {
            if (vertex.Link == null)
            {
                if (vertex == _root || vertex.Parent == _root)
                    vertex.Link = _root;
                else
                    vertex.Link = CrossByCharacter(GetLink(vertex.Parent), vertex.ParentCharacter);
            }

            return vertex.Link;
        }

        private Vertex CrossByCharacter(Vertex vertex, char character)
        {
            if (!vertex.Next.ContainsKey(character))
            {
                if (vertex.Direct.ContainsKey(character))
                    vertex.Next.Add(character, vertex.Direct[character]);
                else if (vertex == _root)
                    vertex.Next.Add(character, _root);
                else
                    vertex.Next.Add(character, CrossByCharacter(GetLink(vertex), character));
            }

            return vertex.Next[character];
        }
    }
}