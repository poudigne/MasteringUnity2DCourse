﻿using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CommandButton : MonoBehaviour
{
    private CommandBar commandBar;
    public InventoryItem item;
    bool selected;

    public void Init (CommandBar bar)
    {
        this.commandBar = bar;
        gameObject.layer = bar.Layer;
        var collider = gameObject.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(1f, 1f);
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = bar.defaultButtonImage;
        renderer.sortingLayerName = "GUI";
        renderer.sortingOrder = 5;

    }

    public void AddInventoryItem(InventoryItem item)
    {
        this.item = item;
        var childGO = new GameObject("InventoryItemDisplayImage");
        var renderer = childGO.AddComponent<SpriteRenderer>();
        renderer.sprite = item.sprite;
        renderer.sortingLayerName = "GUI";
        renderer.sortingOrder = 10;
        renderer.transform.parent = this.transform;
        renderer.transform.localScale *= 4;
    }

    void UpdateSelection()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = selected ? commandBar.selectedButtonImage : commandBar.defaultButtonImage;
    }

    public void ClearSelection()
    {
        selected = false;
        UpdateSelection();
    }

    void OnMouseDown()
    {
        if (commandBar.CanSelectButton)
        {
            selected = !selected;
            UpdateSelection();
            commandBar.SelectButton(selected ? this : null);
            Debug.Log("Selected new weapon");
        }
    }
}
