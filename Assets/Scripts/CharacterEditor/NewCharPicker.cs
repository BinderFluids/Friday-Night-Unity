using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System.Xml.Linq;

public class NewCharPicker : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<XDocument> docs = new List<XDocument>(); 
    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChange(dropdown); });
    }

    private void OnDropdownChange(TMP_Dropdown change)
    {

    }
}
