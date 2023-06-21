using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TradeNodeTextManager : MonoBehaviour
{
    public TradeNode tradeNode;
    public TMP_Text textMesh;

    // Start is called before the first frame update
    void Start()
    {
        tradeNode = GetComponentInParent<TradeNode>();
        textMesh = GetComponent<TMP_Text>();
        textMesh.text = tradeNode.name;
    }

    private void Update()
    {
        if (!tradeNode || !textMesh) return;
        string text = tradeNode.name;
        text += "\nResources Produced";
        text += "\n" + tradeNode.GetResourcesProducedAsString();
        text += "\nResources Needed";
        text += "\n" + tradeNode.GetResourcesNeededAsString();
        textMesh.text = text;
    }
}
