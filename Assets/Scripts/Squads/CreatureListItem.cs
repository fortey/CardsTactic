using System.Collections.Generic;
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
    public string _name;
    public string Name { get => _name; }

    public void Initialize(UserCreature creature, Transform parent)
    {
        _name = creature.name;

        _nameLabel.text = _name;
        _image.sprite = Global.Instance.CardSprites[_name].sprite;

        _transform.SetParent(parent);
        _transform.localPosition = Vector3.zero;
        inventoryCell = parent;

        _canvasGroup.blocksRaycasts = true;
        _image.color = Color.white;
        previousCell = null;
    }

    public void Initialize(string name)
    {
        _name = name;

        _nameLabel.text = _name;
        if (_name == null)
            print("null");
        print(_name);
        _image.sprite = Global.Instance.CardSprites[_name].sprite;
        _transform.localPosition = Vector3.zero;
        _image.color = Color.white;
        previousCell = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_transform.parent.TryGetComponent<InventoryCell>(out InventoryCell invCell))
        {
            invCell.Pop();
            _transform.SetAsLastSibling();
        }
        else
        {
            _transform.SetParent(inventoryCell);
        }
        _transform.parent.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        _canvasGroup.blocksRaycasts = true;

        // if (_transform.parent.TryGetComponent<InventoryCell>(out InventoryCell invCell))
        // {
        //     invCell.Push(this);
        //     if (previousCell && previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
        //     {
        //         squadCell.Clear();

        //     }
        // }
        // previousCell = _transform.parent;

        List<RaycastResult> results = new List<RaycastResult>();
        Global.Instance.GraphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<SquadCell>(out SquadCell squadCell) && (previousCell != null || squadCell.listItem == null))
            {
                PutToCell(squadCell);
                return;
            }
        }
        PutToCell(null);
    }

    public void PutToCell(SquadCell cell, bool fromSquadCell = false)
    {
        if (cell)
        {
            if (!fromSquadCell)
                cell.SetItem(this);
            _transform.SetParent(cell.transform);

            previousCell = cell.transform;

        }
        else
        {
            _transform.SetParent(inventoryCell);
            if (inventoryCell.TryGetComponent<InventoryCell>(out InventoryCell invCell))
            {
                invCell.Push(this);
                if (previousCell && previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
                {
                    squadCell.Clear();

                }
            }
            previousCell = null;
        }
        _transform.localPosition = Vector3.zero;
    }

    public void SetDisable()
    {
        _canvasGroup.blocksRaycasts = false;
        _image.color = Color.gray;
    }

    public void SetBlockRaycasts(bool block)
    {
        _canvasGroup.blocksRaycasts = block;
    }

    // public void ReturnToInventory()
    // {

    // }
}
