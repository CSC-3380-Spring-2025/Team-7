using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject{
    public Character[] Character;

    // Gets the length of the character name
    public int CharacterCount{
        get{
            return Character.Length;
        }
    }
    
    // Gets character from the database
    public Character GetCharacter(int index){
        return Character[index];
    }
}
