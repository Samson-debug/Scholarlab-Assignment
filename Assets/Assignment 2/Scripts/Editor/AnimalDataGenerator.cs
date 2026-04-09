using UnityEngine;
using UnityEditor;
using Assignment2;

public class AnimalDataGenerator : EditorWindow
{
    [MenuItem("Assignment2/Generate Animal Data")]
    public static void GenerateAnimals()
    {
        string baseSavePath = "Assets/Assignment 2/Data/Animals";
        
        // Ensure the directory exists
        if (!AssetDatabase.IsValidFolder("Assets/Assignment 2/Data"))
        {
            AssetDatabase.CreateFolder("Assets/Assignment 2", "Data");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Assignment 2/Data/Animals"))
        {
            AssetDatabase.CreateFolder("Assets/Assignment 2/Data", "Animals");
        }

        string[] animalInfo = new string[]
        {
            // Format: Name|Description|isFlying|isInsect|isOmnivorous|isGroup|isEggLaying|SpriteName
            "Ant|Ants form colonies that range in size from a few dozen predatory individuals living in small natural cavities to highly organised colonies that may occupy large territories and consist of millions of individuals.|false|true|true|true|true|Ant.png",
            "Bee|Bees are flying insects closely related to wasps and ants, known for their role in pollination and, in the case of the best-known bee species, the western honey bee, for producing honey. They live in colonies and can sting.|true|true|false|true|true|Bee.png",
            "Bat|Bats are mammals of the order Chiroptera. With their forelimbs adapted as wings, they are the only mammals capable of true and sustained flight. They are nocturnal animals.|true|false|true|true|false|Bat Face.png",
            "Bear|Bears are carnivoran mammals of the family Ursidae. They are classified as caniforms or doglike carnivorans. Although only eight species of bears are extant, they are widespread, appearing in a wide variety of habitats throughout the Northern Hemisphere and partially in the Southern Hemisphere.|false|false|true|false|false|Bear.png",
            "Sparrow|Sparrows are a family of small passerine birds. They are also known as true sparrows, or Old World sparrows, names also used for a particular genus of the family, Passer.|true|false|true|false|true|Bird.png",
            "Beetle|Beetles are a group of insects that form the order Coleoptera. The word \"coleoptera\" is from the Greek κολεός, koleos meaning \"sheath\"; and πτερόν, pteron meaning \"wing\", thus \"sheathed wing\", because most beetles have two pairs of wings, the front pair, the elytra, being hardened and thickened into a shell-like protection for the rear pair and the beetle's abdomen.|true|true|true|false|true|Bug.png",
            "Butterfly|Butterflies are insects in the macrolepidopteran clade Rhopalocera from the order Lepidoptera, which also includes moths. Adult butterflies have large, often brightly coloured wings, and conspicuous, fluttering flight.|true|true|false|false|true|Butterfly.png",
            "Cat|The domestic cat is a member of the Felidae family and is one of the most popular pets in the world. Domestic cats are often called house cats when kept as indoor pets.|false|false|true|false|false|Cat.png",
            "Chicken|The chicken is a type of domesticated fowl, a subspecies of the red junglefowl. It is one of the most common and widespread domestic animals.|false|false|true|true|true|Chicken.png",
            "Clown Fish|Clownfish or anemonefish are fishes from the subfamily Amphiprioninae in the family Pomacentridae. Thirty species are recognized: one in the genus Premnas, while the remaining are in the genus Amphiprion.|false|false|true|true|true|Clown Fish.png",
            "Dog|The domestic dog is a domesticated descendant of wolves. The dog descended from an ancient wolf ancestor that lived with humans over 15,000 years ago.|false|false|true|false|false|Dog.png",
            "Cow|Cattle—colloquially cows—are domesticated mammals that constitute the genus Bos, and they are most commonly classified collectively as Bos taurus.|false|false|false|true|false|Cow.png",
            "Deer|Deer (singular and plural) are hoofed ruminant mammals forming the family Cervidae. The two main groups of deer are the Cervinae, including muntjac, elk (wapiti), fallow deer and chital, and the Capreolinae, including reindeer (caribou), roe deer and moose.|false|false|false|true|false|Deer.png",
            "Dragonfly|A dragonfly is an insect belonging to the order Odonata, infraorder Anisoptera. Adult dragonflies are characterized by large multifaceted eyes, two pairs of strong transparent wings, sometimes with coloured patches, and an elongated body.|true|false|true|false|true|Dragonfly.png",
            "Duck|Duck is a common name for numerous species in waterfowl family Anatidae which also includes swans and geese.|true|false|false|true|true|Duck.png",
            "Falcon|Falcons are birds of prey in the genus Falco which includes about 40 species. Falcons are widely distributed on all continents except Antarctica.|true|false|true|false|true|Falcon.png",
            "Fly|True flies are insects of Diptera order which includes house flies, mosquitoes etc., characterized by having a single pair of wings on their mesothorax and halteres on their metathorax used for balance.|true|true|true|true|true|Fly.png",
            "Grasshopper|Grasshoppers are insects belonging to suborder Caelifera within order Orthoptera. They are herbivorous insects that can jump long distances.|false|true|true|false|true|Grasshopper.png"
        };

        int count = 0;
        foreach (string info in animalInfo)
        {
            string[] parts = info.Split('|');
            if (parts.Length < 8) continue;

            string anName = parts[0];
            string anDesc = parts[1];
            bool isFly = bool.Parse(parts[2]);
            bool isIns = bool.Parse(parts[3]);
            bool isOmni = bool.Parse(parts[4]);
            bool isGrp = bool.Parse(parts[5]);
            bool isEgg = bool.Parse(parts[6]);
            string sprName = parts[7];

            string assetPath = $"{baseSavePath}/{anName}.asset";

            // If an asset already exists, we can optionally load it to overwrite, 
            // but for a clean start we will just create a new instance and overwrite the file.
            AnimalDataSO so = AssetDatabase.LoadAssetAtPath<AnimalDataSO>(assetPath);
            bool isNew = false;
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<AnimalDataSO>();
                isNew = true;
            }
            
            so.animalName = anName;
            so.description = anDesc;
            so.isFlying = isFly;
            so.isInsect = isIns;
            so.isOmnivorous = isOmni;
            so.isGroup = isGrp;
            so.isEggLaying = isEgg;

            string spritePath = "Assets/Assignment 2/Sprites/" + sprName;
            Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sp != null)
            {
                so.animalSprite = sp;
            }
            else
            {
                Debug.LogWarning($"[AnimalDataGenerator] Sprite not found at: {spritePath}");
            }

            if (isNew)
            {
                AssetDatabase.CreateAsset(so, assetPath);
            }
            else
            {
                EditorUtility.SetDirty(so);
            }

            count++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[AnimalDataGenerator] Successfully generated/updated {count} Animal SOs in '{baseSavePath}'");
    }
}
