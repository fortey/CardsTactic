using UnityEngine;
using UnityEngine.UI;

public class CreatureListItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _nameLabel;
    [SerializeField] private TMPro.TextMeshProUGUI _countLabel;
    [SerializeField] private Image _image;

    private string _name;
    private int _count;

    public void Initialize(UserCreature creature)
    {
        _name = creature.name;
        _count = creature.count;

        _nameLabel.text = _name;
        _countLabel.text = _count.ToString();
        _image.sprite = Global.Instance.CardSprites[_name];
    }

}
