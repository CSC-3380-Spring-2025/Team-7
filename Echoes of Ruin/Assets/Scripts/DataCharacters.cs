using UnityEngine;
using System.Collections.Generic; 

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Data Characters Asset")]
public class DataCharacters : ScriptableObject 
{
    public List<DataCharacterEntry> characters = new List<DataCharacterEntry>();

    public int CharacterCount => characters?.Count ?? 0;

    public DataCharacterEntry GetCharacter(int index)
    {
        if (characters != null && index >= 0 && index < characters.Count)
        {
            return characters[index];
        }
        return null; 
    }

    public DataCharacterEntry GetCharacterByName(string name)
    {
        return characters.Find(character => character.characterName == name);
    }
}