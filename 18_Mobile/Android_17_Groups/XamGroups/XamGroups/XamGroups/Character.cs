using System;
using System.Collections.Generic;
using System.Text;

namespace XamGroups
{
    public class Character
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string ImageUrl { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Species);
        }
        public static IList<Character> Characters
        {
            get
            {
                return new List<Character> {

                new Character
                {
                    Name = "Owen Lars",
                    Species = "człowiek",
                    ImageUrl = "http://vignette1.wikia.nocookie.net/starwars/images/9/91/OwenLarsHS-SWE.jpg/revision/latest?cb=20120428164235"
                },
                new Character
                {
                    Name = "C-3PO",
                    Species = "droid",
                    ImageUrl = "http://www.tomopop.com/ul/20046-550x-C-3PO_Bust_Header.jpg"
                },
                new Character
                {
                    Name = "R2-D2",
                    Species = "droid",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/39/R2-D2_Droid.png"
                },
                new Character
                {
                    Name = "Darth Vader",
                    Species = "człowiek",
                    ImageUrl = "http://cdn.bgr.com/2015/08/darth-vader.jpg"
                },
                new Character
                {
                    Name = "Luke Skywalker",
                    Species = "człowiek",
                    ImageUrl = "https://static1.comicvine.com/uploads/scale_small/0/3119/105663-148054-luke-skywalker.jpg"

                },
                new Character
                {
                    Name = "Obi-Wan Kenobi",
                    Species = "człowiek",
                    ImageUrl = "http://f.tqn.com/y/scifi/1/W/3/n/-/-/EP2-IA-60435_R_8x10.jpg"
                }
            };
            }
        }
    }
}

