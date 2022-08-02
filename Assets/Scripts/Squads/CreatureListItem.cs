using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreatureListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI _nameLabel;
    //[SerializeField] private TMPro.TextMeshProUGUI _countLabel;
    [SerializeField] private Image _image;

    public Transform previousCell;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _transform;
    public Transform inventoryCell;

    private void Start()
    {
        inventoryCell = _transform.parent;
    }
    private string _name;
    public string Name { get => _name; }

    public void Initialize(UserCreature creature, Transform parent)
    {
        _name = creature.name;
        //_count = creature.count;

        _nameLabel.text = _name;
        //_countLabel.text = _count.ToString();
        _image.sprite = Global.Instance.CardSprites[_name];

        transform.SetParent(parent);
        inventoryCell = parent;
    }

    public void Initialize(string name)
    {
        _name = name;
        //_count = creature.count;

        _nameLabel.text = _name;
        //_countLabel.text = _count.ToString();
        _image.sprite = Global.Instance.CardSprites[_name];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_transform.parent.TryGetComponent<InventoryCell>(out InventoryCell invCell))
        {
            invCell.Pop();
            _transform.SetAsLastSibling();
        }
        _transform.SetParent(inventoryCell);
        _transform.parent.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _transform.localPosition = Vector3.zero;
        _canvasGroup.blocksRaycasts = true;

        if (_transform.parent.TryGetComponent<InventoryCell>(out InventoryCell invCell))
        {
            invCell.Push(this);
            if (previousCell && previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
            {
                squadCell.Clear();

            }
        }
        previousCell = _transform.parent;

    }

    public void SetDisable()
    {
        _canvasGroup.blocksRaycasts = false;
        _image.color = Color.gray;
    }

    // public void ReturnToInventory()
    // {

    // }
}
