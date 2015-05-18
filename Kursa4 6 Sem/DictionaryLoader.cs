using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI
{
    class DictionaryLoader
    {
        public static Dictionary<string, Texture2D> GetDictionary(string character, ContentManager content)
        {
            var result = new Dictionary<string, Texture2D>()
            {
                {"Stay", content.Load<Texture2D>("Moves//" + character + "//" + character + "Stay")},
                {"Win", content.Load<Texture2D>("Moves//" + character + "//" + character + "Win")},
                {"Move", content.Load<Texture2D>("Moves//" + character + "//" + character + "Move")},
                {"Block", content.Load<Texture2D>("Moves//" + character + "//" + character + "Block")},
                {"RightHandHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "RightHandHit")},
                {"LeftHandHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "LeftHandHit")},
                {"SitDown", content.Load<Texture2D>("Moves//" + character + "//" + character + "SitDown")},
                {"GroundHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "GroundHit")},
                {"DownBlock", content.Load<Texture2D>("Moves//" + character + "//" + character + "DownBlock")},
                {"AirHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "AirHit")},
                {"UppercodeStayHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "UppercodeStayHit")},
                {"UppercodeSitHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "UppercodeSitHit")},
                {"HandSitHit", content.Load<Texture2D>("Moves//" + character + "//" + character + "HandSitHit")},
                {"Sit", content.Load<Texture2D>("Moves//" + character + "//" + character + "Sit")},
            };
            return result;
        }
    }
}
