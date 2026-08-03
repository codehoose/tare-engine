using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "TARE/Room Images")]
public class RoomImages : ScriptableObject
{
    public Texture2D[] images;

    public Texture2D GetImage(string slug) => images.FirstOrDefault(i => i.name == slug);
}
